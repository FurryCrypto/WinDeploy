using System.Text.Json;
using WinDeploy.Core.Models;
using WinDeploy.Core.Services;

var failures = new List<string>();

void Check(bool condition, string name)
{
    if (condition) Console.WriteLine($"PASS  {name}");
    else { Console.WriteLine($"FAIL  {name}"); failures.Add(name); }
}

try
{
    const string metadata = "\uFEFF" + """
        <WIM><IMAGE INDEX="3"><NAME>Windows 11 Pro</NAME><DESCRIPTION>Windows 11 Pro</DESCRIPTION><TOTALBYTES>15123456789</TOTALBYTES><WINDOWS><ARCH>9</ARCH><VERSION><MAJOR>10</MAJOR><MINOR>0</MINOR><BUILD>26100</BUILD><SPBUILD>1</SPBUILD></VERSION></WINDOWS></IMAGE></WIM>
        """;
    var edition = WimService.ParseEdition(metadata, 1);
    Check(edition.Index == 3 && edition.Name == "Windows 11 Pro", "WIM metadata index and name");
    Check(edition.Architecture == CpuArchitecture.X64 && edition.Build == 26100, "WIM architecture and build");
    Check(ImageService.GenerationFromBuild(26100) == WindowsGeneration.Windows11, "Windows 11 build detection");
    Check(ImageService.GenerationFromBuild(7601) == WindowsGeneration.Windows7, "Windows 7 build detection");

    var scratchPath = WimService.EnsureScratchDirectory();
    Check(Directory.Exists(scratchPath), "Writable WIMGAPI scratch directory");

    var destination = new PartitionInfo(1, 3, 1024 * 1024, 100L * 1024 * 1024 * 1024, 'W', "Target", "NTFS",
        "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee", "Basic", "ebd0a0a2-b9e5-4433-87c0-68b6b72699c7", 0,
        PartitionRole.BasicData, false, false, false, false, false, false, false, false, ["W:\\"]);
    var efi = new PartitionInfo(1, 1, 1024 * 1024, 260L * 1024 * 1024, null, "", "FAT32",
        "11111111-2222-3333-4444-555555555555", "System", "c12a7328-f81f-11d2-ba4b-00a0c93ec93b", 0,
        PartitionRole.EfiSystem, false, false, false, false, false, false, false, false, []);
    var disk = new DiskInfo(1, "Test SSD", "Test SSD", "SERIAL-1", "UNIQUE-1", "path", "NVMe",
        256L * 1024 * 1024 * 1024, PartitionScheme.Gpt, false, false, false, false, [efi, destination]);
    var imageEdition = new WindowsImageEdition(1, "Windows 11 Pro", "Windows 11 Pro", CpuArchitecture.X64, 26100,
        new Version(10, 0, 26100, 1), 15L * 1024 * 1024 * 1024);
    var image = new WindowsImage("C:\\test.iso", "D:\\sources\\install.wim", WindowsImageKind.Iso,
        WindowsGeneration.Windows11, "Windows 11", CpuArchitecture.X64, 5L * 1024 * 1024 * 1024,
        DateTime.UnixEpoch, "D:\\", [imageEdition]);
    var host = new CompatibilitySnapshot(FirmwareMode.Uefi, CpuArchitecture.X64, true, true, true, true,
        16L * 1024 * 1024 * 1024);
    var compatibility = new CompatibilityService(new ProcessRunner());
    var validation = compatibility.CheckImageCompatibility(image, imageEdition, disk, destination, efi, host);
    Check(validation.IsValid, "Valid Windows 11 UEFI/GPT plan");

    var unsupportedHost = host with
    {
        TpmPresent = false,
        TpmReady = false,
        SecureBootCapable = false,
        SecureBootEnabled = false,
        PhysicalMemoryBytes = 2L * 1024 * 1024 * 1024
    };
    var unsupportedBlocked = compatibility.CheckImageCompatibility(image, imageEdition, disk, destination, efi,
        unsupportedHost);
    Check(!unsupportedBlocked.IsValid && unsupportedBlocked.Errors.Any(issue => issue.Code == "win11-tpm"),
        "Unsupported Windows 11 hardware is blocked by default");
    var unsupportedBypassed = compatibility.CheckImageCompatibility(image, imageEdition, disk, destination, efi,
        unsupportedHost, bypassWindows11Requirements: true);
    Check(unsupportedBypassed.IsValid && unsupportedBypassed.Warnings.Any(issue => issue.Code == "win11-bypass") &&
          unsupportedBypassed.Warnings.Any(issue => issue.Code == "win11-memory"),
        "Advanced Windows 11 hardware bypass converts policy gates to warnings");

    var protectedDestination = destination with { IsCurrentWindows = true };
    var blocked = compatibility.CheckImageCompatibility(image, imageEdition, disk, protectedDestination, efi, host);
    Check(!blocked.IsValid && blocked.Errors.Any(issue => issue.Code == "protected-target"), "Current Windows partition is blocked");
    var protectedWithBypass = compatibility.CheckImageCompatibility(image, imageEdition, disk,
        protectedDestination, efi, unsupportedHost, bypassWindows11Requirements: true);
    Check(!protectedWithBypass.IsValid && protectedWithBypass.Errors.Any(issue => issue.Code == "protected-target"),
        "Windows 11 hardware bypass cannot bypass partition protection");
    var extendedContainer = destination with
    {
        PartitionNumber = 0,
        DriveLetter = null,
        FileSystem = string.Empty,
        Role = PartitionRole.Unknown
    };
    var unsupportedTarget = compatibility.CheckImageCompatibility(image, imageEdition, disk,
        extendedContainer, efi, host);
    Check(extendedContainer.IsProtected && !unsupportedTarget.IsValid &&
          unsupportedTarget.Errors.Any(issue => issue.Code == "unsupported-target"),
        "Unallocated and MBR extended-container targets are blocked before execution");

    var state = new SessionState
    {
        Image = image,
        Edition = imageEdition,
        DestinationDisk = disk,
        DestinationPartition = destination,
        BootPartition = efi,
        Compatibility = host,
        AdvancedMode = true,
        BypassWindows11Requirements = true
    };
    var plan = new InstallationPlanFactory().Create(state);
    var roundTrip = JsonSerializer.Deserialize<InstallationPlan>(JsonSerializer.Serialize(plan));
    Check(roundTrip?.DestinationPartition.OffsetBytes == destination.OffsetBytes && roundTrip.ConfirmationFingerprint.Length == 12,
        "Immutable plan JSON round-trip and fingerprint");
    Check(roundTrip?.BypassWindows11Requirements == true &&
          roundTrip.Operations.Any(operation => operation.Id == "win11-bypass"),
        "Windows 11 bypass is explicit in the immutable plan");

    if (OperatingSystem.IsWindows())
    {
        var runner = new ProcessRunner();
        var disks = await new DiskService(runner).GetDisksAsync();
        Check(disks.Count > 0, "Read-only physical disk enumeration");
        Check(disks.All(item => item.SizeBytes > 0 && item.Partitions.Count > 0), "Disk geometry is populated");
        var storageSizesResult = await runner.RunPowerShellAsync(
            "Get-Disk | ForEach-Object { Write-Output (\"{0}:{1}\" -f $_.Number, $_.Size) }");
        var storageSizes = storageSizesResult.StandardOutput.Split(Environment.NewLine,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => line.Split(':', 2))
            .Where(parts => parts.Length == 2 && int.TryParse(parts[0], out _) && long.TryParse(parts[1], out _))
            .ToDictionary(parts => int.Parse(parts[0]), parts => long.Parse(parts[1]));
        Check(storageSizesResult.Succeeded && disks.All(item => storageSizes.TryGetValue(item.Number, out var size) && size == item.SizeBytes),
            "Planned disk size uses the same Storage API as execution validation");
        var current = disks.SelectMany(item => item.Partitions).FirstOrDefault(item => item.IsCurrentWindows);
        Check(current is not null && current.IsProtected, "Running Windows partition is detected and protected");
        var actualHost = await new CompatibilityService(runner).InspectHostAsync();
        Check(actualHost.FirmwareMode != FirmwareMode.Unknown && actualHost.PhysicalMemoryBytes > 0,
            "Firmware and physical memory inspection");
        var readableError = await runner.RunPowerShellAsync("throw 'Readable PowerShell failure'");
        Check(!readableError.Succeeded && readableError.StandardError.Contains("Readable PowerShell failure") &&
              !readableError.StandardError.Contains("CLIXML", StringComparison.OrdinalIgnoreCase),
            "PowerShell failures are readable and do not expose CLIXML");

        var integrationImages = Environment.GetEnvironmentVariable("WINDEPLOY_TEST_IMAGES")?
            .Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];
        foreach (var integrationImage in integrationImages)
        {
            await using var imageService = new ImageService(runner, new WimService());
            var inspected = await imageService.InspectAsync(integrationImage);
            Check(inspected.Editions.Count > 0,
                $"Inspect {Path.GetFileName(integrationImage)} ({inspected.Editions.Count} edition(s))");
        }
    }
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    failures.Add("Unhandled test exception");
}

if (failures.Count == 0)
{
    Console.WriteLine("ALL TESTS PASSED");
    return 0;
}

Console.WriteLine($"{failures.Count} TEST(S) FAILED: {string.Join(", ", failures)}");
return 1;

using System.Text.Json;
using ESDInstaller.Core.Models;

namespace ESDInstaller.Core.Services;

public sealed class ImageService : IAsyncDisposable
{
    private readonly ProcessRunner _processRunner;
    private readonly WimService _wimService;
    private readonly HashSet<string> _mountedByThisProcess = new(StringComparer.OrdinalIgnoreCase);

    public ImageService(ProcessRunner processRunner, WimService wimService)
    {
        _processRunner = processRunner;
        _wimService = wimService;
    }

    public async Task<WindowsImage> InspectAsync(string sourcePath, CancellationToken cancellationToken = default)
    {
        sourcePath = Path.GetFullPath(sourcePath);
        if (!File.Exists(sourcePath)) throw new ESDInstallerException("ErrorImageNotFound", sourcePath);

        var extension = Path.GetExtension(sourcePath).ToLowerInvariant();
        return extension switch
        {
            ".iso" => await InspectIsoAsync(sourcePath, cancellationToken).ConfigureAwait(false),
            ".wim" => await InspectWimAsync(sourcePath, sourcePath, WindowsImageKind.Wim, null, cancellationToken).ConfigureAwait(false),
            ".esd" => await InspectWimAsync(sourcePath, sourcePath, WindowsImageKind.Esd, null, cancellationToken).ConfigureAwait(false),
            _ => throw new ESDInstallerException("ErrorUnsupportedImageType", extension)
        };
    }

    private async Task<WindowsImage> InspectIsoAsync(string sourcePath, CancellationToken cancellationToken)
    {
        const string script = """
            $ErrorActionPreference = 'Stop'
            $path = $env:ESDINSTALLER_IMAGE_PATH
            $image = Get-DiskImage -ImagePath $path -ErrorAction SilentlyContinue
            $already = $image -and $image.Attached
            if (-not $already) { $image = Mount-DiskImage -ImagePath $path -PassThru -NoDriveLetter:$false }
            $volume = $image | Get-Volume | Where-Object DriveLetter | Select-Object -First 1
            if (-not $volume) { throw 'The ISO was mounted but no drive letter was assigned.' }
            [pscustomobject]@{ Root = "$($volume.DriveLetter):\"; AlreadyAttached = [bool]$already } | ConvertTo-Json -Compress
            """;

        var result = await _processRunner.RunPowerShellAsync(script,
            new Dictionary<string, string?> { ["ESDINSTALLER_IMAGE_PATH"] = sourcePath },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            throw new ESDInstallerException("ErrorIsoMount", result.StandardError.Trim());
        }

        IsoMountResult? mount;
        try { mount = JsonSerializer.Deserialize<IsoMountResult>(ExtractJson(result.StandardOutput)); }
        catch (Exception exception) { throw new ESDInstallerException("ErrorIsoMount", result.StandardOutput, exception); }
        if (mount is null || string.IsNullOrWhiteSpace(mount.Root))
            throw new ESDInstallerException("ErrorIsoMount", "PowerShell did not return a mount root.");
        if (!mount.AlreadyAttached) _mountedByThisProcess.Add(sourcePath);

        var wim = Path.Combine(mount.Root, "sources", "install.wim");
        var esd = Path.Combine(mount.Root, "sources", "install.esd");
        if (File.Exists(wim)) return await InspectWimAsync(sourcePath, wim, WindowsImageKind.Iso, mount.Root, cancellationToken).ConfigureAwait(false);
        if (File.Exists(esd)) return await InspectWimAsync(sourcePath, esd, WindowsImageKind.Iso, mount.Root, cancellationToken).ConfigureAwait(false);

        if (Directory.Exists(Path.Combine(mount.Root, "I386")) &&
            (File.Exists(Path.Combine(mount.Root, "I386", "SETUPLDR.BIN")) || File.Exists(Path.Combine(mount.Root, "I386", "NTLDR"))))
        {
            var info = new FileInfo(sourcePath);
            return new WindowsImage(sourcePath, sourcePath, WindowsImageKind.LegacyIso, WindowsGeneration.WindowsXp,
                "Windows XP", CpuArchitecture.Unknown, info.Length, info.LastWriteTimeUtc, mount.Root,
                Array.Empty<WindowsImageEdition>(), "LegacyXpEngineUnavailable");
        }

        if (File.Exists(Path.Combine(mount.Root, "sources", "install.swm")))
            throw new ESDInstallerException("ErrorSplitWimUnsupported", "The ISO contains a split install.swm image.");
        throw new ESDInstallerException("ErrorNoInstallImage", mount.Root);
    }

    private async Task<WindowsImage> InspectWimAsync(string sourcePath, string imagePath, WindowsImageKind kind,
        string? mountedIsoPath, CancellationToken cancellationToken)
    {
        var editions = await _wimService.ReadEditionsAsync(imagePath, cancellationToken).ConfigureAwait(false);
        var build = editions.Select(edition => edition.Build).Where(value => value > 0).DefaultIfEmpty(0).Max();
        var generation = GenerationFromBuild(build);
        var architecture = editions.Select(edition => edition.Architecture).Distinct().Count() == 1
            ? editions[0].Architecture
            : CpuArchitecture.Unknown;
        var info = new FileInfo(sourcePath);
        var reason = generation == WindowsGeneration.WindowsVista ? "LegacyNt6EngineUnavailable" : null;
        return new WindowsImage(sourcePath, imagePath, kind, generation, DisplayVersion(generation, build), architecture,
            info.Length, info.LastWriteTimeUtc, mountedIsoPath, editions, reason);
    }

    public static WindowsGeneration GenerationFromBuild(int build) => build switch
    {
        >= 22000 => WindowsGeneration.Windows11,
        >= 10240 => WindowsGeneration.Windows10,
        >= 9600 => WindowsGeneration.Windows81,
        >= 9200 => WindowsGeneration.Windows8,
        >= 7600 => WindowsGeneration.Windows7,
        >= 6000 => WindowsGeneration.WindowsVista,
        _ => WindowsGeneration.Unknown
    };

    private static string DisplayVersion(WindowsGeneration generation, int build) => generation switch
    {
        WindowsGeneration.Windows11 => "Windows 11",
        WindowsGeneration.Windows10 => "Windows 10",
        WindowsGeneration.Windows81 => "Windows 8.1",
        WindowsGeneration.Windows8 => "Windows 8",
        WindowsGeneration.Windows7 => "Windows 7",
        WindowsGeneration.WindowsVista => "Windows Vista",
        _ => build > 0 ? $"Windows (build {build})" : "Windows"
    };

    private static string ExtractJson(string text)
    {
        var start = text.IndexOf('{');
        var end = text.LastIndexOf('}');
        return start >= 0 && end >= start ? text[start..(end + 1)] : text.Trim();
    }

    public async ValueTask DisposeAsync()
    {
        const string script = "$ErrorActionPreference='SilentlyContinue'; Dismount-DiskImage -ImagePath $env:ESDINSTALLER_IMAGE_PATH";
        foreach (var path in _mountedByThisProcess.ToArray())
        {
            await _processRunner.RunPowerShellAsync(script,
                new Dictionary<string, string?> { ["ESDINSTALLER_IMAGE_PATH"] = path }).ConfigureAwait(false);
        }
        _mountedByThisProcess.Clear();
    }

    private sealed record IsoMountResult(string Root, bool AlreadyAttached);
}

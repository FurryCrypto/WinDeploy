using System.Security.Cryptography;
using System.Text;
using WinDeploy.Core.Models;

namespace WinDeploy.Core.Services;

public sealed class InstallationPlanFactory
{
    public InstallationPlan Create(SessionState session, bool requestOneTimeBoot = false)
    {
        var image = session.Image ?? throw new InvalidOperationException("No image was selected.");
        var edition = session.Edition ?? throw new InvalidOperationException("No edition was selected.");
        var disk = session.DestinationDisk ?? throw new InvalidOperationException("No destination disk was selected.");
        var partition = session.DestinationPartition ?? throw new InvalidOperationException("No destination partition was selected.");
        var boot = session.BootPartition ?? throw new InvalidOperationException("No valid boot partition was selected.");
        var compatibility = session.Compatibility ?? throw new InvalidOperationException("Compatibility was not inspected.");

        var source = new SourceIdentity(image.SourcePath, image.ResolvedImagePath, image.Kind,
            image.FileSizeBytes, image.SourceLastWriteUtc);
        var diskIdentity = new DiskIdentity(disk.Number, disk.UniqueId, disk.SerialNumber, disk.SafeDisplayName,
            disk.SizeBytes, disk.PartitionScheme);
        var targetIdentity = ToIdentity(partition);
        var bootIdentity = ToIdentity(boot);
        var operations = new List<PlannedOperation>
        {
            new("validate", "OperationValidatePlan", false),
            new("format", "OperationFormatDestination", true),
            new("apply", "OperationApplyImage", true)
        };
        if (session.BypassWindows11Requirements)
            operations.Add(new("win11-bypass", "OperationConfigureWindows11Bypass", true));
        operations.Add(new("boot", "OperationInstallBootFiles", true));
        operations.Add(new("verify", "OperationVerifyInstallation", false));
        var fingerprint = Fingerprint(source, edition, diskIdentity, targetIdentity, bootIdentity,
            session.BypassWindows11Requirements);
        return new InstallationPlan(Guid.NewGuid(), DateTime.UtcNow, source, edition, image.Generation,
            SelectEngine(image), diskIdentity, targetIdentity, bootIdentity, compatibility.FirmwareMode,
            disk.PartitionScheme, true, session.BypassWindows11Requirements, false, requestOneTimeBoot,
            operations, fingerprint);
    }

    private static InstallationEngineKind SelectEngine(WindowsImage image) => image.Generation switch
    {
        WindowsGeneration.WindowsXp => InstallationEngineKind.LegacyXpNt5,
        WindowsGeneration.WindowsVista => InstallationEngineKind.LegacyNt6,
        _ => InstallationEngineKind.ModernWindows
    };

    private static PartitionIdentity ToIdentity(PartitionInfo partition) => new(
        partition.DiskNumber, partition.PartitionNumber, partition.OffsetBytes, partition.LengthBytes,
        partition.PartitionGuid, partition.DriveLetter, partition.VolumeLabel, partition.FileSystem, partition.Role);

    private static string Fingerprint(SourceIdentity source, WindowsImageEdition edition, DiskIdentity disk,
        PartitionIdentity destination, PartitionIdentity boot, bool bypassWindows11Requirements)
    {
        var value = string.Join('|', source.SourcePath, source.SourceSizeBytes, source.SourceLastWriteUtc.Ticks,
            edition.Index, edition.Build, disk.UniqueId, disk.SerialNumber, disk.SizeBytes,
            destination.PartitionNumber, destination.OffsetBytes, destination.LengthBytes, destination.PartitionGuid,
            boot.PartitionNumber, boot.OffsetBytes, boot.LengthBytes, boot.PartitionGuid,
            bypassWindows11Requirements);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)))[..12];
    }
}

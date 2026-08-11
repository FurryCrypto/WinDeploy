namespace ESDInstaller.Windows7.Core.Models;

public sealed class SessionState
{
    public WindowsImage? Image { get; set; }
    public WindowsImageEdition? Edition { get; set; }
    public IReadOnlyList<DiskInfo> Disks { get; set; } = Array.Empty<DiskInfo>();
    public DiskInfo? DestinationDisk { get; set; }
    public PartitionInfo? DestinationPartition { get; set; }
    public PartitionInfo? BootPartition { get; set; }
    public CompatibilitySnapshot? Compatibility { get; set; }
    public InstallationPlan? Plan { get; set; }
    public bool AdvancedMode { get; set; }
    public bool BypassWindows11Requirements { get; set; }
}

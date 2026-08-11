namespace ESDInstaller.Windows8.Core.Models;

public sealed record PartitionInfo(
    int DiskNumber,
    int PartitionNumber,
    long OffsetBytes,
    long LengthBytes,
    char? DriveLetter,
    string VolumeLabel,
    string FileSystem,
    string PartitionGuid,
    string Type,
    string GptType,
    int MbrType,
    PartitionRole Role,
    bool IsActive,
    bool IsBoot,
    bool IsSystem,
    bool IsCurrentWindows,
    bool IsBitLocker,
    bool IsReadOnly,
    bool IsOffline,
    bool IsUnallocated,
    IReadOnlyList<string> AccessPaths)
{
    public string StableKey => $"{DiskNumber}:{OffsetBytes}:{LengthBytes}:{PartitionGuid}";
    public bool IsProtected => IsCurrentWindows || IsBoot || IsSystem || IsReadOnly || IsOffline ||
        Role is PartitionRole.EfiSystem or PartitionRole.MicrosoftReserved or PartitionRole.Recovery or
            PartitionRole.Oem or PartitionRole.Unallocated or PartitionRole.Unknown;
    public string DriveDisplay => DriveLetter.HasValue ? $"{DriveLetter.Value}:" : string.Empty;
}

public sealed record DiskInfo(
    int Number,
    string FriendlyName,
    string Model,
    string SerialNumber,
    string UniqueId,
    string Path,
    string BusType,
    long SizeBytes,
    PartitionScheme PartitionScheme,
    bool IsBoot,
    bool IsSystem,
    bool IsReadOnly,
    bool IsOffline,
    IReadOnlyList<PartitionInfo> Partitions)
{
    public string StableKey => $"{UniqueId}|{SerialNumber}|{SizeBytes}";
    public string SafeDisplayName => string.IsNullOrWhiteSpace(Model) ? FriendlyName : Model;
}

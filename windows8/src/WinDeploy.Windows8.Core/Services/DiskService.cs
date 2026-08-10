using System.Management;
using WinDeploy.Windows8.Core.Models;

namespace WinDeploy.Windows8.Core.Services;

public sealed class DiskService
{
    private const string StorageNamespace = @"\\.\root\Microsoft\Windows\Storage";

    public Task<IReadOnlyList<DiskInfo>> GetDisksAsync(CancellationToken cancellationToken = default) =>
        Task.Run(() => Enumerate(cancellationToken), cancellationToken);

    private static IReadOnlyList<DiskInfo> Enumerate(CancellationToken cancellationToken)
    {
        var result = new List<DiskInfo>();
        try
        {
            var storageDisks = ReadStorageDisks();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
            using (var disks = searcher.Get())
            {
                foreach (ManagementObject disk in disks)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var number = ToInt(disk["Index"]);
                    storageDisks.TryGetValue(number, out var storageDisk);
                    var partitions = EnumeratePartitions(number, cancellationToken);
                    var size = storageDisk?.SizeBytes ?? ToLong(disk["Size"]);
                    AddUnallocatedRegions(partitions, number, size);
                    var scheme = storageDisk?.PartitionScheme ?? InferPartitionScheme(partitions);
                    var model = Text(disk["Model"]);
                    var serial = FirstText(storageDisk?.SerialNumber, Text(disk["SerialNumber"]).Trim());
                    var uniqueId = FirstText(storageDisk?.UniqueId, Text(disk["PNPDeviceID"]).Trim());
                    var path = FirstText(storageDisk?.Path, Text(disk["DeviceID"]));
                    var busType = storageDisk == null ? Text(disk["InterfaceType"]) : BusTypeName(storageDisk.BusType);
                    result.Add(new DiskInfo(number, FirstText(storageDisk?.FriendlyName, model), model, serial,
                        uniqueId, path, busType, size, scheme,
                        storageDisk?.IsBoot ?? partitions.Any(p => p.IsCurrentWindows),
                        storageDisk?.IsSystem ?? partitions.Any(p => p.IsSystem),
                        storageDisk?.IsReadOnly ?? false, storageDisk?.IsOffline ?? false,
                        partitions.OrderBy(p => p.OffsetBytes).ToArray()));
                }
            }
        }
        catch (Exception exception)
        { throw new WinDeployException("ErrorDiskEnumeration", exception.Message, exception); }
        return result.OrderBy(d => d.Number).ToArray();
    }

    private static List<PartitionInfo> EnumeratePartitions(int diskNumber,
        CancellationToken cancellationToken)
    {
        var result = new List<PartitionInfo>();
        var storagePartitions = ReadStoragePartitions(diskNumber);
        var query = "SELECT * FROM Win32_DiskPartition WHERE DiskIndex=" + diskNumber;
        using (var searcher = new ManagementObjectSearcher(query))
        using (var partitions = searcher.Get())
        {
            foreach (ManagementObject partition in partitions)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var type = Text(partition["Type"]);
                if (type.IndexOf("Extended", StringComparison.OrdinalIgnoreCase) >= 0) continue;
                var offset = ToLong(partition["StartingOffset"]);
                var length = ToLong(partition["Size"]);
                var storage = FindStoragePartition(storagePartitions, offset, length);
                var logical = FindLogicalDisk(partition);
                var letterText = logical == null ? string.Empty : Text(logical["DeviceID"]);
                char? letter = letterText.Length > 0 ? char.ToUpperInvariant(letterText[0]) : (char?)null;
                var current = letter.HasValue && string.Equals(letter + ":", Environment.GetEnvironmentVariable("SystemDrive"),
                    StringComparison.OrdinalIgnoreCase);
                var fileSystem = Text(logical == null ? null : logical["FileSystem"]);
                var role = RoleFromMetadata(storage?.GptType, storage?.MbrType ?? 0, type, fileSystem,
                    storage?.IsSystem ?? false);
                var active = storage?.IsActive ?? ToBool(partition["Bootable"]);
                var isBoot = storage?.IsBoot ?? current;
                var isSystem = ResolveSystemFlag(storage != null, storage?.IsSystem ?? false, active, current,
                    letter.HasValue, role);
                var paths = storage?.AccessPaths.Length > 0
                    ? storage.AccessPaths
                    : letter.HasValue ? new[] { letter + ":\\" } : Array.Empty<string>();
                result.Add(new PartitionInfo(diskNumber,
                    storage?.PartitionNumber ?? (ToInt(partition["Index"]) + 1), offset, length, letter,
                    Text(logical == null ? null : logical["VolumeName"]), fileSystem,
                    NormalizeGuid(storage?.PartitionGuid), type, NormalizeGuid(storage?.GptType), storage?.MbrType ?? 0,
                    role, active, isBoot, isSystem, current,
                    letter.HasValue && IsBitLocker(letter.Value), storage?.IsReadOnly ?? false,
                    storage?.IsOffline ?? false, false, paths));
                logical?.Dispose();
            }
        }
        return result;
    }

    private static Dictionary<int, StorageDiskMetadata> ReadStorageDisks()
    {
        var result = new Dictionary<int, StorageDiskMetadata>();
        try
        {
            var scope = new ManagementScope(StorageNamespace);
            scope.Connect();
            using (var searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM MSFT_Disk")))
            using (var disks = searcher.Get())
            {
                foreach (ManagementObject disk in disks)
                {
                    var number = ToInt(disk["Number"]);
                    result[number] = new StorageDiskMetadata(number, Text(disk["FriendlyName"]),
                        Text(disk["SerialNumber"]).Trim(), Text(disk["UniqueId"]).Trim(), Text(disk["Path"]),
                        ToInt(disk["BusType"]), ToLong(disk["Size"]), ParsePartitionScheme(ToInt(disk["PartitionStyle"])),
                        ToBool(disk["IsBoot"]), ToBool(disk["IsSystem"]), ToBool(disk["IsReadOnly"]),
                        ToBool(disk["IsOffline"]));
                }
            }
        }
        catch (ManagementException) { }
        catch (UnauthorizedAccessException) { }
        return result;
    }

    private static List<StoragePartitionMetadata> ReadStoragePartitions(int diskNumber)
    {
        var result = new List<StoragePartitionMetadata>();
        try
        {
            var scope = new ManagementScope(StorageNamespace);
            scope.Connect();
            using (var searcher = new ManagementObjectSearcher(scope,
                       new ObjectQuery("SELECT * FROM MSFT_Partition WHERE DiskNumber=" + diskNumber)))
            using (var partitions = searcher.Get())
            {
                foreach (ManagementObject partition in partitions)
                {
                    result.Add(new StoragePartitionMetadata(ToInt(partition["PartitionNumber"]),
                        ToLong(partition["Offset"]), ToLong(partition["Size"]), Text(partition["Guid"]),
                        Text(partition["GptType"]), ToInt(partition["MbrType"]), ToBool(partition["IsActive"]),
                        ToBool(partition["IsBoot"]), ToBool(partition["IsSystem"]), ToBool(partition["IsReadOnly"]),
                        ToBool(partition["IsOffline"]), TextArray(partition["AccessPaths"])));
                }
            }
        }
        catch (ManagementException) { }
        catch (UnauthorizedAccessException) { }
        return result;
    }

    private static StoragePartitionMetadata? FindStoragePartition(IEnumerable<StoragePartitionMetadata> partitions,
        long offset, long length) => partitions.FirstOrDefault(p => p.OffsetBytes == offset && p.LengthBytes == length)
                                ?? partitions.FirstOrDefault(p => p.OffsetBytes == offset);

    private static ManagementObject? FindLogicalDisk(ManagementObject partition)
    {
        try
        {
            using (var logical = partition.GetRelated("Win32_LogicalDisk"))
                return logical.Cast<ManagementObject>().FirstOrDefault();
        }
        catch (ManagementException) { return null; }
    }

    private static bool IsBitLocker(char letter)
    {
        try
        {
            var scope = new ManagementScope(@"\\.\root\CIMV2\Security\MicrosoftVolumeEncryption");
            scope.Connect();
            using (var searcher = new ManagementObjectSearcher(scope,
                       new ObjectQuery("SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter='" + letter + ":'")))
            using (var items = searcher.Get())
            {
                var volume = items.Cast<ManagementObject>().FirstOrDefault();
                return volume != null &&
                       (ToInt(volume["ProtectionStatus"]) != 0 || ToInt(volume["ConversionStatus"]) != 0);
            }
        }
        catch { return false; }
    }

    internal static bool ResolveSystemFlag(bool hasStorageMetadata, bool storageIsSystem, bool isActive,
        bool isCurrentWindows, bool hasDriveLetter, PartitionRole role)
    {
        if (hasStorageMetadata) return storageIsSystem || role == PartitionRole.EfiSystem;
        return role == PartitionRole.EfiSystem || (isActive && !isCurrentWindows && !hasDriveLetter);
    }

    internal static PartitionRole RoleFromMetadata(string? gptType, int mbrType, string type,
        string fileSystem, bool isSystem)
    {
        var gpt = NormalizeGuid(gptType);
        if (gpt.Equals("c12a7328-f81f-11d2-ba4b-00a0c93ec93b", StringComparison.OrdinalIgnoreCase)) return PartitionRole.EfiSystem;
        if (gpt.Equals("e3c9e316-0b5c-4db8-817d-f92df00215ae", StringComparison.OrdinalIgnoreCase)) return PartitionRole.MicrosoftReserved;
        if (gpt.Equals("de94bba4-06d1-4d40-a16a-bfd50179d6ac", StringComparison.OrdinalIgnoreCase)) return PartitionRole.Recovery;
        if (gpt.Equals("ebd0a0a2-b9e5-4433-87c0-68b6b72699c7", StringComparison.OrdinalIgnoreCase)) return PartitionRole.BasicData;
        if (mbrType == 0x27) return PartitionRole.Recovery;
        if (mbrType == 0x12 || mbrType == 0x84 || mbrType == 0xA0) return PartitionRole.Oem;
        if (type.IndexOf("Reserved", StringComparison.OrdinalIgnoreCase) >= 0) return PartitionRole.MicrosoftReserved;
        if (type.IndexOf("Recovery", StringComparison.OrdinalIgnoreCase) >= 0) return PartitionRole.Recovery;
        if (type.IndexOf("OEM", StringComparison.OrdinalIgnoreCase) >= 0) return PartitionRole.Oem;
        if (type.IndexOf("EFI System", StringComparison.OrdinalIgnoreCase) >= 0 ||
            (type.StartsWith("GPT", StringComparison.OrdinalIgnoreCase) &&
             type.IndexOf("System", StringComparison.OrdinalIgnoreCase) >= 0)) return PartitionRole.EfiSystem;
        if (mbrType == 0x07 || mbrType == 0x0B || mbrType == 0x0C ||
            type.IndexOf("Basic", StringComparison.OrdinalIgnoreCase) >= 0 ||
            type.IndexOf("Installable", StringComparison.OrdinalIgnoreCase) >= 0 ||
            !string.IsNullOrWhiteSpace(fileSystem)) return PartitionRole.BasicData;
        return PartitionRole.Unknown;
    }

    private static PartitionScheme InferPartitionScheme(IEnumerable<PartitionInfo> partitions) =>
        partitions.Any(p => !string.IsNullOrWhiteSpace(p.GptType) || p.Type.StartsWith("GPT", StringComparison.OrdinalIgnoreCase))
            ? PartitionScheme.Gpt : PartitionScheme.Mbr;

    private static PartitionScheme ParsePartitionScheme(int value) => value == 2 ? PartitionScheme.Gpt
        : value == 1 ? PartitionScheme.Mbr : value == 0 ? PartitionScheme.Raw : PartitionScheme.Unknown;

    private static string BusTypeName(int value)
    {
        switch (value)
        {
            case 1: return "SCSI"; case 2: return "ATAPI"; case 3: return "ATA"; case 4: return "IEEE 1394";
            case 6: return "Fibre Channel"; case 7: return "USB"; case 8: return "RAID"; case 9: return "iSCSI";
            case 10: return "SAS"; case 11: return "SATA"; case 12: return "SD"; case 13: return "MMC";
            case 14: return "Virtual"; case 15: return "File-backed Virtual"; case 16: return "Storage Spaces";
            case 17: return "NVMe"; case 18: return "SCM"; case 19: return "UFS"; default: return "Unknown";
        }
    }

    private static void AddUnallocatedRegions(List<PartitionInfo> partitions, int diskNumber, long size)
    {
        const long minimum = 1024L * 1024;
        long cursor = 0;
        foreach (var partition in partitions.OrderBy(p => p.OffsetBytes).ToArray())
        {
            var gap = partition.OffsetBytes - cursor;
            if (gap >= minimum) partitions.Add(Unallocated(diskNumber, cursor, gap));
            cursor = Math.Max(cursor, partition.OffsetBytes + partition.LengthBytes);
        }
        if (size - cursor >= minimum) partitions.Add(Unallocated(diskNumber, cursor, size - cursor));
    }

    private static PartitionInfo Unallocated(int disk, long offset, long length) =>
        new PartitionInfo(disk, 0, offset, length, null, string.Empty, string.Empty, string.Empty,
            "Unallocated", string.Empty, 0, PartitionRole.Unallocated, false, false, false, false,
            false, false, false, true, Array.Empty<string>());

    private static string NormalizeGuid(string? value) => (value ?? string.Empty).Trim().Trim('{', '}').ToLowerInvariant();
    private static string FirstText(string? primary, string fallback) => string.IsNullOrWhiteSpace(primary) ? fallback : primary!.Trim();
    private static string Text(object? value) => Convert.ToString(value) ?? string.Empty;
    private static string[] TextArray(object? value) => value is string[] values ? values.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() : Array.Empty<string>();
    private static int ToInt(object? value) => value == null ? 0 : Convert.ToInt32(value);
    private static long ToLong(object? value) => value == null ? 0 : Convert.ToInt64(value);
    private static bool ToBool(object? value) => value != null && Convert.ToBoolean(value);

    private sealed class StorageDiskMetadata
    {
        public StorageDiskMetadata(int number, string friendlyName, string serialNumber, string uniqueId, string path,
            int busType, long sizeBytes, PartitionScheme partitionScheme, bool isBoot, bool isSystem,
            bool isReadOnly, bool isOffline)
        { Number = number; FriendlyName = friendlyName; SerialNumber = serialNumber; UniqueId = uniqueId; Path = path;
            BusType = busType; SizeBytes = sizeBytes; PartitionScheme = partitionScheme; IsBoot = isBoot;
            IsSystem = isSystem; IsReadOnly = isReadOnly; IsOffline = isOffline; }
        public int Number { get; }
        public string FriendlyName { get; }
        public string SerialNumber { get; }
        public string UniqueId { get; }
        public string Path { get; }
        public int BusType { get; }
        public long SizeBytes { get; }
        public PartitionScheme PartitionScheme { get; }
        public bool IsBoot { get; }
        public bool IsSystem { get; }
        public bool IsReadOnly { get; }
        public bool IsOffline { get; }
    }

    private sealed class StoragePartitionMetadata
    {
        public StoragePartitionMetadata(int partitionNumber, long offsetBytes, long lengthBytes,
            string partitionGuid, string gptType, int mbrType, bool isActive, bool isBoot, bool isSystem,
            bool isReadOnly, bool isOffline, string[] accessPaths)
        { PartitionNumber = partitionNumber; OffsetBytes = offsetBytes; LengthBytes = lengthBytes;
            PartitionGuid = partitionGuid; GptType = gptType; MbrType = mbrType; IsActive = isActive;
            IsBoot = isBoot; IsSystem = isSystem; IsReadOnly = isReadOnly; IsOffline = isOffline;
            AccessPaths = accessPaths; }
        public int PartitionNumber { get; }
        public long OffsetBytes { get; }
        public long LengthBytes { get; }
        public string PartitionGuid { get; }
        public string GptType { get; }
        public int MbrType { get; }
        public bool IsActive { get; }
        public bool IsBoot { get; }
        public bool IsSystem { get; }
        public bool IsReadOnly { get; }
        public bool IsOffline { get; }
        public string[] AccessPaths { get; }
    }
}

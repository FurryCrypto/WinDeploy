using System.Text.Json;
using ESDInstaller.Core.Models;

namespace ESDInstaller.Core.Services;

public sealed class DiskService
{
    private readonly ProcessRunner _processRunner;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public DiskService(ProcessRunner processRunner) => _processRunner = processRunner;

    public async Task<IReadOnlyList<DiskInfo>> GetDisksAsync(CancellationToken cancellationToken = default)
    {
        const string script = """
            $ErrorActionPreference = 'Stop'
            $systemLetter = $env:SystemDrive.TrimEnd(':')
            $wmiDisks = @(Get-CimInstance Win32_DiskDrive | Sort-Object Index)
            $wmiPartitions = @(Get-CimInstance Win32_DiskPartition)
            $logicalByPartition = @{}
            Get-CimInstance Win32_LogicalDiskToPartition -ErrorAction SilentlyContinue | ForEach-Object {
              $partitionId = ([string]$_.Antecedent.DeviceID)
              $driveId = ([string]$_.Dependent.DeviceID)
              if ($partitionId -and $driveId) { $logicalByPartition[$partitionId] = $driveId.TrimEnd(':') }
            }
            $logicalDisks = @{}
            Get-CimInstance Win32_LogicalDisk -ErrorAction SilentlyContinue | ForEach-Object { $logicalDisks[$_.DeviceID.TrimEnd(':')] = $_ }
            $storageAvailable = $true
            try { $storageDisks = @(Get-Disk -ErrorAction Stop) } catch { $storageAvailable = $false; $storageDisks = @() }
            $bitlocker = @{}
            if ($storageAvailable) {
              try { Get-BitLockerVolume -ErrorAction Stop | ForEach-Object { if ($_.MountPoint) { $bitlocker[$_.MountPoint.TrimEnd('\').TrimEnd(':')] = ($_.VolumeStatus -ne 'FullyDecrypted') } } } catch {}
            }
            $items = @($wmiDisks | ForEach-Object {
              $wmiDisk = $_
              $storageDisk = if ($storageAvailable) { $storageDisks | Where-Object Number -eq $wmiDisk.Index | Select-Object -First 1 } else { $null }
              if ($storageDisk) {
                $partitions = @(Get-Partition -DiskNumber $storageDisk.Number -ErrorAction Stop | Where-Object {
                  $candidateType = [string]$_.Type
                  $candidateMbrType = if ($null -ne $_.MbrType) { [int]$_.MbrType } else { 0 }
                  $candidateType -notmatch 'Extended' -and $candidateMbrType -notin 0x05, 0x0F, 0x85
                } | Sort-Object Offset | ForEach-Object {
                  $partition = $_; $volume = $null
                  try { $volume = $partition | Get-Volume -ErrorAction Stop } catch {}
                  $mount = if ($partition.DriveLetter) { [string]$partition.DriveLetter } else { '' }
                  [pscustomobject]@{
                    DiskNumber=[int]$storageDisk.Number; PartitionNumber=[int]$partition.PartitionNumber
                    OffsetBytes=[long]$partition.Offset; LengthBytes=[long]$partition.Size
                    DriveLetter=if($partition.DriveLetter){[string]$partition.DriveLetter}else{$null}
                    VolumeLabel=if($volume){[string]$volume.FileSystemLabel}else{''}; FileSystem=if($volume){[string]$volume.FileSystem}else{''}
                    PartitionGuid=if($partition.Guid){[string]$partition.Guid}else{''}; Type=[string]$partition.Type
                    GptType=if($partition.GptType){[string]$partition.GptType}else{''}; MbrType=if($null-ne $partition.MbrType){[int]$partition.MbrType}else{0}
                    IsActive=[bool]$partition.IsActive; IsBoot=[bool]$partition.IsBoot; IsSystem=[bool]$partition.IsSystem
                    IsCurrentWindows=[bool]($mount -and $mount -eq $systemLetter); IsBitLocker=[bool]($mount -and $bitlocker.ContainsKey($mount) -and $bitlocker[$mount])
                    IsReadOnly=[bool]$partition.IsReadOnly; IsOffline=[bool]$partition.IsOffline; AccessPaths=@($partition.AccessPaths|ForEach-Object{[string]$_})
                  }
                })
              } else {
                $rawParts = @($wmiPartitions | Where-Object {
                  $_.DiskIndex -eq $wmiDisk.Index -and ([string]$_.Type) -notmatch 'Extended'
                } | Sort-Object StartingOffset)
                $partitions = @($rawParts | ForEach-Object {
                  $partition = $_; $letter = if($logicalByPartition.ContainsKey($partition.DeviceID)){[string]$logicalByPartition[$partition.DeviceID]}else{''}
                  $logical = if($letter -and $logicalDisks.ContainsKey($letter)){$logicalDisks[$letter]}else{$null}
                  $type = [string]$partition.Type; $gptType = ''
                  if($type -match 'System'){$gptType='c12a7328-f81f-11d2-ba4b-00a0c93ec93b'}
                  elseif($type -match 'Reserved'){$gptType='e3c9e316-0b5c-4db8-817d-f92df00215ae'}
                  elseif($type -match 'Recovery'){$gptType='de94bba4-06d1-4d40-a16a-bfd50179d6ac'}
                  elseif($type -match 'Basic'){$gptType='ebd0a0a2-b9e5-4433-87c0-68b6b72699c7'}
                  [pscustomobject]@{
                    DiskNumber=[int]$wmiDisk.Index; PartitionNumber=([int]$partition.Index+1)
                    OffsetBytes=[long]$partition.StartingOffset; LengthBytes=[long]$partition.Size
                    DriveLetter=if($letter){$letter}else{$null}; VolumeLabel=if($logical){[string]$logical.VolumeName}else{''}; FileSystem=if($logical){[string]$logical.FileSystem}else{''}
                    PartitionGuid=''; Type=$type; GptType=$gptType; MbrType=0
                    IsActive=[bool]$partition.Bootable; IsBoot=[bool]($letter -eq $systemLetter); IsSystem=[bool]($type -match 'System')
                    IsCurrentWindows=[bool]($letter -eq $systemLetter); IsBitLocker=$false; IsReadOnly=$false; IsOffline=$false
                    AccessPaths=if($letter){@("$letter`:\")}else{@()}
                  }
                })
              }
              $scheme = if($storageDisk){[string]$storageDisk.PartitionStyle}elseif(($partitions|Where-Object{$_.Type -match '^GPT'}).Count -gt 0){'GPT'}else{'MBR'}
              [pscustomobject]@{
                Number=[int]$wmiDisk.Index; FriendlyName=[string]$wmiDisk.Model; Model=[string]$wmiDisk.Model
                SerialNumber=if($wmiDisk.SerialNumber){([string]$wmiDisk.SerialNumber).Trim()}else{''}
                UniqueId=if($wmiDisk.PNPDeviceID){([string]$wmiDisk.PNPDeviceID).Trim()}else{''}; Path=[string]$wmiDisk.DeviceID
                BusType=if($storageDisk){[string]$storageDisk.BusType}else{[string]$wmiDisk.InterfaceType}
                SizeBytes=if($storageDisk){[long]$storageDisk.Size}else{[long]$wmiDisk.Size}
                PartitionScheme=$scheme; IsBoot=[bool]($partitions|Where-Object IsCurrentWindows); IsSystem=[bool]($partitions|Where-Object IsSystem)
                IsReadOnly=if($storageDisk){[bool]$storageDisk.IsReadOnly}else{$false}; IsOffline=if($storageDisk){[bool]$storageDisk.IsOffline}else{$false}
                Partitions=$partitions
              }
            })
            ConvertTo-Json -InputObject $items -Depth 6 -Compress
            """;

        var result = await _processRunner.RunPowerShellAsync(script, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (!result.Succeeded)
            throw new ESDInstallerException("ErrorDiskEnumeration", result.StandardError.Trim());

        try
        {
            var json = ExtractJson(result.StandardOutput);
            var dtos = JsonSerializer.Deserialize<List<DiskDto>>(json, JsonOptions) ?? [];
            return dtos.Select(MapDisk).ToArray();
        }
        catch (Exception exception) when (exception is not ESDInstallerException)
        {
            throw new ESDInstallerException("ErrorDiskEnumeration", result.StandardOutput, exception);
        }
    }

    private static DiskInfo MapDisk(DiskDto disk)
    {
        var partitions = disk.Partitions.Select(partition => new PartitionInfo(
            disk.Number, partition.PartitionNumber, partition.OffsetBytes, partition.LengthBytes,
            string.IsNullOrWhiteSpace(partition.DriveLetter) ? null : char.ToUpperInvariant(partition.DriveLetter[0]),
            partition.VolumeLabel ?? string.Empty, partition.FileSystem ?? string.Empty,
            NormalizeGuid(partition.PartitionGuid), partition.Type ?? string.Empty, NormalizeGuid(partition.GptType),
            partition.MbrType, GetRole(partition), partition.IsActive, partition.IsBoot, partition.IsSystem,
            partition.IsCurrentWindows, partition.IsBitLocker, partition.IsReadOnly, partition.IsOffline, false,
            partition.AccessPaths ?? Array.Empty<string>())).OrderBy(partition => partition.OffsetBytes).ToList();

        AddUnallocatedRegions(partitions, disk.Number, disk.SizeBytes);
        return new DiskInfo(disk.Number, disk.FriendlyName ?? string.Empty, disk.Model ?? string.Empty,
            disk.SerialNumber ?? string.Empty, disk.UniqueId ?? string.Empty, disk.Path ?? string.Empty,
            disk.BusType ?? string.Empty, disk.SizeBytes, ParseScheme(disk.PartitionScheme), disk.IsBoot,
            disk.IsSystem, disk.IsReadOnly, disk.IsOffline, partitions.OrderBy(partition => partition.OffsetBytes).ToArray());
    }

    private static void AddUnallocatedRegions(List<PartitionInfo> partitions, int diskNumber, long diskSize)
    {
        const long minimumVisibleGap = 1024L * 1024;
        long cursor = 0;
        foreach (var partition in partitions.OrderBy(item => item.OffsetBytes).ToArray())
        {
            var gap = partition.OffsetBytes - cursor;
            if (gap >= minimumVisibleGap) partitions.Add(Unallocated(diskNumber, cursor, gap));
            cursor = Math.Max(cursor, partition.OffsetBytes + partition.LengthBytes);
        }
        if (diskSize - cursor >= minimumVisibleGap) partitions.Add(Unallocated(diskNumber, cursor, diskSize - cursor));
    }

    private static PartitionInfo Unallocated(int disk, long offset, long length) => new(
        disk, 0, offset, length, null, string.Empty, string.Empty, string.Empty, "Unallocated", string.Empty, 0,
        PartitionRole.Unallocated, false, false, false, false, false, false, false, true, Array.Empty<string>());

    private static PartitionRole GetRole(PartitionDto partition)
    {
        var gpt = NormalizeGuid(partition.GptType);
        if (gpt.Equals("c12a7328-f81f-11d2-ba4b-00a0c93ec93b", StringComparison.OrdinalIgnoreCase)) return PartitionRole.EfiSystem;
        if (gpt.Equals("e3c9e316-0b5c-4db8-817d-f92df00215ae", StringComparison.OrdinalIgnoreCase)) return PartitionRole.MicrosoftReserved;
        if (gpt.Equals("de94bba4-06d1-4d40-a16a-bfd50179d6ac", StringComparison.OrdinalIgnoreCase)) return PartitionRole.Recovery;
        if (gpt.Equals("ebd0a0a2-b9e5-4433-87c0-68b6b72699c7", StringComparison.OrdinalIgnoreCase)) return PartitionRole.BasicData;
        if (partition.MbrType == 0x27) return PartitionRole.Recovery;
        if (partition.MbrType is 0x12 or 0x84 or 0xA0) return PartitionRole.Oem;
        if (partition.Type?.Contains("Reserved", StringComparison.OrdinalIgnoreCase) == true) return PartitionRole.MicrosoftReserved;
        if (partition.Type?.Contains("System", StringComparison.OrdinalIgnoreCase) == true) return PartitionRole.EfiSystem;
        if (partition.Type?.Contains("Recovery", StringComparison.OrdinalIgnoreCase) == true) return PartitionRole.Recovery;
        if (partition.MbrType is 0x07 or 0x0B or 0x0C || !string.IsNullOrWhiteSpace(partition.FileSystem)) return PartitionRole.BasicData;
        return PartitionRole.Unknown;
    }

    private static PartitionScheme ParseScheme(string? value) => value?.Trim().ToUpperInvariant() switch
    {
        "GPT" => PartitionScheme.Gpt,
        "MBR" => PartitionScheme.Mbr,
        "RAW" => PartitionScheme.Raw,
        _ => PartitionScheme.Unknown
    };

    private static string NormalizeGuid(string? value) => (value ?? string.Empty).Trim().Trim('{', '}').ToLowerInvariant();

    private static string ExtractJson(string value)
    {
        var arrayStart = value.IndexOf('[');
        var objectStart = value.IndexOf('{');
        var start = arrayStart >= 0 && (objectStart < 0 || arrayStart < objectStart) ? arrayStart : objectStart;
        var end = Math.Max(value.LastIndexOf(']'), value.LastIndexOf('}'));
        if (start < 0 || end < start) throw new JsonException("No JSON payload was returned.");
        var json = value[start..(end + 1)];
        return json.StartsWith('{') ? $"[{json}]" : json;
    }

    private sealed class DiskDto
    {
        public int Number { get; set; }
        public string? FriendlyName { get; set; }
        public string? Model { get; set; }
        public string? SerialNumber { get; set; }
        public string? UniqueId { get; set; }
        public string? Path { get; set; }
        public string? BusType { get; set; }
        public long SizeBytes { get; set; }
        public string? PartitionScheme { get; set; }
        public bool IsBoot { get; set; }
        public bool IsSystem { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsOffline { get; set; }
        public List<PartitionDto> Partitions { get; set; } = [];
    }

    private sealed class PartitionDto
    {
        public int PartitionNumber { get; set; }
        public long OffsetBytes { get; set; }
        public long LengthBytes { get; set; }
        public string? DriveLetter { get; set; }
        public string? VolumeLabel { get; set; }
        public string? FileSystem { get; set; }
        public string? PartitionGuid { get; set; }
        public string? Type { get; set; }
        public string? GptType { get; set; }
        public int MbrType { get; set; }
        public bool IsActive { get; set; }
        public bool IsBoot { get; set; }
        public bool IsSystem { get; set; }
        public bool IsCurrentWindows { get; set; }
        public bool IsBitLocker { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsOffline { get; set; }
        public string[]? AccessPaths { get; set; }
    }
}

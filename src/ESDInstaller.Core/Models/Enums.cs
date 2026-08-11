namespace ESDInstaller.Core.Models;

public enum WindowsImageKind { Iso, Wim, Esd, LegacyIso }
public enum WindowsGeneration { Unknown, WindowsXp, WindowsVista, Windows7, Windows8, Windows81, Windows10, Windows11 }
public enum CpuArchitecture { Unknown, X86, X64, Arm, Arm64 }
public enum FirmwareMode { Unknown, Bios, Uefi }
public enum PartitionScheme { Unknown, Mbr, Gpt, Raw }
public enum PartitionRole { BasicData, EfiSystem, MicrosoftReserved, Recovery, Oem, Unallocated, Unknown }
public enum InstallationEngineKind { ModernWindows, LegacyNt6, LegacyXpNt5 }
public enum PlanSeverity { Information, Warning, Error }
public enum InstallationStage { Connecting, Validating, PreparingDestination, Formatting, ApplyingImage, InstallingBootFiles, Verifying, Completed, Failed }

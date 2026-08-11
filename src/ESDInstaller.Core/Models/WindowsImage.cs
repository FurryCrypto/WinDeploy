namespace ESDInstaller.Core.Models;

public sealed record WindowsImageEdition(
    int Index,
    string Name,
    string Description,
    CpuArchitecture Architecture,
    int Build,
    Version? Version,
    long ApproximateSizeBytes);

public sealed record WindowsImage(
    string SourcePath,
    string ResolvedImagePath,
    WindowsImageKind Kind,
    WindowsGeneration Generation,
    string DisplayVersion,
    CpuArchitecture Architecture,
    long FileSizeBytes,
    DateTime SourceLastWriteUtc,
    string? MountedIsoPath,
    IReadOnlyList<WindowsImageEdition> Editions,
    string? LegacyReason = null)
{
    public bool RequiresLegacyEngine => Kind == WindowsImageKind.LegacyIso ||
        Generation is WindowsGeneration.WindowsXp or WindowsGeneration.WindowsVista;
}

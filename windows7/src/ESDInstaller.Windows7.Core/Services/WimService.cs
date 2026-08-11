using System.Xml.Linq;
using ManagedWimLib;
using ESDInstaller.Windows7.Core.Models;

namespace ESDInstaller.Windows7.Core.Services;

public sealed class WimService
{
    public Task<IReadOnlyList<WindowsImageEdition>> ReadEditionsAsync(string path,
        CancellationToken cancellationToken = default) => Task.Run(() => ReadEditions(path, cancellationToken), cancellationToken);

    private static IReadOnlyList<WindowsImageEdition> ReadEditions(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path)) throw new ESDInstallerException("ErrorImageNotFound", path);
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            WimLibNative.EnsureInitialized();
            using (var wim = Wim.OpenWim(path, OpenFlags.CheckIntegrity))
                return ParseEditions(wim.GetXmlData());
        }
        catch (ESDInstallerException) { throw; }
        catch (Exception exception)
        { throw new ESDInstallerException("ErrorImageMetadata", exception.Message, exception); }
    }

    internal static IReadOnlyList<WindowsImageEdition> ParseEditions(string xml)
    {
        XDocument document;
        try { document = XDocument.Parse(xml.Trim('\0', '\uFEFF'), LoadOptions.None); }
        catch (System.Xml.XmlException exception)
        { throw new ESDInstallerException("ErrorImageMetadata", "The WIM metadata XML is invalid.", exception); }
        var images = document.Descendants().Where(x => x.Name.LocalName == "IMAGE").ToArray();
        if (document.Root != null && document.Root.Name.LocalName == "IMAGE") images = new[] { document.Root };
        if (images.Length == 0) throw new ESDInstallerException("ErrorNoEditions", "The image contains no indexes.");
        return images.Select((image, position) => ParseEdition(image, position + 1)).ToArray();
    }

    private static WindowsImageEdition ParseEdition(XElement image, int fallbackIndex)
    {
        Func<string, string?> value = name => image.Descendants().FirstOrDefault(x => x.Name.LocalName == name)?.Value;
        var windows = image.Descendants().FirstOrDefault(x => x.Name.LocalName == "WINDOWS");
        Func<string, string?> windowsValue = name => windows?.Descendants().FirstOrDefault(x => x.Name.LocalName == name)?.Value;
        var index = int.TryParse(image.Attribute("INDEX")?.Value, out var parsedIndex) ? parsedIndex : fallbackIndex;
        var name = value("DISPLAYNAME") ?? value("NAME") ?? "Image " + index;
        var description = value("DISPLAYDESCRIPTION") ?? value("DESCRIPTION") ?? name;
        var version = windows?.Descendants().FirstOrDefault(x => x.Name.LocalName == "VERSION");
        var major = ParseInt(version?.Elements().FirstOrDefault(x => x.Name.LocalName == "MAJOR")?.Value);
        var minor = ParseInt(version?.Elements().FirstOrDefault(x => x.Name.LocalName == "MINOR")?.Value);
        var build = ParseInt(version?.Elements().FirstOrDefault(x => x.Name.LocalName == "BUILD")?.Value);
        var revision = ParseInt(version?.Elements().FirstOrDefault(x => x.Name.LocalName == "SPBUILD")?.Value);
        Version? parsedVersion = major > 0 ? new Version(major, Math.Max(0, minor), Math.Max(0, build), Math.Max(0, revision)) : null;
        var size = long.TryParse(value("TOTALBYTES"), out var parsedSize) ? parsedSize : 0;
        return new WindowsImageEdition(index, name, description, ParseArchitecture(windowsValue("ARCH")), build,
            parsedVersion, size);
    }

    private static int ParseInt(string? value) => int.TryParse(value, out var result) ? result : 0;
    private static CpuArchitecture ParseArchitecture(string? value)
    {
        switch ((value ?? "").Trim().ToLowerInvariant())
        {
            case "0": case "x86": return CpuArchitecture.X86;
            case "5": case "arm": return CpuArchitecture.Arm;
            case "9": case "amd64": case "x64": return CpuArchitecture.X64;
            case "12": case "arm64": return CpuArchitecture.Arm64;
            default: return CpuArchitecture.Unknown;
        }
    }
}

using System.Runtime.InteropServices;
using System.Xml.Linq;
using WinDeploy.Core.Models;

namespace WinDeploy.Core.Services;

public sealed class WimService
{
    private const uint WimGenericRead = 0x80000000;
    private const uint WimOpenExisting = 3;

    public Task<IReadOnlyList<WindowsImageEdition>> ReadEditionsAsync(string path, CancellationToken cancellationToken = default)
        => Task.Run(() => ReadEditions(path, cancellationToken), cancellationToken);

    private static IReadOnlyList<WindowsImageEdition> ReadEditions(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
        {
            throw new WinDeployException("ErrorImageNotFound", path);
        }

        var handle = WIMCreateFile(path, WimGenericRead, WimOpenExisting, 0, 0, out _);
        if (handle == IntPtr.Zero || handle == new IntPtr(-1))
        {
            throw new WinDeployException("ErrorImageOpen", $"WIMCreateFile failed with Win32 error {Marshal.GetLastWin32Error()}.");
        }

        try
        {
            var scratchPath = EnsureScratchDirectory();
            if (!WIMSetTemporaryPath(handle, scratchPath))
            {
                var error = Marshal.GetLastWin32Error();
                throw new WinDeployException("ErrorImageScratch",
                    $"WIMSetTemporaryPath failed with Win32 error {error}. Scratch path: {scratchPath}");
            }

            var count = WIMGetImageCount(handle);
            if (count == 0)
            {
                throw new WinDeployException("ErrorNoEditions", "The image contains no indexes.");
            }

            var result = new List<WindowsImageEdition>((int)count);
            for (var index = 1; index <= count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var imageHandle = WIMLoadImage(handle, index);
                if (imageHandle == IntPtr.Zero)
                {
                    throw new WinDeployException("ErrorImageMetadata", $"WIMLoadImage({index}) failed with Win32 error {Marshal.GetLastWin32Error()}.");
                }

                try
                {
                    if (!WIMGetImageInformation(imageHandle, out var buffer, out var byteCount))
                    {
                        throw new WinDeployException("ErrorImageMetadata", $"WIMGetImageInformation({index}) failed with Win32 error {Marshal.GetLastWin32Error()}.");
                    }

                    try
                    {
                        var xml = Marshal.PtrToStringUni(buffer, checked((int)byteCount / 2)) ?? string.Empty;
                        result.Add(ParseEdition(xml, (int)index));
                    }
                    finally
                    {
                        LocalFree(buffer);
                    }
                }
                finally
                {
                    WIMCloseHandle(imageHandle);
                }
            }

            return result;
        }
        finally
        {
            WIMCloseHandle(handle);
        }
    }

    internal static WindowsImageEdition ParseEdition(string xml, int fallbackIndex)
    {
        // WIMGetImageInformation commonly includes a UTF-16 BOM in the returned string.
        // XDocument parses a TextReader-produced BOM, but not a literal U+FEFF at position zero.
        xml = xml.Trim('\0', '\uFEFF');
        XDocument document;
        try
        {
            document = XDocument.Parse(xml, LoadOptions.None);
        }
        catch (System.Xml.XmlException exception)
        {
            throw new WinDeployException("ErrorImageMetadata", "The WIM metadata XML is invalid.", exception);
        }
        var image = document.Root?.Name.LocalName == "IMAGE"
            ? document.Root
            : document.Descendants().FirstOrDefault(element => element.Name.LocalName == "IMAGE");
        if (image is null)
        {
            throw new WinDeployException("ErrorImageMetadata", "The WIM metadata did not contain an IMAGE element.");
        }

        string? Value(string name) => image.Descendants().FirstOrDefault(element => element.Name.LocalName == name)?.Value;
        string? WindowsValue(string name) => image.Descendants().FirstOrDefault(element => element.Name.LocalName == "WINDOWS")?
            .Descendants().FirstOrDefault(element => element.Name.LocalName == name)?.Value;

        var index = int.TryParse(image.Attribute("INDEX")?.Value, out var parsedIndex) ? parsedIndex : fallbackIndex;
        var name = Value("DISPLAYNAME") ?? Value("NAME") ?? $"Image {index}";
        var description = Value("DISPLAYDESCRIPTION") ?? Value("DESCRIPTION") ?? name;
        var architecture = ParseArchitecture(WindowsValue("ARCH"));
        var versionElement = image.Descendants().FirstOrDefault(element => element.Name.LocalName == "VERSION");
        var major = ParseInt(versionElement?.Elements().FirstOrDefault(element => element.Name.LocalName == "MAJOR")?.Value);
        var minor = ParseInt(versionElement?.Elements().FirstOrDefault(element => element.Name.LocalName == "MINOR")?.Value);
        var build = ParseInt(versionElement?.Elements().FirstOrDefault(element => element.Name.LocalName == "BUILD")?.Value);
        var revision = ParseInt(versionElement?.Elements().FirstOrDefault(element => element.Name.LocalName == "SPBUILD")?.Value);
        Version? version = major > 0 ? new Version(major, Math.Max(0, minor), Math.Max(0, build), Math.Max(0, revision)) : null;
        var bytes = long.TryParse(Value("TOTALBYTES"), out var parsedBytes) ? parsedBytes : 0;
        return new WindowsImageEdition(index, name, description, architecture, build, version, bytes);
    }

    private static int ParseInt(string? value) => int.TryParse(value, out var parsed) ? parsed : 0;

    internal static string EnsureScratchDirectory()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        if (string.IsNullOrWhiteSpace(localAppData))
        {
            throw new WinDeployException("ErrorImageScratch", "The local application data directory is unavailable.");
        }

        var scratchPath = Path.Combine(localAppData, "WinDeploy", "WimScratch");
        try
        {
            Directory.CreateDirectory(scratchPath);

            // Fail early with a useful error instead of surfacing WIMGAPI's opaque error 1632 later.
            var probePath = Path.Combine(scratchPath, $".write-test-{Environment.ProcessId}-{Guid.NewGuid():N}.tmp");
            using var probe = new FileStream(probePath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                bufferSize: 1, FileOptions.DeleteOnClose);
            return scratchPath;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            throw new WinDeployException("ErrorImageScratch", $"Scratch path is not writable: {scratchPath}", exception);
        }
    }

    private static CpuArchitecture ParseArchitecture(string? value) => value?.Trim().ToLowerInvariant() switch
    {
        "0" or "x86" => CpuArchitecture.X86,
        "5" or "arm" => CpuArchitecture.Arm,
        "9" or "amd64" or "x64" => CpuArchitecture.X64,
        "12" or "arm64" => CpuArchitecture.Arm64,
        _ => CpuArchitecture.Unknown
    };

    [DllImport("wimgapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr WIMCreateFile(string path, uint desiredAccess, uint creationDisposition,
        uint flagsAndAttributes, uint compressionType, out uint creationResult);

    [DllImport("wimgapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WIMSetTemporaryPath(IntPtr wimHandle, string path);

    [DllImport("wimgapi.dll", SetLastError = true)]
    private static extern uint WIMGetImageCount(IntPtr wimHandle);

    [DllImport("wimgapi.dll", SetLastError = true)]
    private static extern IntPtr WIMLoadImage(IntPtr wimHandle, int imageIndex);

    [DllImport("wimgapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WIMGetImageInformation(IntPtr imageHandle, out IntPtr imageInfo, out uint sizeOfImageInfo);

    [DllImport("wimgapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WIMCloseHandle(IntPtr handle);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LocalFree(IntPtr memory);
}

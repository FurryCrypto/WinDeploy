namespace WinDeploy.Services;

internal static class StartupDiagnostics
{
    private static readonly string Path = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WinDeploy", "startup.log");

    public static void Reset()
    {
        try { Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path)!); File.WriteAllText(Path, string.Empty); } catch { }
    }

    public static void Write(string message)
    {
        try { File.AppendAllText(Path, $"{DateTimeOffset.Now:O} {message}{Environment.NewLine}"); } catch { }
    }
}

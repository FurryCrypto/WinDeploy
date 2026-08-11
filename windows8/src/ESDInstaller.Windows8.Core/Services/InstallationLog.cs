using System.Text;

namespace ESDInstaller.Windows8.Core.Services;

public sealed class InstallationLog : IDisposable
{
    private readonly object _sync = new object();
    private readonly StreamWriter _writer;
    public InstallationLog(string? directory = null)
    {
        directory = directory ?? System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ESD Installer Logs");
        Directory.CreateDirectory(directory);
        Path = System.IO.Path.Combine(directory, "Install-" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".log");
        _writer = new StreamWriter(Path, false, new UTF8Encoding(false)) { AutoFlush = true };
    }
    public string Path { get; }
    public void Write(string level, string message)
    {
        lock (_sync) _writer.WriteLine("{0:O} [{1}] {2}", DateTime.UtcNow, level, message);
    }
    public void Dispose() { lock (_sync) _writer.Dispose(); }
}

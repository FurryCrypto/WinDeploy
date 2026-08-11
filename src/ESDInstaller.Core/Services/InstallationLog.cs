using System.Text;

namespace ESDInstaller.Core.Services;

public sealed class InstallationLog : IDisposable
{
    private readonly StreamWriter _writer;
    private readonly object _gate = new();

    public InstallationLog(string directory)
    {
        Directory.CreateDirectory(directory);
        Path = System.IO.Path.Combine(directory, $"Install-{DateTime.Now:yyyy-MM-dd-HHmmss}.log");
        _writer = new StreamWriter(new FileStream(Path, FileMode.CreateNew, FileAccess.Write, FileShare.Read),
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)) { AutoFlush = true };
    }

    public string Path { get; }

    public void Write(string category, string message)
    {
        lock (_gate)
        {
            _writer.WriteLine($"{DateTimeOffset.Now:O} [{category}] {message}");
        }
    }

    public void Dispose() => _writer.Dispose();
}

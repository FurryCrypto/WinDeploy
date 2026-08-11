using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using ESDInstaller.Core.Models;
using ESDInstaller.Core.Services;

namespace ESDInstaller.Services;

public sealed record WorkerResult(int ExitCode, string? LogPath, bool ElevationCancelled);

public sealed class WorkerClient
{
    public async Task<WorkerResult> ExecuteAsync(InstallationPlan plan, IProgress<ProgressMessage> progress,
        CancellationToken cancellationToken = default)
    {
        var localRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ESDInstaller");
        var tempDirectory = Path.Combine(localRoot, "Temp");
        var logDirectory = Path.Combine(localRoot, "Logs");
        Directory.CreateDirectory(tempDirectory);
        Directory.CreateDirectory(logDirectory);
        var planPath = Path.Combine(tempDirectory, $"plan-{plan.PlanId:N}.json");
        await File.WriteAllTextAsync(planPath,
            JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }),
            new UTF8Encoding(false), cancellationToken).ConfigureAwait(false);

        var pipeName = $"ESDInstaller-{plan.PlanId:N}";
        await using var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 1,
            PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        var workerDirectory = Path.Combine(AppContext.BaseDirectory, "Worker");
        var workerPath = Path.Combine(workerDirectory, "ESDInstaller.Worker.exe");
        if (!File.Exists(workerPath))
        {
            workerDirectory = AppContext.BaseDirectory;
            workerPath = Path.Combine(workerDirectory, "ESDInstaller.Worker.exe");
        }
        if (!File.Exists(workerPath))
            throw new ESDInstallerException("ErrorWorkerMissing", workerPath);

        Process? worker = null;
        string? logPath = null;
        try
        {
            var info = new ProcessStartInfo
            {
                FileName = workerPath,
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = workerDirectory
            };
            info.ArgumentList.Add("--plan");
            info.ArgumentList.Add(planPath);
            info.ArgumentList.Add("--pipe");
            info.ArgumentList.Add(pipeName);
            info.ArgumentList.Add("--log-dir");
            info.ArgumentList.Add(logDirectory);

            try { worker = Process.Start(info); }
            catch (Win32Exception exception) when (exception.NativeErrorCode == 1223)
            {
                return new WorkerResult(1223, null, true);
            }
            if (worker is null) throw new ESDInstallerException("ErrorWorkerStart", workerPath);

            using var connectionTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            connectionTimeout.CancelAfter(TimeSpan.FromSeconds(45));
            await pipe.WaitForConnectionAsync(connectionTimeout.Token).ConfigureAwait(false);
            using var reader = new StreamReader(pipe, Encoding.UTF8, detectEncodingFromByteOrderMarks: false,
                bufferSize: 4096, leaveOpen: true);
            while (true)
            {
                var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line is null) break;
                try
                {
                    var message = JsonSerializer.Deserialize<ProgressMessage>(line,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (message is null) continue;
                    logPath = message.LogPath ?? logPath;
                    progress.Report(message);
                }
                catch (JsonException) { }
            }
            await worker.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            return new WorkerResult(worker.ExitCode, logPath, false);
        }
        finally
        {
            worker?.Dispose();
            try { File.Delete(planPath); } catch { }
        }
    }
}

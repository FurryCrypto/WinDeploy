using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WinDeploy.Windows7.Core.Models;
using WinDeploy.Windows7.Core.Services;

namespace WinDeploy.Windows7.Services;

public sealed record WorkerResult(int ExitCode, string? LogPath, bool ElevationCancelled);

public sealed class WorkerClient
{
    public async Task<WorkerResult> ExecuteAsync(InstallationPlan plan, IProgress<ProgressMessage> progress,
        CancellationToken cancellationToken = default)
    {
        var localRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WinDeployWindows7");
        var temp = Path.Combine(localRoot, "Temp");
        var logs = Path.Combine(localRoot, "Logs");
        Directory.CreateDirectory(temp); Directory.CreateDirectory(logs);
        var planPath = Path.Combine(temp, "plan-" + plan.PlanId.ToString("N") + ".json");
        File.WriteAllText(planPath, JsonSerializer.Serialize(plan, new JsonSerializerOptions { WriteIndented = true }), new UTF8Encoding(false));
        var pipeName = "WinDeployWindows7-" + plan.PlanId.ToString("N");
        using (var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte,
                   PipeOptions.Asynchronous))
        {
            var workerDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Worker");
            var workerPath = Path.Combine(workerDirectory, "WinDeploy.Windows7.Worker.exe");
            if (!File.Exists(workerPath)) throw new WinDeployException("ErrorWorkerMissing", workerPath);
            Process? worker = null;
            string? logPath = null;
            try
            {
                var info = new ProcessStartInfo
                {
                    FileName = workerPath, UseShellExecute = true, Verb = "runas", WorkingDirectory = workerDirectory,
                    Arguments = "--plan " + ProcessRunner.QuoteArgument(planPath) + " --pipe " +
                                ProcessRunner.QuoteArgument(pipeName) + " --log-dir " + ProcessRunner.QuoteArgument(logs)
                };
                try { worker = Process.Start(info); }
                catch (Win32Exception exception) when (exception.NativeErrorCode == 1223)
                { return new WorkerResult(1223, null, true); }
                if (worker == null) throw new WinDeployException("ErrorWorkerStart", workerPath);
                var connection = pipe.WaitForConnectionAsync();
                var timeout = Task.Delay(TimeSpan.FromSeconds(45), cancellationToken);
                if (await Task.WhenAny(connection, timeout).ConfigureAwait(false) != connection)
                    throw new WinDeployException("ErrorWorkerStart", "The elevated worker did not connect.");
                await connection.ConfigureAwait(false);
                using (var reader = new StreamReader(pipe, Encoding.UTF8, false, 4096, true))
                {
                    while (true)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var line = await reader.ReadLineAsync().ConfigureAwait(false);
                        if (line == null) break;
                        try
                        {
                            var message = JsonSerializer.Deserialize<ProgressMessage>(line,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (message == null) continue;
                            logPath = message.LogPath ?? logPath;
                            progress.Report(message);
                        }
                        catch (JsonException) { }
                    }
                }
                await Task.Run(() => worker.WaitForExit(), cancellationToken).ConfigureAwait(false);
                return new WorkerResult(worker.ExitCode, logPath, false);
            }
            finally
            {
                worker?.Dispose();
                try { File.Delete(planPath); } catch { }
            }
        }
    }
}

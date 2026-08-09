using WinDeploy.Core.Models;
using WinDeploy.Core.Services;

namespace WinDeploy.Core.Installation;

public interface IInstallationEngine
{
    InstallationEngineKind Kind { get; }
    Task ExecuteAsync(InstallationPlan plan, InstallationExecutionContext context, CancellationToken cancellationToken = default);
}

public sealed class InstallationExecutionContext
{
    public required ProcessRunner Processes { get; init; }
    public required ExecutionPlanValidator Validator { get; init; }
    public required InstallationLog Log { get; init; }
    public required Action<ProgressMessage> Report { get; init; }

    public void Progress(InstallationStage stage, int overall, int? operation, string key, string detail = "", bool error = false)
    {
        Report(new ProgressMessage(stage, Math.Clamp(overall, 0, 100), operation, key, detail,
            DateTime.UtcNow, error, Log.Path));
        Log.Write(error ? "ERROR" : "PROGRESS", $"{stage} {overall}% {key} {detail}".Trim());
    }
}

using WinDeploy.Windows8.Core.Models;
using WinDeploy.Windows8.Core.Services;

namespace WinDeploy.Windows8.Core.Installation;

public sealed class LegacyNt6Engine : IInstallationEngine
{
    public InstallationEngineKind Kind => InstallationEngineKind.LegacyNt6;
    public Task ExecuteAsync(InstallationPlan plan, InstallationExecutionContext context,
        CancellationToken cancellationToken = default) =>
        Task.FromException(new WinDeployException("LegacyNt6EngineUnavailable", plan.Generation.ToString()));
}

public sealed class LegacyXpNt5Engine : IInstallationEngine
{
    public InstallationEngineKind Kind => InstallationEngineKind.LegacyXpNt5;
    public Task ExecuteAsync(InstallationPlan plan, InstallationExecutionContext context,
        CancellationToken cancellationToken = default) =>
        Task.FromException(new WinDeployException("LegacyXpEngineUnavailable", plan.Generation.ToString()));
}

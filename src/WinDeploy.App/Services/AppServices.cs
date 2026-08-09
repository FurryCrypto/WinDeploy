using WinDeploy.Core.Services;

namespace WinDeploy.Services;

public sealed class AppServices
{
    public AppServices()
    {
        Processes = new ProcessRunner();
        Wim = new WimService();
        Images = new ImageService(Processes, Wim);
        Disks = new DiskService(Processes);
        Compatibility = new CompatibilityService(Processes);
        PlanFactory = new InstallationPlanFactory();
        Localizer = new Localizer();
        Settings = new SettingsService();
        Worker = new WorkerClient();
    }

    public ProcessRunner Processes { get; }
    public WimService Wim { get; }
    public ImageService Images { get; }
    public DiskService Disks { get; }
    public CompatibilityService Compatibility { get; }
    public InstallationPlanFactory PlanFactory { get; }
    public Localizer Localizer { get; }
    public SettingsService Settings { get; }
    public WorkerClient Worker { get; }
}

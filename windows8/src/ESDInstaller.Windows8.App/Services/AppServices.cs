using ESDInstaller.Windows8.Core.Services;

namespace ESDInstaller.Windows8.Services;

public sealed class AppServices
{
    public AppServices()
    {
        Settings = new SettingsService();
        Localizer = new Localizer(Settings);
        Updates = new UpdateService(Settings);
        Wim = new WimService();
        Images = new ImageService(Wim);
        Disks = new DiskService();
        Compatibility = new CompatibilityService();
        PlanFactory = new InstallationPlanFactory();
        Worker = new WorkerClient();
    }
    public SettingsService Settings { get; }
    public Localizer Localizer { get; }
    public UpdateService Updates { get; }
    public WimService Wim { get; }
    public ImageService Images { get; }
    public DiskService Disks { get; }
    public CompatibilityService Compatibility { get; }
    public InstallationPlanFactory PlanFactory { get; }
    public WorkerClient Worker { get; }
}

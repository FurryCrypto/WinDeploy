using Microsoft.UI.Xaml.Controls;
using ESDInstaller.Core.Models;
using ESDInstaller.Core.Services;
using ESDInstaller.Views;

namespace ESDInstaller.Services;

public sealed class WizardCoordinator
{
    private readonly MainWindow _window;
    private readonly AppServices _services;
    private readonly Localizer _text;
    private ImagePage? _imagePage;
    private DestinationPage? _destinationPage;

    public WizardCoordinator(MainWindow window, AppServices services)
    {
        _window = window;
        _services = services;
        _text = services.Localizer;
        Session.AdvancedMode = services.Settings.Current.AdvancedMode;
    }

    public SessionState Session { get; } = new();
    public int CurrentStep { get; private set; }

    public void ShowImagePage()
    {
        CurrentStep = 0;
        _window.SetStep(0);
        _imagePage = new ImagePage(this);
        _window.PageFrame.Content = _imagePage;
        _window.SetStatus(_text.Get("StatusReady"));
    }

    public async Task OpenImageAsync()
    {
        if (CurrentStep != 0 || _imagePage is null) ShowImagePage();
        await _imagePage!.PickFileAsync();
    }

    public async Task InspectImageAsync(string path, ImagePage page)
    {
        _window.SetStatus(_text.Get("StatusInspectingImage"));
        try
        {
            await _services.Images.DisposeAsync();
            Session.Image = await _services.Images.InspectAsync(path);
            Session.Edition = null;
            Session.DestinationDisk = null;
            Session.DestinationPartition = null;
            Session.BootPartition = null;
            Session.Plan = null;
            Session.BypassWindows11Requirements = false;
            page.ShowImage(Session.Image);
            _window.SetStatus(_text.Get("StatusImageReady"));
        }
        catch (ESDInstallerException exception)
        {
            page.ShowError(_text.Get(exception.MessageKey), exception.TechnicalDetail);
            _window.SetStatus(_text.Get("StatusError"));
        }
        catch (Exception exception)
        {
            page.ShowError(_text.Get("ErrorUnexpected"), exception.Message);
            _window.SetStatus(_text.Get("StatusError"));
        }
    }

    public void ShowEditionPage()
    {
        if (Session.Image is null || Session.Image.Editions.Count == 0) return;
        CurrentStep = 1;
        _window.SetStep(1);
        _window.PageFrame.Content = new EditionPage(this, Session.Image.Editions);
        _window.SetStatus(_text.Get("StatusChooseEdition"));
    }

    public void SelectEdition(WindowsImageEdition edition) => Session.Edition = edition;

    public async Task ShowDestinationPageAsync()
    {
        if (Session.Edition is null) return;
        CurrentStep = 2;
        _window.SetStep(2);
        _destinationPage = new DestinationPage(this);
        _window.PageFrame.Content = _destinationPage;
        await RefreshDisksAsync();
    }

    public async Task RefreshDisksAsync()
    {
        if (CurrentStep != 2 || _destinationPage is null)
        {
            if (Session.Edition is null)
            {
                _window.SetStatus(_text.Get("StatusSelectImageFirst"));
                return;
            }
            await ShowDestinationPageAsync();
            return;
        }

        _window.SetStatus(_text.Get("StatusReadingDisks"));
        _destinationPage.ShowLoading(true);
        try
        {
            var diskTask = _services.Disks.GetDisksAsync();
            var compatibilityTask = _services.Compatibility.InspectHostAsync();
            await Task.WhenAll(diskTask, compatibilityTask);
            Session.Disks = diskTask.Result;
            Session.Compatibility = compatibilityTask.Result;
            Session.DestinationDisk = null;
            Session.DestinationPartition = null;
            Session.BootPartition = null;
            _destinationPage.ShowDisks(Session.Disks, Session.Compatibility);
            _window.SetStatus(_text.Get("StatusSelectDestination"));
        }
        catch (ESDInstallerException exception)
        {
            _destinationPage.ShowError(_text.Get(exception.MessageKey), exception.TechnicalDetail);
            _window.SetStatus(_text.Get("StatusError"));
        }
        finally { _destinationPage.ShowLoading(false); }
    }

    public void SelectDestination(DiskInfo disk, PartitionInfo partition)
    {
        Session.DestinationDisk = disk;
        Session.DestinationPartition = partition;
        Session.BootPartition = Session.Compatibility is null
            ? null : CompatibilityService.FindBootPartition(disk, Session.Compatibility.FirmwareMode, partition);
        Session.Plan = null;
    }

    public void ShowBootPage()
    {
        if (Session.Image is null || Session.Edition is null || Session.DestinationDisk is null ||
            Session.DestinationPartition is null || Session.Compatibility is null) return;
        CurrentStep = 3;
        _window.SetStep(3);
        var result = _services.Compatibility.CheckImageCompatibility(Session.Image, Session.Edition,
            Session.DestinationDisk, Session.DestinationPartition, Session.BootPartition, Session.Compatibility,
            Session.BypassWindows11Requirements);
        _window.PageFrame.Content = new BootPage(this, result);
        _window.SetStatus(result.IsValid ? _text.Get("StatusBootConfigurationReady") : _text.Get("StatusCompatibilityProblem"));
    }

    public bool ShowReviewPage()
    {
        try
        {
            if (Session.Image is null || Session.Edition is null || Session.DestinationDisk is null ||
                Session.DestinationPartition is null || Session.BootPartition is null || Session.Compatibility is null)
                throw new InvalidOperationException("The review session is incomplete.");

            var validation = _services.Compatibility.CheckImageCompatibility(Session.Image, Session.Edition,
                Session.DestinationDisk, Session.DestinationPartition, Session.BootPartition, Session.Compatibility,
                Session.BypassWindows11Requirements);
            if (!validation.IsValid)
                throw new InvalidOperationException("The installation plan is no longer compatible.");

            Session.Plan = _services.PlanFactory.Create(Session);
            var reviewPage = new ReviewPage(this, validation);
            CurrentStep = 4;
            _window.SetStep(4);
            _window.PageFrame.Content = reviewPage;
            _window.SetStatus(_text.Get("StatusReviewPlan"));
            StartupDiagnostics.Write($"Review page shown; plan {Session.Plan.ConfirmationFingerprint}");
            return true;
        }
        catch (Exception exception)
        {
            Session.Plan = null;
            StartupDiagnostics.Write($"Review navigation failed: {exception}");
            _window.SetStatus(_text.Get("StatusError"));
            return false;
        }
    }

    public PlanValidationResult SetWindows11RequirementsBypass(bool enabled)
    {
        Session.BypassWindows11Requirements = enabled && Session.AdvancedMode &&
            Session.Image?.Generation == WindowsGeneration.Windows11;
        Session.Plan = null;
        if (Session.Image is null || Session.Edition is null || Session.DestinationDisk is null ||
            Session.DestinationPartition is null || Session.Compatibility is null)
            return new PlanValidationResult(Array.Empty<PlanIssue>());
        return _services.Compatibility.CheckImageCompatibility(Session.Image, Session.Edition,
            Session.DestinationDisk, Session.DestinationPartition, Session.BootPartition, Session.Compatibility,
            Session.BypassWindows11Requirements);
    }

    public async Task BeginInstallationAsync()
    {
        if (Session.Plan is null) return;
        CurrentStep = 5;
        _window.SetStep(5);
        _window.SetInstallLock(true);
        var page = new ProgressPage(this);
        _window.PageFrame.Content = page;
        _window.SetStatus(_text.Get("StatusWaitingForAdministrator"));
        await page.ExecuteAsync(Session.Plan);
    }

    public void InstallationFinished(bool succeeded)
    {
        _window.SetInstallLock(false);
        _window.SetStatus(_text.Get(succeeded ? "StatusInstallationComplete" : "StatusInstallationFailed"));
    }

    public void BackFrom(int step)
    {
        switch (step)
        {
            case 1: ShowImagePage(); break;
            case 2: ShowEditionPage(); break;
            case 3: _ = ShowDestinationPageAsync(); break;
            case 4: ShowBootPage(); break;
        }
    }
}

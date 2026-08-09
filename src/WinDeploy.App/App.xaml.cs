using Microsoft.UI.Xaml;
using WinDeploy.Services;

namespace WinDeploy;

public partial class App : Application
{
    public App()
    {
        StartupDiagnostics.Reset();
        StartupDiagnostics.Write("App constructor entered");
        UnhandledException += (_, args) =>
        {
            StartupDiagnostics.Write($"UnhandledException: {args.Exception}");
            System.Diagnostics.Debug.WriteLine(args.Exception);
        };
        Services.Settings.Load();
        StartupDiagnostics.Write("Settings loaded");
        InitializeComponent();
        StartupDiagnostics.Write("App XAML initialized");
        Services.Localizer.Refresh();
    }

    public static MainWindow? MainWindowInstance { get; private set; }
    public static AppServices Services { get; } = new();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        StartupDiagnostics.Write("OnLaunched entered");
        try { OpenMainWindow(); }
        catch (Exception exception) { StartupDiagnostics.Write($"OnLaunched failed: {exception}"); throw; }
    }

    public static void OpenMainWindow()
    {
        StartupDiagnostics.Write("Creating MainWindow");
        MainWindowInstance = new MainWindow();
        StartupDiagnostics.Write("MainWindow constructed");
        MainWindowInstance.Activate();
        StartupDiagnostics.Write("MainWindow activated");
    }
}

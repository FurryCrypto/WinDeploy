using System;
using System.Windows;
using WinDeploy.Windows8.Services;

namespace WinDeploy.Windows8;

public partial class App : Application
{
    public static AppServices Services { get; private set; } = null!;
    protected override void OnStartup(StartupEventArgs e)
    {
        Services = new AppServices();
        Services.Settings.Load();
        Services.Localizer.Refresh();
        base.OnStartup(e);
        DispatcherUnhandledException += (sender, args) =>
        {
            MessageBox.Show(args.Exception.Message, "WinDeploy", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };
    }
}

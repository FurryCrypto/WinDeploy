using System;
using System.Windows;
using ESDInstaller.Windows7.Services;

namespace ESDInstaller.Windows7;

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
            MessageBox.Show(args.Exception.Message, "ESD Installer", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };
    }
}

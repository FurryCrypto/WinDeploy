using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ESDInstaller.Windows8.Controls;
using ESDInstaller.Windows8.Core.Models;
using ESDInstaller.Windows8.Services;

namespace ESDInstaller.Windows8.Views;

public partial class BootPage : Page
{
    private readonly WizardCoordinator _coordinator;
    private bool _updating;
    public BootPage(WizardCoordinator coordinator, PlanValidationResult result)
    {
        InitializeComponent(); _coordinator = coordinator;
        var session = coordinator.Session;
        Firmware.Text = session.Compatibility?.FirmwareMode.ToString() ?? App.Services.Localizer.Get("Unknown");
        Scheme.Text = session.DestinationDisk?.PartitionScheme.ToString() ?? App.Services.Localizer.Get("Unknown");
        BootPartition.Text = session.BootPartition == null ? App.Services.Localizer.Get("BootPartitionNotFound") :
            App.Services.Localizer.Format("BootPartitionSummary", session.BootPartition.DiskNumber,
                session.BootPartition.PartitionNumber, session.BootPartition.DriveDisplay,
                session.BootPartition.FileSystem, DestinationPageFormat(session.BootPartition.LengthBytes));
        BootConfiguration.Text = App.Services.Localizer.Get("WindowsBootManager");
        var showBypass = session.AdvancedMode && session.Image?.Generation == WindowsGeneration.Windows11;
        Bypass.Visibility = BypassDescription.Visibility = showBypass ? Visibility.Visible : Visibility.Collapsed;
        _updating = true; Bypass.IsChecked = session.BypassWindows11Requirements; _updating = false;
        Advanced.Visibility = session.AdvancedMode ? Visibility.Visible : Visibility.Collapsed;
        if (session.AdvancedMode && session.Compatibility != null && session.DestinationDisk != null)
            AdvancedText.Text = App.Services.Localizer.Format("AdvancedEngine", session.Image == null ? App.Services.Localizer.Get("Unknown") : session.Image.Generation.ToString()) + "\n" +
                App.Services.Localizer.Format("AdvancedFirmware", session.Compatibility.FirmwareMode) + "\n" +
                App.Services.Localizer.Format("AdvancedDiskStyle", session.DestinationDisk.PartitionScheme) + "\n" +
                App.Services.Localizer.Format("AdvancedDiskId", session.DestinationDisk.UniqueId) + "\n" +
                App.Services.Localizer.Format("AdvancedTargetOffset", session.DestinationPartition?.OffsetBytes ?? 0) + "\n" +
                App.Services.Localizer.Format("AdvancedTpm", session.Compatibility.TpmPresent, session.Compatibility.TpmReady) + "\n" +
                App.Services.Localizer.Format("AdvancedSecureBoot", session.Compatibility.SecureBootCapable, session.Compatibility.SecureBootEnabled);
        UpdateResult(result);
    }
    private void UpdateResult(PlanValidationResult result)
    {
        var bypass = _coordinator.Session.BypassWindows11Requirements;
        Compatibility.Text = result.IsValid ? App.Services.Localizer.Get(bypass ? "CompatibilityBypassed" : "CompatibilityPassed") : App.Services.Localizer.Get("CompatibilityFailed");
        Compatibility.Foreground = result.IsValid ? Brushes.DarkGreen : Brushes.DarkRed;
        if (result.IsValid)
        {
            Banner.SetSuccess(App.Services.Localizer.Get(bypass ? "UnsupportedWindows11ActiveTitle" : "BootConfigurationReady"),
                App.Services.Localizer.Get(bypass ? "UnsupportedWindows11ActiveMessage" : "BootConfigurationReadyText"));
        }
        else Banner.SetError(App.Services.Localizer.Get("BootConfigurationBlocked"), App.Services.Localizer.Get("BootConfigurationBlockedText"));
        Issues.ItemsSource = result.Issues.Select(x => new IssueItem(App.Services.Localizer.Get(x.MessageKey),
            x.Severity == PlanSeverity.Error ? SymbolKind.Error : x.Severity == PlanSeverity.Warning ? SymbolKind.Warning : SymbolKind.Information,
            x.Severity == PlanSeverity.Error ? Brushes.Firebrick : x.Severity == PlanSeverity.Warning ? Brushes.DarkOrange : new SolidColorBrush(Color.FromRgb(0, 120, 215)))).ToArray();
        Review.IsEnabled = result.IsValid;
    }
    private void Bypass_Changed(object sender, RoutedEventArgs e)
    {
        if (_updating) return;
        if (Bypass.IsChecked == true)
        {
            var accepted = MessageBox.Show(Window.GetWindow(this), App.Services.Localizer.Get("UnsupportedWindows11ConfirmText"),
                App.Services.Localizer.Get("UnsupportedWindows11ConfirmTitle"), MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            if (!accepted) { _updating = true; Bypass.IsChecked = false; _updating = false; }
        }
        UpdateResult(_coordinator.SetWindows11RequirementsBypass(Bypass.IsChecked == true));
    }
    private void Back_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(3);
    private void Review_Click(object sender, RoutedEventArgs e) => _coordinator.ShowReviewPage();
    private static string DestinationPageFormat(long bytes) { double value = bytes / 1073741824d; return value.ToString("0.0") + " GB"; }
    private sealed class IssueItem
    {
        public IssueItem(string text, SymbolKind kind, Brush foreground) { Text = text; Kind = kind; Foreground = foreground; }
        public string Text { get; }
        public SymbolKind Kind { get; }
        public Brush Foreground { get; }
    }
}

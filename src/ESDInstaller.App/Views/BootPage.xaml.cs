using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ESDInstaller.Core.Models;
using ESDInstaller.Services;

namespace ESDInstaller.Views;

public sealed partial class BootPage : Page
{
    private readonly WizardCoordinator _coordinator;
    private readonly Localizer _text = App.Services.Localizer;
    private bool _initializing = true;

    public BootPage(WizardCoordinator coordinator, PlanValidationResult validation)
    {
        InitializeComponent();
        _coordinator = coordinator;
        var session = coordinator.Session;
        FirmwareText.Text = session.Compatibility?.FirmwareMode.ToString() ?? _text.Get("Unknown");
        SchemeText.Text = session.DestinationDisk?.PartitionScheme.ToString() ?? _text.Get("Unknown");
        BootPartitionText.Text = session.BootPartition is { } boot
            ? _text.Format("BootPartitionSummary", boot.DiskNumber, boot.PartitionNumber, boot.DriveDisplay, ImagePage.FormatBytes(boot.LengthBytes), boot.FileSystem)
            : _text.Get("BootPartitionNotFound");
        BootConfigurationText.Text = _text.Get("WindowsBootManager");

        if (session.AdvancedMode)
        {
            AdvancedPanel.Visibility = Visibility.Visible;
            var yes = _text.Get("Yes");
            var no = _text.Get("No");
            string Bool(bool? value) => value == true ? yes : no;
            AdvancedDetailsText.Text = string.Join(Environment.NewLine,
                _text.Format("AdvancedEngine", session.Image?.RequiresLegacyEngine == true ? _text.Get("EngineLegacy") : "ModernWindowsEngine"),
                _text.Format("AdvancedFirmware", session.Compatibility?.FirmwareMode),
                _text.Format("AdvancedDiskStyle", session.DestinationDisk?.PartitionScheme),
                _text.Format("AdvancedDiskId", session.DestinationDisk?.UniqueId),
                _text.Format("AdvancedTargetOffset", session.DestinationPartition?.OffsetBytes),
                _text.Format("AdvancedBootPartition", session.BootPartition?.StableKey),
                _text.Format("AdvancedTpm", Bool(session.Compatibility?.TpmPresent), Bool(session.Compatibility?.TpmReady)),
                _text.Format("AdvancedSecureBoot", Bool(session.Compatibility?.SecureBootCapable), Bool(session.Compatibility?.SecureBootEnabled)));

            if (session.Image?.Generation == WindowsGeneration.Windows11)
            {
                UnsupportedWindows11Separator.Visibility = Visibility.Visible;
                UnsupportedWindows11CheckBox.Visibility = Visibility.Visible;
                UnsupportedWindows11Description.Visibility = Visibility.Visible;
                UnsupportedWindows11CheckBox.IsChecked = session.BypassWindows11Requirements;
            }
        }

        RenderValidation(validation);
        _initializing = false;
    }

    private void RenderValidation(PlanValidationResult validation)
    {
        var bypassActive = _coordinator.Session.BypassWindows11Requirements;
        CompatibilityText.Text = bypassActive && validation.IsValid
            ? _text.Get("CompatibilityBypassed")
            : validation.IsValid ? _text.Get("CompatibilityPassed") : _text.Get("CompatibilityFailed");
        ResultBar.Severity = !validation.IsValid ? InfoBarSeverity.Error
            : bypassActive ? InfoBarSeverity.Warning : InfoBarSeverity.Success;
        ResultBar.Title = !validation.IsValid ? _text.Get("BootConfigurationBlocked")
            : bypassActive ? _text.Get("UnsupportedWindows11ActiveTitle") : _text.Get("BootConfigurationReady");
        ResultBar.Message = !validation.IsValid ? _text.Get("BootConfigurationBlockedText")
            : bypassActive ? _text.Get("UnsupportedWindows11ActiveMessage") : _text.Get("BootConfigurationReadyText");
        NextButton.IsEnabled = validation.IsValid;
        IssuesPanel.Children.Clear();

        foreach (var issue in validation.Issues)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(issue.Severity == PlanSeverity.Error
                    ? ColorHelper.FromArgb(255, 253, 231, 233) : ColorHelper.FromArgb(255, 255, 244, 206)),
                BorderBrush = new SolidColorBrush(issue.Severity == PlanSeverity.Error ? Colors.IndianRed : Colors.Goldenrod),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Child = new TextBlock { Text = _text.Get(issue.MessageKey), TextWrapping = TextWrapping.Wrap }
            };
            IssuesPanel.Children.Add(border);
        }
    }

    private async void UnsupportedWindows11CheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (_initializing) return;
        var enable = UnsupportedWindows11CheckBox.IsChecked == true;
        if (enable)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Title = _text.Get("UnsupportedWindows11ConfirmTitle"),
                Content = new TextBlock
                {
                    Text = _text.Get("UnsupportedWindows11ConfirmText"),
                    TextWrapping = TextWrapping.Wrap,
                    MaxWidth = 540
                },
                PrimaryButtonText = _text.Get("UnsupportedWindows11ConfirmButton"),
                CloseButtonText = _text.Get("Cancel"),
                DefaultButton = ContentDialogButton.Close
            };
            if (await dialog.ShowAsync() != ContentDialogResult.Primary)
            {
                _initializing = true;
                UnsupportedWindows11CheckBox.IsChecked = false;
                _initializing = false;
                return;
            }
        }

        RenderValidation(_coordinator.SetWindows11RequirementsBypass(enable));
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(3);
    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        StartupDiagnostics.Write("Review Installation clicked");
        NextButton.IsEnabled = false;
        if (_coordinator.ShowReviewPage()) return;

        NextButton.IsEnabled = true;
        ResultBar.Severity = InfoBarSeverity.Error;
        ResultBar.Title = _text.Get("ErrorUnexpected");
        ResultBar.Message = _text.Get("StatusError");
        ResultBar.IsOpen = true;
    }
}

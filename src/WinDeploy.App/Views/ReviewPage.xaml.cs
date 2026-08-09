using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinDeploy.Core.Models;
using WinDeploy.Services;

namespace WinDeploy.Views;

public sealed partial class ReviewPage : Page
{
    private readonly WizardCoordinator _coordinator;
    private readonly Localizer _text = App.Services.Localizer;

    public ReviewPage(WizardCoordinator coordinator, PlanValidationResult validation)
    {
        InitializeComponent();
        _coordinator = coordinator;
        var session = coordinator.Session;
        var image = session.Image!;
        var edition = session.Edition!;
        var disk = session.DestinationDisk!;
        var partition = session.DestinationPartition!;
        var plan = session.Plan!;
        EditionText.Text = edition.Name;
        ArchitectureText.Text = edition.Architecture.ToString();
        SourceText.Text = Path.GetFileName(image.SourcePath);
        DestinationText.Text = _text.Format("ReviewDestinationSummary", disk.Number, disk.SafeDisplayName,
            partition.PartitionNumber, partition.DriveDisplay, ImagePage.FormatBytes(partition.LengthBytes));
        FirmwareText.Text = session.Compatibility!.FirmwareMode.ToString();
        SchemeText.Text = disk.PartitionScheme.ToString();
        FilesystemText.Text = "NTFS";
        BootText.Text = _text.Get("WindowsBootManager");
        if (plan.BypassWindows11Requirements)
        {
            UnsupportedWindows11Bar.Visibility = Visibility.Visible;
            UnsupportedWindows11Bar.IsOpen = true;
            UnsupportedWindows11Bar.Title = _text.Get("UnsupportedWindows11ActiveTitle");
            UnsupportedWindows11Bar.Message = _text.Get("UnsupportedWindows11ReviewMessage");
        }
        DestructiveDetailsText.Text = string.Join(Environment.NewLine,
            _text.Format("WarningDiskModel", disk.SafeDisplayName),
            _text.Format("WarningDiskNumber", disk.Number),
            _text.Format("WarningPartitionNumber", partition.PartitionNumber),
            _text.Format("WarningDriveLetter", string.IsNullOrWhiteSpace(partition.DriveDisplay) ? _text.Get("None") : partition.DriveDisplay),
            _text.Format("WarningVolumeName", string.IsNullOrWhiteSpace(partition.VolumeLabel) ? _text.Get("None") : partition.VolumeLabel),
            _text.Format("WarningCapacity", ImagePage.FormatBytes(partition.LengthBytes)));
        FingerprintText.Text = _text.Format("PlanFingerprint", plan.ConfirmationFingerprint);
    }

    private void ConfirmationCheckBox_Changed(object sender, RoutedEventArgs e) =>
        InstallButton.IsEnabled = ConfirmationCheckBox.IsChecked == true;

    private void BackButton_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(4);

    private async void InstallButton_Click(object sender, RoutedEventArgs e)
    {
        if (ConfirmationCheckBox.IsChecked != true) return;
        InstallButton.IsEnabled = false;
        await _coordinator.BeginInstallationAsync();
    }
}

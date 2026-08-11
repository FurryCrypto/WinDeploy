using System.IO;
using System.Windows;
using System.Windows.Controls;
using ESDInstaller.Windows8.Core.Models;
using ESDInstaller.Windows8.Services;

namespace ESDInstaller.Windows8.Views;

public partial class ReviewPage : Page
{
    private readonly WizardCoordinator _coordinator;
    public ReviewPage(WizardCoordinator coordinator, PlanValidationResult validation)
    {
        InitializeComponent(); _coordinator = coordinator;
        var s = coordinator.Session; var plan = s.Plan!; var disk = s.DestinationDisk!; var partition = s.DestinationPartition!;
        Image.Text = s.Edition!.Name; Architecture.Text = s.Edition.Architecture.ToString(); Source.Text = Path.GetFileName(s.Image!.SourcePath);
        Destination.Text = App.Services.Localizer.Format("ReviewDestinationSummary", disk.Number, disk.SafeDisplayName,
            partition.PartitionNumber, partition.DriveDisplay, FormatBytes(partition.LengthBytes));
        Firmware.Text = plan.FirmwareMode.ToString(); Scheme.Text = plan.PartitionScheme.ToString();
        Filesystem.Text = string.IsNullOrWhiteSpace(partition.FileSystem) ? "NTFS" : partition.FileSystem;
        Warning.SetError(App.Services.Localizer.Get("DestructiveWarning.Text"),
            App.Services.Localizer.Format("WarningDiskModel", disk.SafeDisplayName));
        Identifiers.Text = App.Services.Localizer.Format("WarningDiskNumber", disk.Number) + "\n" +
            App.Services.Localizer.Format("WarningPartitionNumber", partition.PartitionNumber) + "\n" +
            App.Services.Localizer.Format("WarningDriveLetter", string.IsNullOrWhiteSpace(partition.DriveDisplay) ? App.Services.Localizer.Get("None") : partition.DriveDisplay) + "\n" +
            App.Services.Localizer.Format("WarningVolumeName", partition.VolumeLabel) + "\n" +
            App.Services.Localizer.Format("WarningCapacity", FormatBytes(partition.LengthBytes));
        Fingerprint.Text = App.Services.Localizer.Format("PlanFingerprint", plan.ConfirmationFingerprint);
        if (plan.BypassWindows11Requirements) { BypassWarning.Text = App.Services.Localizer.Get("UnsupportedWindows11ReviewMessage"); BypassWarning.Visibility = Visibility.Visible; }
    }
    private void Confirm_Changed(object sender, RoutedEventArgs e) => Install.IsEnabled = Confirm.IsChecked == true;
    private void Back_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(4);
    private async void Install_Click(object sender, RoutedEventArgs e) => await _coordinator.BeginInstallationAsync();
    private static string FormatBytes(long bytes) => (bytes / 1073741824d).ToString("0.0") + " GB";
}

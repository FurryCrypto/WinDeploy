using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ESDInstaller.Core.Models;
using ESDInstaller.Services;

namespace ESDInstaller.Views;

public sealed partial class EditionPage : Page
{
    private readonly WizardCoordinator _coordinator;

    public EditionPage(WizardCoordinator coordinator, IReadOnlyList<WindowsImageEdition> editions)
    {
        InitializeComponent();
        _coordinator = coordinator;
        var generation = coordinator.Session.Image?.Generation ?? WindowsGeneration.Unknown;
        EditionList.ItemsSource = editions.Select(edition => new EditionItem(edition, generation)).ToArray();
        if (coordinator.Session.Edition is { } selected)
            EditionList.SelectedItem = EditionList.Items.Cast<EditionItem>().FirstOrDefault(item => item.Edition.Index == selected.Index);
    }

    private void EditionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EditionList.SelectedItem is not EditionItem item) return;
        _coordinator.SelectEdition(item.Edition);
        NextButton.IsEnabled = true;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(1);
    private async void NextButton_Click(object sender, RoutedEventArgs e) => await _coordinator.ShowDestinationPageAsync();

    private sealed class EditionItem
    {
        public EditionItem(WindowsImageEdition edition, WindowsGeneration generation)
        {
            Edition = edition;
            Name = edition.Name;
            Description = edition.Description;
            Architecture = edition.Architecture.ToString();
            Details = App.Services.Localizer.Format("EditionTechnicalDetails", edition.Index, edition.Build);
            Size = edition.ApproximateSizeBytes > 0 ? ImagePage.FormatBytes(edition.ApproximateSizeBytes) : string.Empty;
            Windows10LogoVisibility = generation == WindowsGeneration.Windows10 ? Visibility.Visible : Visibility.Collapsed;
            Windows11LogoVisibility = generation == WindowsGeneration.Windows11 ? Visibility.Visible : Visibility.Collapsed;
            OtherWindowsLogoVisibility = generation is not WindowsGeneration.Windows10 and not WindowsGeneration.Windows11
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        public WindowsImageEdition Edition { get; }
        public string Name { get; }
        public string Description { get; }
        public string Architecture { get; }
        public string Details { get; }
        public string Size { get; }
        public Visibility Windows10LogoVisibility { get; }
        public Visibility Windows11LogoVisibility { get; }
        public Visibility OtherWindowsLogoVisibility { get; }
    }
}

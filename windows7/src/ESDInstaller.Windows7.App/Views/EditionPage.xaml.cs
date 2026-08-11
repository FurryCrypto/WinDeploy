using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ESDInstaller.Windows7.Core.Models;
using ESDInstaller.Windows7.Services;

namespace ESDInstaller.Windows7.Views;

public partial class EditionPage : Page
{
    private readonly WizardCoordinator _coordinator;
    public EditionPage(WizardCoordinator coordinator, IReadOnlyList<WindowsImageEdition> editions)
    {
        InitializeComponent(); _coordinator = coordinator;
        Editions.ItemsSource = editions.Select(x => new EditionItem(x,
            App.Services.Localizer.Format("EditionTechnicalDetails", x.Index, x.Build))).ToArray();
    }
    private void Editions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = Editions.SelectedItem as EditionItem; Next.IsEnabled = item != null;
        if (item != null) _coordinator.SelectEdition(item.Edition);
    }
    private void Back_Click(object sender, RoutedEventArgs e) => _coordinator.BackFrom(1);
    private async void Next_Click(object sender, RoutedEventArgs e) => await _coordinator.ShowDestinationPageAsync();
    private sealed class EditionItem
    {
        public EditionItem(WindowsImageEdition edition, string technical) { Edition = edition; Technical = technical; }
        public WindowsImageEdition Edition { get; } public string Name => Edition.Name;
        public string Description => Edition.Description; public string Architecture => Edition.Architecture.ToString();
        public string Technical { get; }
    }
}

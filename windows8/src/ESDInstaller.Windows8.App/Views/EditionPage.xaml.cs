using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ESDInstaller.Windows8.Core.Models;
using ESDInstaller.Windows8.Services;

namespace ESDInstaller.Windows8.Views;

public partial class EditionPage : Page
{
    private readonly WizardCoordinator _coordinator;
    public EditionPage(WizardCoordinator coordinator, IReadOnlyList<WindowsImageEdition> editions)
    {
        InitializeComponent(); _coordinator = coordinator;
        var generation = coordinator.Session.Image?.Generation ?? WindowsGeneration.Unknown;
        Editions.ItemsSource = editions.Select(x => new EditionItem(x, generation,
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
        public EditionItem(WindowsImageEdition edition, WindowsGeneration generation, string technical) { Edition = edition; Generation = generation; Technical = technical; }
        public WindowsImageEdition Edition { get; } public string Name => Edition.Name;
        public string Description => Edition.Description; public string Architecture => Edition.Architecture.ToString();
        public WindowsGeneration Generation { get; }
        public string Technical { get; }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ESDInstaller.Windows8.Controls;

public enum SymbolKind
{
    File, FolderOpen, Refresh, Settings, Help, Editions, Disk, Boot, Review, Install, Information, Warning, Error, Success
}

public partial class SymbolIcon : UserControl
{
    public static readonly DependencyProperty KindProperty = DependencyProperty.Register(
        nameof(Kind), typeof(SymbolKind), typeof(SymbolIcon), new PropertyMetadata(SymbolKind.File, OnKindChanged));

    public SymbolIcon()
    {
        InitializeComponent();
        UpdateGeometry();
    }

    public SymbolKind Kind
    {
        get => (SymbolKind)GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    private static void OnKindChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) =>
        ((SymbolIcon)sender).UpdateGeometry();

    private void UpdateGeometry()
    {
        var data = Kind switch
        {
            SymbolKind.FolderOpen => "M2,7 L9,7 L11,10 L22,10 L20,21 L2,21 Z M2,7 L2,21",
            SymbolKind.Refresh => "M20,8 A8,8 0 1 0 21,15 M20,8 L20,3 M20,8 L15,8",
            SymbolKind.Settings => "M12,8 A4,4 0 1 0 12,16 A4,4 0 1 0 12,8 M12,1 L12,5 M12,19 L12,23 M1,12 L5,12 M19,12 L23,12 M4.2,4.2 L7,7 M17,17 L19.8,19.8 M19.8,4.2 L17,7 M7,17 L4.2,19.8",
            SymbolKind.Help => "M12,2 A10,10 0 1 0 12,22 A10,10 0 1 0 12,2 M9,9 A3,3 0 0 1 15,9 C15,12 12,12 12,15 M12,18 L12,18.2",
            SymbolKind.Editions => "M3,4 L8,4 L8,9 L3,9 Z M11,5 L21,5 M11,8 L19,8 M3,13 L8,13 L8,18 L3,18 Z M11,14 L21,14 M11,17 L19,17",
            SymbolKind.Disk => "M3,4 L21,4 L21,20 L3,20 Z M6,16 L6,17 M9,16 L9,17 M13,16 L18,16",
            SymbolKind.Boot => "M3,3 L21,3 L21,16 L3,16 Z M8,21 L16,21 M12,16 L12,21 M8,6 L8,13 L16,9.5 Z",
            SymbolKind.Review => "M5,2 L17,2 L21,6 L21,22 L5,22 Z M17,2 L17,6 L21,6 M8,11 L10.5,13.5 L16,8 M8,17 L17,17",
            SymbolKind.Install => "M12,2 L12,16 M7,11 L12,16 L17,11 M4,19 L4,22 L20,22 L20,19",
            SymbolKind.Information => "M12,2 A10,10 0 1 0 12,22 A10,10 0 1 0 12,2 M12,10 L12,18 M12,6 L12,7",
            SymbolKind.Warning => "M12,2 L23,22 L1,22 Z M12,8 L12,15 M12,18 L12,19",
            SymbolKind.Error => "M12,2 A10,10 0 1 0 12,22 A10,10 0 1 0 12,2 M8,8 L16,16 M16,8 L8,16",
            SymbolKind.Success => "M12,2 A10,10 0 1 0 12,22 A10,10 0 1 0 12,2 M7,12 L10.5,15.5 L17,8.5",
            _ => "M4,2 L15,2 L20,7 L20,22 L4,22 Z M15,2 L15,7 L20,7 M8,16 L16,16 M12,12 L12,20 M9,15 L12,12 L15,15"
        };
        IconPath.Data = Geometry.Parse(data);
    }
}

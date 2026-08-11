using System.Windows;
using System.Windows.Controls;
using ESDInstaller.Windows8.Core.Models;

namespace ESDInstaller.Windows8.Controls;

public partial class WindowsImageMark : UserControl
{
    public static readonly DependencyProperty GenerationProperty = DependencyProperty.Register(
        nameof(Generation), typeof(WindowsGeneration), typeof(WindowsImageMark),
        new PropertyMetadata(WindowsGeneration.Unknown, OnGenerationChanged));

    public WindowsImageMark()
    {
        InitializeComponent();
        UpdateLogo();
    }

    public WindowsGeneration Generation
    {
        get => (WindowsGeneration)GetValue(GenerationProperty);
        set => SetValue(GenerationProperty, value);
    }

    private static void OnGenerationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) =>
        ((WindowsImageMark)sender).UpdateLogo();

    private void UpdateLogo()
    {
        var isWindows11 = Generation == WindowsGeneration.Windows11;
        FlatLogo.Visibility = isWindows11 ? Visibility.Visible : Visibility.Collapsed;
        PerspectiveLogo.Visibility = isWindows11 ? Visibility.Collapsed : Visibility.Visible;
    }
}

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;
using WinDeploy.Services;

namespace WinDeploy;

public sealed partial class MainWindow : Window
{
    private readonly WizardCoordinator _coordinator;
    private readonly Localizer _text = App.Services.Localizer;
    private bool _installLocked;

    public MainWindow()
    {
        StartupDiagnostics.Write("MainWindow constructor entered");
        InitializeComponent();
        StartupDiagnostics.Write("MainWindow XAML initialized");
        Title = _text.Get("AppTitle");
        StartupDiagnostics.Write("MainWindow title localized");
        AppWindow.Resize(new SizeInt32(1000, 700));
        StartupDiagnostics.Write("MainWindow resized");
        RootGrid.SizeChanged += RootGrid_SizeChanged;
        AppWindow.Closing += (_, args) => { if (_installLocked) args.Cancel = true; };
        Closed += async (_, _) => await App.Services.Images.DisposeAsync();
        _coordinator = new WizardCoordinator(this, App.Services);
        StartupDiagnostics.Write("WizardCoordinator created");
        UpdateModeText();
        _coordinator.ShowImagePage();
        StartupDiagnostics.Write("Image page shown");
    }

    public Frame PageFrame => ContentFrame;

    public void SetStatus(string text) => StatusText.Text = text;

    public void SetStep(int index)
    {
        var steps = new[] { Step0, Step1, Step2, Step3, Step4, Step5 };
        for (var position = 0; position < steps.Length; position++)
        {
            steps[position].Background = position == index
                ? new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorHelper.FromArgb(255, 224, 239, 252))
                : new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);
            steps[position].BorderBrush = position == index
                ? new Microsoft.UI.Xaml.Media.SolidColorBrush(ColorHelper.FromArgb(255, 0, 120, 215))
                : new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.Transparent);
        }
    }

    public void SetInstallLock(bool locked)
    {
        _installLocked = locked;
        OpenImageButton.IsEnabled = !locked;
        RefreshDisksButton.IsEnabled = !locked;
        SettingsButton.IsEnabled = !locked;
    }

    private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var width = Math.Max(850, (int)e.NewSize.Width);
        var height = Math.Max(600, (int)e.NewSize.Height);
        if (width != (int)e.NewSize.Width || height != (int)e.NewSize.Height)
            AppWindow.Resize(new SizeInt32(width, height));
    }

    private async void OpenImageButton_Click(object sender, RoutedEventArgs e) => await _coordinator.OpenImageAsync();
    private async void RefreshDisksButton_Click(object sender, RoutedEventArgs e) => await _coordinator.RefreshDisksAsync();

    private async void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var language = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch };
        language.Items.Add(new ComboBoxItem { Content = _text.Get("LanguageSystem"), Tag = "system" });
        language.Items.Add(new ComboBoxItem { Content = "English", Tag = "en" });
        language.Items.Add(new ComboBoxItem { Content = "Français", Tag = "fr" });
        language.Items.Add(new ComboBoxItem { Content = "Deutsch", Tag = "de" });
        language.Items.Add(new ComboBoxItem { Content = "Lëtzebuergesch", Tag = "lb" });
        language.Items.Add(new ComboBoxItem { Content = "Srpski (latinica)", Tag = "sr-Latn" });
        language.Items.Add(new ComboBoxItem { Content = "Русский", Tag = "ru" });
        language.Items.Add(new ComboBoxItem { Content = "简体中文", Tag = "zh-Hans" });
        language.Items.Add(new ComboBoxItem { Content = "Español", Tag = "es" });
        language.Items.Add(new ComboBoxItem { Content = "Polski", Tag = "pl" });
        language.Items.Add(new ComboBoxItem { Content = "Ελληνικά", Tag = "el" });
        language.Items.Add(new ComboBoxItem { Content = "Dansk", Tag = "da" });
        language.Items.Add(new ComboBoxItem { Content = "Norsk", Tag = "nb" });
        language.Items.Add(new ComboBoxItem { Content = "Suomi", Tag = "fi" });
        language.Items.Add(new ComboBoxItem { Content = "Svenska", Tag = "sv" });
        language.Items.Add(new ComboBoxItem { Content = "Монгол", Tag = "mn" });
        language.Items.Add(new ComboBoxItem { Content = "Հայերեն", Tag = "hy" });
        language.Items.Add(new ComboBoxItem { Content = "Қазақша", Tag = "kk" });
        language.Items.Add(new ComboBoxItem { Content = "Башҡортса", Tag = "ba" });
        language.Items.Add(new ComboBoxItem { Content = "Татарча", Tag = "tt" });
        language.Items.Add(new ComboBoxItem { Content = "Qırımtatarca", Tag = "crh" });
        language.Items.Add(new ComboBoxItem { Content = "Аҧсшәа", Tag = "ab" });
        language.Items.Add(new ComboBoxItem { Content = "Ирон", Tag = "os" });
        language.SelectedItem = language.Items.Cast<ComboBoxItem>().FirstOrDefault(item =>
            string.Equals((string)item.Tag, App.Services.Settings.Current.Language, StringComparison.Ordinal)) ?? language.Items[0];
        var advanced = new CheckBox { Content = _text.Get("AdvancedMode"), IsChecked = App.Services.Settings.Current.AdvancedMode };
        var panel = new StackPanel { Spacing = 8 };
        panel.Children.Add(new TextBlock { Text = _text.Get("LanguageLabel") });
        panel.Children.Add(language);
        panel.Children.Add(advanced);
        panel.Children.Add(new TextBlock { Text = _text.Get("AdvancedModeDescription"), TextWrapping = TextWrapping.Wrap, Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Colors.DimGray) });
        var dialog = new ContentDialog
        {
            XamlRoot = RootGrid.XamlRoot,
            Title = _text.Get("SettingsTitle"),
            Content = panel,
            PrimaryButtonText = _text.Get("Save"),
            CloseButtonText = _text.Get("Cancel"),
            DefaultButton = ContentDialogButton.Primary
        };
        if (await dialog.ShowAsync() != ContentDialogResult.Primary) return;
        var selectedLanguage = (string)((ComboBoxItem)language.SelectedItem).Tag;
        var languageChanged = selectedLanguage != App.Services.Settings.Current.Language;
        App.Services.Settings.Save(selectedLanguage, advanced.IsChecked == true);
        _coordinator.Session.AdvancedMode = advanced.IsChecked == true;
        if (!App.Services.Settings.Current.AdvancedMode)
            _coordinator.Session.BypassWindows11Requirements = false;
        UpdateModeText();
        if (languageChanged)
        {
            Close();
            App.Services.Localizer.Refresh();
            App.OpenMainWindow();
        }
    }

    private async void HelpButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            XamlRoot = RootGrid.XamlRoot,
            Title = _text.Get("HelpTitle"),
            Content = new TextBlock { Text = _text.Get("HelpText"), TextWrapping = TextWrapping.Wrap, MaxWidth = 520 },
            CloseButtonText = _text.Get("Close")
        };
        await dialog.ShowAsync();
    }

    private void UpdateModeText() => ModeText.Text = App.Services.Settings.Current.AdvancedMode
        ? _text.Get("AdvancedModeEnabled") : _text.Get("StandardMode");
}

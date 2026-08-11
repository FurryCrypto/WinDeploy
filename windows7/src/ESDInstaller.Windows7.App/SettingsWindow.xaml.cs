using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ESDInstaller.Windows7.Services;

namespace ESDInstaller.Windows7;

public partial class SettingsWindow : Window
{
    private readonly string _originalLanguage;
    public SettingsWindow()
    {
        InitializeComponent();
        FlowDirection = App.Services.Settings.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        _originalLanguage = App.Services.Settings.Current.Language;
        var items = new[]
        {
            new LanguageItem(App.Services.Localizer.Get("LanguageSystem"), "system"), new LanguageItem("English", "en"),
            new LanguageItem("Français", "fr"), new LanguageItem("Deutsch", "de"), new LanguageItem("Lëtzebuergesch", "lb"),
            new LanguageItem("Srpski (latinica)", "sr-Latn"), new LanguageItem("Русский", "ru"),
            new LanguageItem("简体中文", "zh-Hans"), new LanguageItem("繁體中文", "zh-Hant"),
            new LanguageItem("Español", "es"), new LanguageItem("Polski", "pl"),
            new LanguageItem("Ελληνικά", "el"), new LanguageItem("Dansk", "da"),
            new LanguageItem("Norsk bokmål", "nb"), new LanguageItem("Norsk nynorsk", "nn"),
            new LanguageItem("Suomi", "fi"), new LanguageItem("Svenska", "sv"),
            new LanguageItem("Монгол", "mn"), new LanguageItem("Հայերեն", "hy"), new LanguageItem("Қазақша", "kk"),
            new LanguageItem("Башҡортса", "ba"), new LanguageItem("Татарча", "tt"),
            new LanguageItem("Qırımtatarca", "crh"), new LanguageItem("Аҧсшәа", "ab"),
            new LanguageItem("Ирон", "os"), new LanguageItem("العربية", "ar"), new LanguageItem("עברית", "he"),
            new LanguageItem("فارسی", "fa"), new LanguageItem("Afrikaans", "af"), new LanguageItem("Magyar", "hu"),
            new LanguageItem("Português", "pt"), new LanguageItem("Čeština", "cs"),
            new LanguageItem("Уйғурчә (кириллица)", "ug-Cyrl"), new LanguageItem("Türkçe", "tr"),
            new LanguageItem("ไทย", "th"), new LanguageItem("한국어", "ko"), new LanguageItem("日本語", "ja"),
            new LanguageItem("ქართული", "ka"), new LanguageItem("Azərbaycanca", "az"),
            new LanguageItem("Кыргызча", "ky"), new LanguageItem("Italiano", "it"),
            new LanguageItem("Română", "ro"), new LanguageItem("Íslenska", "is")
        };
        LanguagePicker.ItemsSource = items; LanguagePicker.DisplayMemberPath = "Name";
        LanguagePicker.SelectedItem = items.FirstOrDefault(x => x.Code == _originalLanguage) ?? items[0];
        Advanced.IsChecked = App.Services.Settings.Current.AdvancedMode;
        AutomaticUpdates.IsChecked = App.Services.Settings.Current.CheckForUpdatesAutomatically;
    }
    public bool LanguageChanged { get; private set; }
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var selected = (LanguageItem)LanguagePicker.SelectedItem;
        LanguageChanged = selected.Code != _originalLanguage;
        App.Services.Settings.Save(selected.Code, Advanced.IsChecked == true, AutomaticUpdates.IsChecked == true);
        DialogResult = true;
    }
    private async void CheckUpdates_Click(object sender, RoutedEventArgs e)
    {
        CheckUpdates.IsEnabled = false;
        UpdateStatus.Text = App.Services.Localizer.Get("CheckingForUpdates");
        try
        {
            var result = await App.Services.Updates.CheckAsync(true);
            if (result.Status == UpdateCheckStatus.Available && result.Manifest != null)
            {
                UpdateStatus.Text = string.Empty;
                new UpdateWindow(result.Manifest) { Owner = this }.ShowDialog();
            }
            else UpdateStatus.Text = result.Status == UpdateCheckStatus.Current
                ? App.Services.Localizer.Get("UpToDate") : App.Services.Localizer.Get("UnableToCheckForUpdates");
        }
        catch (System.OperationCanceledException) { }
        finally { CheckUpdates.IsEnabled = true; }
    }
    private sealed class LanguageItem
    {
        public LanguageItem(string name, string code) { Name = name; Code = code; }
        public string Name { get; } public string Code { get; }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WinDeploy.Windows7;

public partial class SettingsWindow : Window
{
    private readonly string _originalLanguage;
    public SettingsWindow()
    {
        InitializeComponent();
        _originalLanguage = App.Services.Settings.Current.Language;
        var items = new[]
        {
            new LanguageItem(App.Services.Localizer.Get("LanguageSystem"), "system"), new LanguageItem("English", "en"),
            new LanguageItem("Français", "fr"), new LanguageItem("Deutsch", "de"), new LanguageItem("Lëtzebuergesch", "lb"),
            new LanguageItem("Srpski (latinica)", "sr-Latn"), new LanguageItem("Русский", "ru"),
            new LanguageItem("简体中文", "zh-Hans"), new LanguageItem("Español", "es"), new LanguageItem("Polski", "pl"),
            new LanguageItem("Ελληνικά", "el"), new LanguageItem("Dansk", "da")
        };
        LanguagePicker.ItemsSource = items; LanguagePicker.DisplayMemberPath = "Name";
        LanguagePicker.SelectedItem = items.FirstOrDefault(x => x.Code == _originalLanguage) ?? items[0];
        Advanced.IsChecked = App.Services.Settings.Current.AdvancedMode;
    }
    public bool LanguageChanged { get; private set; }
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var selected = (LanguageItem)LanguagePicker.SelectedItem;
        LanguageChanged = selected.Code != _originalLanguage;
        App.Services.Settings.Save(selected.Code, Advanced.IsChecked == true);
        DialogResult = true;
    }
    private sealed class LanguageItem
    {
        public LanguageItem(string name, string code) { Name = name; Code = code; }
        public string Name { get; } public string Code { get; }
    }
}

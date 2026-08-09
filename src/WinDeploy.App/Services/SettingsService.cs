using System.Text.Json;
using System.Globalization;
using Microsoft.Windows.Globalization;

namespace WinDeploy.Services;

public sealed record AppSettings(string Language, bool AdvancedMode);

public sealed class SettingsService
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WinDeploy", "settings.json");

    public AppSettings Current { get; private set; } = new("system", false);

    public void Load()
    {
        try
        {
            if (File.Exists(_path)) Current = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_path)) ?? Current;
        }
        catch { }
        ApplyLanguage(Current.Language);
    }

    public void Save(string language, bool advancedMode)
    {
        Current = new AppSettings(language, advancedMode);
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true }));
        ApplyLanguage(language);
    }

    private static void ApplyLanguage(string language)
    {
        ApplicationLanguages.PrimaryLanguageOverride = language switch
        {
            "en" => "en-US",
            "fr" => "fr-FR",
            "de" => "de-DE",
            "lb" => "lb-LU",
            "sr-Latn" => "sr-Latn-RS",
            "ru" => "ru-RU",
            "zh-Hans" => "zh-CN",
            "es" => "es-ES",
            "pl" => "pl-PL",
            "el" => "el-GR",
            "da" => "da-DK",
            _ => ResolveSystemLanguage()
        };
    }

    private static string ResolveSystemLanguage()
    {
        var culture = CultureInfo.InstalledUICulture;
        return culture.TwoLetterISOLanguageName switch
        {
            "en" => "en-US",
            "fr" => "fr-FR",
            "de" => "de-DE",
            "lb" => "lb-LU",
            "sr" => "sr-Latn-RS",
            "ru" => "ru-RU",
            "zh" => "zh-CN",
            "es" => "es-ES",
            "pl" => "pl-PL",
            "el" => "el-GR",
            "da" => "da-DK",
            _ => "en-US"
        };
    }
}

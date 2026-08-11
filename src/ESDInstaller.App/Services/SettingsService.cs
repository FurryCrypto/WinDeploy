using System.Text.Json;
using System.Globalization;
using Microsoft.Windows.Globalization;

namespace ESDInstaller.Services;

public sealed class AppSettings
{
    public string Language { get; set; } = "system";
    public bool AdvancedMode { get; set; }
    public bool CheckForUpdatesAutomatically { get; set; } = true;
    public DateTimeOffset? LastUpdateCheckUtc { get; set; }
}

public sealed class SettingsService
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ESDInstaller", "settings.json");

    public AppSettings Current { get; private set; } = new();

    public void Load()
    {
        try
        {
            if (File.Exists(_path)) Current = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_path)) ?? Current;
        }
        catch { }
        ApplyLanguage(Current.Language);
    }

    public void Save(string language, bool advancedMode, bool checkForUpdatesAutomatically)
    {
        Current.Language = language;
        Current.AdvancedMode = advancedMode;
        Current.CheckForUpdatesAutomatically = checkForUpdatesAutomatically;
        Write();
        ApplyLanguage(language);
    }

    public void RecordUpdateCheck(DateTimeOffset checkedAtUtc)
    {
        Current.LastUpdateCheckUtc = checkedAtUtc;
        Write();
    }

    private void Write()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static void ApplyLanguage(string language)
    {
        ApplicationLanguages.PrimaryLanguageOverride = MapLanguage(language);
    }

    public bool IsRightToLeft
    {
        get
        {
            var cultureName = MapLanguage(Current.Language);
            return cultureName.StartsWith("ar-", StringComparison.OrdinalIgnoreCase) ||
                   cultureName.StartsWith("he-", StringComparison.OrdinalIgnoreCase) ||
                   cultureName.StartsWith("fa-", StringComparison.OrdinalIgnoreCase);
        }
    }

    private static string MapLanguage(string language) => language switch
    {
            "en" => "en-US",
            "fr" => "fr-FR",
            "de" => "de-DE",
            "lb" => "lb-LU",
            "sr-Latn" => "sr-Latn-RS",
            "ru" => "ru-RU",
            "zh-Hans" => "zh-CN",
            "zh-Hant" => "zh-TW",
            "es" => "es-ES",
            "pl" => "pl-PL",
            "el" => "el-GR",
            "da" => "da-DK",
            "nb" => "nb-NO",
            "no" => "nb-NO",
            "nn" => "nn-NO",
            "fi" => "fi-FI",
            "sv" => "sv-SE",
            "mn" => "mn-MN",
            "hy" => "hy-AM",
            "kk" => "kk-KZ",
            "ba" => "ba-RU",
            "tt" => "tt-RU",
            "crh" => "crh-Latn",
            "ab" => "ab-GE",
            "os" => "os-GE",
            "ar" => "ar-SA",
            "he" => "he-IL",
            "fa" => "fa-IR",
            "af" => "af-ZA",
            "hu" => "hu-HU",
            "pt" => "pt-PT",
            "cs" => "cs-CZ",
            "ug-Cyrl" => "ug-Cyrl-CN",
            "tr" => "tr-TR",
            "th" => "th-TH",
            "ko" => "ko-KR",
            "ja" => "ja-JP",
            "ka" => "ka-GE",
            "az" => "az-Latn-AZ",
            "ky" => "ky-KG",
            "it" => "it-IT",
            "ro" => "ro-RO",
            "is" => "is-IS",
            _ => ResolveSystemLanguage()
    };

    private static string ResolveSystemLanguage()
    {
        var culture = CultureInfo.InstalledUICulture;
        if (culture.Name.StartsWith("zh-TW", StringComparison.OrdinalIgnoreCase) ||
            culture.Name.StartsWith("zh-HK", StringComparison.OrdinalIgnoreCase) ||
            culture.Name.StartsWith("zh-MO", StringComparison.OrdinalIgnoreCase) ||
            culture.Name.Contains("Hant", StringComparison.OrdinalIgnoreCase)) return "zh-TW";
        if (culture.Name.StartsWith("nn-", StringComparison.OrdinalIgnoreCase)) return "nn-NO";
        return culture.TwoLetterISOLanguageName switch
        {
            "en" => "en-US",
            "fr" => "fr-FR",
            "de" => "de-DE",
            "lb" => "lb-LU",
            "sr" => "sr-Latn-RS",
            "ru" => "ru-RU",
            "zh" => "zh-CN",
            "ar" => "ar-SA",
            "he" => "he-IL",
            "fa" => "fa-IR",
            "af" => "af-ZA",
            "hu" => "hu-HU",
            "pt" => "pt-PT",
            "cs" => "cs-CZ",
            "tr" => "tr-TR",
            "th" => "th-TH",
            "ko" => "ko-KR",
            "ja" => "ja-JP",
            "ka" => "ka-GE",
            "az" => "az-Latn-AZ",
            "ky" => "ky-KG",
            "it" => "it-IT",
            "ro" => "ro-RO",
            "is" => "is-IS",
            "es" => "es-ES",
            "pl" => "pl-PL",
            "el" => "el-GR",
            "da" => "da-DK",
            "nb" => "nb-NO",
            "no" => "nb-NO",
            "fi" => "fi-FI",
            "sv" => "sv-SE",
            "mn" => "mn-MN",
            "hy" => "hy-AM",
            "kk" => "kk-KZ",
            "ba" => "ba-RU",
            "tt" => "tt-RU",
            "crh" => "crh-Latn",
            "ab" => "ab-GE",
            "os" => "os-GE",
            _ => "en-US"
        };
    }
}

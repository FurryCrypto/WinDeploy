using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace ESDInstaller.Windows7.Services;

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
        "ESDInstallerWindows7", "settings.json");
    public AppSettings Current { get; private set; } = new AppSettings();
    public void Load()
    {
        try { if (File.Exists(_path)) Current = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_path)) ?? Current; }
        catch { }
    }
    public void Save(string language, bool advancedMode, bool checkForUpdatesAutomatically)
    {
        Current.Language = language;
        Current.AdvancedMode = advancedMode;
        Current.CheckForUpdatesAutomatically = checkForUpdatesAutomatically;
        Write();
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
    public string CultureName
    {
        get
        {
            switch (Current.Language)
            {
                case "en": return "en-US"; case "fr": return "fr-FR"; case "de": return "de-DE";
                case "lb": return "lb-LU"; case "sr-Latn": return "sr-Latn-RS"; case "ru": return "ru-RU";
                case "zh-Hans": return "zh-CN"; case "zh-Hant": return "zh-TW";
                case "es": return "es-ES"; case "pl": return "pl-PL";
                case "el": return "el-GR"; case "da": return "da-DK";
                case "nb": case "no": return "nb-NO"; case "nn": return "nn-NO"; case "fi": return "fi-FI";
                case "sv": return "sv-SE"; case "mn": return "mn-MN"; case "hy": return "hy-AM";
                case "kk": return "kk-KZ";
                case "ba": return "ba-RU"; case "tt": return "tt-RU";
                case "crh": return "crh-Latn"; case "ab": return "ab-GE"; case "os": return "os-GE";
                case "ar": return "ar-SA"; case "he": return "he-IL"; case "fa": return "fa-IR";
                case "af": return "af-ZA"; case "hu": return "hu-HU"; case "pt": return "pt-PT";
                case "cs": return "cs-CZ"; case "ug-Cyrl": return "ug-Cyrl-CN"; case "tr": return "tr-TR";
                case "th": return "th-TH"; case "ko": return "ko-KR"; case "ja": return "ja-JP";
                case "ka": return "ka-GE"; case "az": return "az-Latn-AZ"; case "ky": return "ky-KG";
                case "it": return "it-IT"; case "ro": return "ro-RO"; case "is": return "is-IS";
                default:
                    var installedCulture = CultureInfo.InstalledUICulture;
                    if (installedCulture.Name.StartsWith("zh-TW", StringComparison.OrdinalIgnoreCase) ||
                        installedCulture.Name.StartsWith("zh-HK", StringComparison.OrdinalIgnoreCase) ||
                        installedCulture.Name.StartsWith("zh-MO", StringComparison.OrdinalIgnoreCase) ||
                        installedCulture.Name.IndexOf("Hant", StringComparison.OrdinalIgnoreCase) >= 0) return "zh-TW";
                    if (installedCulture.Name.StartsWith("nn-", StringComparison.OrdinalIgnoreCase)) return "nn-NO";
                    var language = installedCulture.TwoLetterISOLanguageName;
                    switch (language)
                    {
                        case "fr": return "fr-FR"; case "de": return "de-DE"; case "lb": return "lb-LU";
                        case "sr": return "sr-Latn-RS"; case "ru": return "ru-RU"; case "zh": return "zh-CN";
                        case "ar": return "ar-SA"; case "he": return "he-IL"; case "fa": return "fa-IR";
                        case "af": return "af-ZA"; case "hu": return "hu-HU"; case "pt": return "pt-PT";
                        case "cs": return "cs-CZ"; case "tr": return "tr-TR"; case "th": return "th-TH";
                        case "ko": return "ko-KR"; case "ja": return "ja-JP"; case "ka": return "ka-GE";
                        case "az": return "az-Latn-AZ"; case "ky": return "ky-KG"; case "it": return "it-IT";
                        case "ro": return "ro-RO"; case "is": return "is-IS";
                        case "es": return "es-ES"; case "pl": return "pl-PL"; case "el": return "el-GR";
                        case "da": return "da-DK";
                        case "nb": case "no": return "nb-NO"; case "fi": return "fi-FI";
                        case "sv": return "sv-SE"; case "mn": return "mn-MN"; case "hy": return "hy-AM";
                        case "kk": return "kk-KZ"; case "ba": return "ba-RU"; case "tt": return "tt-RU";
                        case "crh": return "crh-Latn"; case "ab": return "ab-GE"; case "os": return "os-GE";
                        default: return "en-US";
                    }
            }
        }
    }
    public bool IsRightToLeft => CultureName.StartsWith("ar-", StringComparison.OrdinalIgnoreCase) ||
        CultureName.StartsWith("he-", StringComparison.OrdinalIgnoreCase) ||
        CultureName.StartsWith("fa-", StringComparison.OrdinalIgnoreCase);
}

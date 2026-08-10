using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace WinDeploy.Windows8.Services;

public sealed record AppSettings(string Language, bool AdvancedMode);

public sealed class SettingsService
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WinDeployWindows8", "settings.json");
    public AppSettings Current { get; private set; } = new AppSettings("system", false);
    public void Load()
    {
        try { if (File.Exists(_path)) Current = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(_path)) ?? Current; }
        catch { }
    }
    public void Save(string language, bool advancedMode)
    {
        Current = new AppSettings(language, advancedMode);
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonConvert.SerializeObject(Current, Formatting.Indented));
    }
    public string CultureName
    {
        get
        {
            switch (Current.Language)
            {
                case "en": return "en-US"; case "fr": return "fr-FR"; case "de": return "de-DE";
                case "lb": return "lb-LU"; case "sr-Latn": return "sr-Latn-RS"; case "ru": return "ru-RU";
                case "zh-Hans": return "zh-CN"; case "es": return "es-ES"; case "pl": return "pl-PL";
                case "el": return "el-GR"; case "da": return "da-DK";
                case "nb": case "no": return "nb-NO"; case "fi": return "fi-FI";
                case "sv": return "sv-SE"; case "mn": return "mn-MN"; case "hy": return "hy-AM";
                case "kk": return "kk-KZ";
                case "ba": return "ba-RU"; case "tt": return "tt-RU";
                case "crh": return "crh-Latn"; case "ab": return "ab-GE"; case "os": return "os-GE";
                default:
                    var language = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                    switch (language)
                    {
                        case "fr": return "fr-FR"; case "de": return "de-DE"; case "lb": return "lb-LU";
                        case "sr": return "sr-Latn-RS"; case "ru": return "ru-RU"; case "zh": return "zh-CN";
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
}

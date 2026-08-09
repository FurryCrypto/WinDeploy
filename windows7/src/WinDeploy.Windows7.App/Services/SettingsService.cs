using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace WinDeploy.Windows7.Services;

public sealed record AppSettings(string Language, bool AdvancedMode);

public sealed class SettingsService
{
    private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WinDeployWindows7", "settings.json");
    public AppSettings Current { get; private set; } = new AppSettings("system", false);
    public void Load()
    {
        try { if (File.Exists(_path)) Current = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_path)) ?? Current; }
        catch { }
    }
    public void Save(string language, bool advancedMode)
    {
        Current = new AppSettings(language, advancedMode);
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
                case "zh-Hans": return "zh-CN"; case "es": return "es-ES"; case "pl": return "pl-PL";
                case "el": return "el-GR"; case "da": return "da-DK";
                default:
                    var language = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
                    switch (language)
                    {
                        case "fr": return "fr-FR"; case "de": return "de-DE"; case "lb": return "lb-LU";
                        case "sr": return "sr-Latn-RS"; case "ru": return "ru-RU"; case "zh": return "zh-CN";
                        case "es": return "es-ES"; case "pl": return "pl-PL"; case "el": return "el-GR";
                        case "da": return "da-DK"; default: return "en-US";
                    }
            }
        }
    }
}

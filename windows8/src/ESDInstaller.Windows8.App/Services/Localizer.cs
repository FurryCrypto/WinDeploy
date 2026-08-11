using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ESDInstaller.Windows8.Services;

public sealed class Localizer
{
    private readonly SettingsService _settings;
    private Dictionary<string, string> _strings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, string> _fallback = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public Localizer(SettingsService settings) => _settings = settings;
    public void Refresh()
    {
        _fallback = Load("en-US");
        _strings = Load(_settings.CultureName);
    }
    public string Get(string key)
    {
        string value;
        if (_strings.TryGetValue(key, out value)) return value;
        if (_fallback.TryGetValue(key, out value)) return value;
        return key;
    }
    public string Format(string key, params object[] arguments) => string.Format(Get(key), arguments);
    private static Dictionary<string, string> Load(string culture)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Strings", culture, "Resources.resw");
        if (!File.Exists(path)) return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var document = XDocument.Load(path, LoadOptions.PreserveWhitespace);
        return document.Descendants("data").Where(x => x.Attribute("name") != null)
            .ToDictionary(x => x.Attribute("name")!.Value,
                x => x.Element("value")?.Value ?? string.Empty, StringComparer.OrdinalIgnoreCase);
    }
}

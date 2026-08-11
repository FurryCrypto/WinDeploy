using Microsoft.Windows.ApplicationModel.Resources;

namespace ESDInstaller.Services;

public sealed class Localizer
{
    private ResourceLoader? _loader;

    public string Get(string key)
    {
        try
        {
            _loader ??= new ResourceLoader();
            var value = _loader.GetString(key);
            return string.IsNullOrEmpty(value) ? key : value;
        }
        catch { return key; }
    }

    public string Format(string key, params object?[] values) => string.Format(Get(key), values);

    public void Refresh() => _loader = null;
}

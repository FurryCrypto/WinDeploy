using System;
using System.Windows.Markup;

namespace ESDInstaller.Windows8.Markup;

[MarkupExtensionReturnType(typeof(string))]
public sealed class LocExtension : MarkupExtension
{
    public LocExtension() { }
    public LocExtension(string key) => Key = key;
    [ConstructorArgument("key")] public string Key { get; set; } = string.Empty;
    public override object ProvideValue(IServiceProvider serviceProvider) => App.Services.Localizer.Get(Key);
}

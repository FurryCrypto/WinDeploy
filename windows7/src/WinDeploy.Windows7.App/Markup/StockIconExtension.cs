using System;
using System.Windows.Markup;
using System.Windows.Media;
using WinDeploy.Windows7.Services;

namespace WinDeploy.Windows7.Markup;

[MarkupExtensionReturnType(typeof(ImageSource))]
public sealed class StockIconExtension : MarkupExtension
{
    public StockIconExtension() { }
    public StockIconExtension(StockIconId id) => Id = id;
    [ConstructorArgument("id")] public StockIconId Id { get; set; }
    public bool Large { get; set; }
    public override object ProvideValue(IServiceProvider serviceProvider) => ShellIconService.Get(Id, Large);
}

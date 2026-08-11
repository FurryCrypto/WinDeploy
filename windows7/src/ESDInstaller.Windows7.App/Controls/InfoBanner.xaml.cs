using System.Windows.Controls;
using System.Windows.Media;
using ESDInstaller.Windows7.Services;

namespace ESDInstaller.Windows7.Controls;

public partial class InfoBanner : UserControl
{
    public InfoBanner() { InitializeComponent(); SetInformation("", ""); }
    public void SetInformation(string title, string message) => Set(title, message, "#FFF3F8FC", "#FF86B9DD", StockIconId.Information);
    public void SetWarning(string title, string message) => Set(title, message, "#FFFFF8D8", "#FFE1B844", StockIconId.Warning);
    public void SetError(string title, string message) => Set(title, message, "#FFFFEAEA", "#FFD77C7C", StockIconId.Error);
    public void SetSuccess(string title, string message) => Set(title, message, "#FFEAF6E8", "#FF78B86E", StockIconId.Information);
    private void Set(string title, string message, string background, string border, StockIconId icon)
    {
        TitleText.Text = title; MessageText.Text = message;
        Border.Background = (Brush)new BrushConverter().ConvertFromString(background)!;
        Border.BorderBrush = (Brush)new BrushConverter().ConvertFromString(border)!;
        Icon.Source = ShellIconService.Get(icon);
    }
}

using System.Windows.Controls;
using System.Windows.Media;

namespace WinDeploy.Windows8.Controls;

public partial class InfoBanner : UserControl
{
    public InfoBanner() { InitializeComponent(); SetInformation("", ""); }
    public void SetInformation(string title, string message) => Set(title, message, "#FFF3F8FC", "#FF86B9DD", "#FF0078D7", SymbolKind.Information);
    public void SetWarning(string title, string message) => Set(title, message, "#FFFFF8D8", "#FFE1B844", "#FF8A4B00", SymbolKind.Warning);
    public void SetError(string title, string message) => Set(title, message, "#FFFFEAEA", "#FFD77C7C", "#FFC42B1C", SymbolKind.Error);
    public void SetSuccess(string title, string message) => Set(title, message, "#FFEAF6E8", "#FF78B86E", "#FF107C10", SymbolKind.Success);

    private void Set(string title, string message, string background, string border, string foreground, SymbolKind kind)
    {
        TitleText.Text = title;
        MessageText.Text = message;
        BannerBorder.Background = Parse(background);
        BannerBorder.BorderBrush = Parse(border);
        Icon.Foreground = Parse(foreground);
        Icon.Kind = kind;
    }

    private static Brush Parse(string value) => (Brush)new BrushConverter().ConvertFromString(value)!;
}

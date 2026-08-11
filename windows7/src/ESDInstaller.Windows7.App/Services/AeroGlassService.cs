using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ESDInstaller.Windows7.Services;

public static class AeroGlassService
{
    public static bool TryExtend(Window window, int topPixels)
    {
        if (Environment.OSVersion.Version.Major < 6 || !IsCompositionEnabled()) return false;
        try
        {
            var helper = new WindowInteropHelper(window);
            var source = HwndSource.FromHwnd(helper.Handle);
            if (source?.CompositionTarget != null) source.CompositionTarget.BackgroundColor = Colors.Transparent;
            var margins = new Margins { Top = topPixels };
            return DwmExtendFrameIntoClientArea(helper.Handle, ref margins) == 0;
        }
        catch { return false; }
    }

    private static bool IsCompositionEnabled()
    {
        try { bool enabled; return DwmIsCompositionEnabled(out enabled) == 0 && enabled; }
        catch { return false; }
    }

    [StructLayout(LayoutKind.Sequential)] private struct Margins { public int Left, Right, Top, Bottom; }
    [DllImport("dwmapi.dll")] private static extern int DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] out bool enabled);
    [DllImport("dwmapi.dll")] private static extern int DwmExtendFrameIntoClientArea(IntPtr window, ref Margins margins);
}

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinDeploy.Windows7.Services;

public enum StockIconId
{
    Application = 2, Folder = 3, FolderOpen = 4, DriveFixed = 8, DriveRemove = 7,
    Computer = 15, Network = 17, Information = 79, Warning = 78, Error = 80,
    Help = 23, Settings = 21, File = 0, Shield = 77
}

public static class ShellIconService
{
    public static ImageSource Get(StockIconId id, bool large = false)
    {
        var info = new StockIconInfo { cbSize = (uint)Marshal.SizeOf(typeof(StockIconInfo)) };
        var flags = 0x000000100u | (large ? 0u : 1u); // SHGSI_ICON | SHGSI_SMALLICON
        if (SHGetStockIconInfo((uint)id, flags, ref info) == 0 && info.hIcon != IntPtr.Zero)
        {
            try
            {
                var source = Imaging.CreateBitmapSourceFromHIcon(info.hIcon, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                source.Freeze();
                return source;
            }
            finally { DestroyIcon(info.hIcon); }
        }
        return Imaging.CreateBitmapSourceFromHIcon(System.Drawing.SystemIcons.Application.Handle,
            Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct StockIconInfo
    {
        public uint cbSize;
        public IntPtr hIcon;
        public int iSysImageIndex;
        public int iIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szPath;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern int SHGetStockIconInfo(uint id, uint flags, ref StockIconInfo info);
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DestroyIcon(IntPtr icon);
}

using ManagedWimLib;

namespace ESDInstaller.Windows7.Core.Services;

internal static class WimLibNative
{
    private static readonly object Sync = new object();
    private static bool _initialized;

    public static void EnsureInitialized()
    {
        lock (Sync)
        {
            if (_initialized) return;
            var architecture = Environment.Is64BitProcess ? "x64" : "x86";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, architecture, "libwim-15.dll");
            if (!File.Exists(path))
                throw new ESDInstallerException("ErrorImageOpen", "The Windows 7 imaging library is missing: " + path);
            try { Wim.GlobalInit(path, InitFlags.None); }
            catch (Exception exception)
            { throw new ESDInstallerException("ErrorImageOpen", "The Windows 7 imaging library could not be initialized.", exception); }
            _initialized = true;
        }
    }
}

using ManagedWimLib;

namespace ESDInstaller.Windows8.Core.Services;

internal static class WimLibNative
{
    private const ushort ImageFileMachineI386 = 0x014c;
    private const ushort ImageFileMachineAmd64 = 0x8664;
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
                throw new ESDInstallerException("ErrorImageOpen", "The imaging library is missing: " + path);

            var expectedMachine = Environment.Is64BitProcess ? ImageFileMachineAmd64 : ImageFileMachineI386;
            var actualMachine = ReadPeMachine(path);
            if (actualMachine != expectedMachine)
            {
                throw new ESDInstallerException(
                    "ErrorImageOpen",
                    string.Format(
                        "The imaging library architecture is invalid (expected 0x{0:X4}, found 0x{1:X4}): {2}",
                        expectedMachine,
                        actualMachine,
                        path));
            }

            try { Wim.GlobalInit(path, InitFlags.None); }
            catch (Exception exception)
            { throw new ESDInstallerException("ErrorImageOpen", "The imaging library could not be initialized.", exception); }
            _initialized = true;
        }
    }

    internal static ushort ReadPeMachine(string path)
    {
        try
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(stream))
            {
                if (stream.Length < 64 || reader.ReadUInt16() != 0x5a4d)
                    throw new InvalidDataException("The file does not contain a valid DOS header.");

                stream.Position = 0x3c;
                var peOffset = reader.ReadInt32();
                if (peOffset < 0 || peOffset > stream.Length - 6)
                    throw new InvalidDataException("The PE header offset is invalid.");

                stream.Position = peOffset;
                if (reader.ReadUInt32() != 0x00004550)
                    throw new InvalidDataException("The file does not contain a valid PE header.");

                return reader.ReadUInt16();
            }
        }
        catch (ESDInstallerException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ESDInstallerException("ErrorImageOpen", "The imaging library is not a valid Windows binary: " + path, exception);
        }
    }
}

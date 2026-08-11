# ESD Installer for Windows 8 and 8.1

This directory contains the separate Windows 8/8.1 edition of ESD Installer. It preserves the current Windows 10-style interface, wizard structure, pages, localization, safety model, worker separation, and deployment workflow.

The Windows 7 WPF edition under `windows7/` and the Windows 10/11 WinUI edition under `src/` are not referenced or modified by this edition.

## Runtime requirements

- Windows 8 or Windows 8.1 desktop, x86 or x64 (Windows RT is not supported)
- .NET Framework 4.6.1 or a later compatible in-place .NET Framework 4.x runtime
- Administrator approval when installation begins

.NET Framework 4.6.1 is the target because it is the newest .NET Framework release that can be installed on Windows 8. The same binaries also run on Windows 8.1 when a compatible .NET Framework 4.x runtime is installed.

## Preserved workflow

The separate WPF shell keeps the six existing pages and their order:

1. Windows Image
2. Edition
3. Destination
4. Boot Configuration
5. Review
6. Install

It retains ISO/WIM/ESD inspection, real image indexes, disk and partition selection, Windows 11 compatibility warnings and explicit Advanced bypass, destructive confirmation, elevated worker isolation, detailed logs, boot-file verification, restart controls, and all existing languages. Windows XP and Vista remain routed to unavailable legacy engines rather than being forced through the modern deployment path.

## Windows 8 implementation

- WPF and .NET Framework 4.6.1 provide genuine Windows 8/8.1 compatibility while reproducing the Windows 10 visual language used by the main edition.
- The Windows 10-style application icon and vector UI symbols are bundled with this edition, so its appearance does not depend on Windows 10 fonts.
- DiscUtils reads ISO 9660/UDF media directly; the application does not depend on `Mount-DiskImage`.
- ManagedWimLib/wimlib applies WIM and ESD images, including LZMS-compressed ESD media.
- The build explicitly packages the x86 and AMD64 wimlib binaries and verifies their PE architecture. This avoids a known ManagedWimLib 2.5.3 `net46` packaging error that maps its ARM64 binary into the `x64` directory.
- The installer deploys Microsoft's down-level Universal CRT 10.0.14393 files app-locally for the detected x86/x64 host. A fully updated Windows installation can provide the same runtime through KB2999226, but the app does not depend on that update being present.
- WMI enumerates disks and partitions. DiskPart receives only the already validated disk and partition numbers; stable model, serial/PNP identity, size, offset, and length are checked again immediately before use.
- BCDBoot arguments adapt to the host version. The `/f` switch is used only when that host BCDBoot supports it.

## Build and test

```powershell
dotnet build windows8\ESDInstaller.Windows8.sln -c Release
windows8\tests\ESDInstaller.Windows8.Tests\bin\Release\net461\ESDInstaller.Windows8.Tests.exe
```

The SDK-style projects use the NuGet .NET Framework 4.6.1 reference-assembly package, so Visual Studio's targeting pack is not required on the build computer. The runtime computer still needs .NET Framework 4.6.1 or a later compatible in-place 4.x runtime.

To build the installer after a Release build:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File windows8\installer\build-installer.ps1
```

The installer builder locates the Microsoft down-level UCRT redistributables from Windows SDK 14393 or Visual Studio's Remote Debugger components. A custom location containing `x86` and `x64` subdirectories can be supplied with `-UcrtRedistRoot`.

The installer has distinct application, registry, Start-menu, and uninstall identities. It does not replace or uninstall either existing ESD Installer edition.

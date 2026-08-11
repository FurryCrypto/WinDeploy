# ESD Installer for Windows 7

This directory contains the separate Windows 7 SP1 edition of ESD Installer. It targets WPF and .NET Framework 4.8 and preserves the current wizard structure, pages, localization, safety model, worker separation, and deployment workflow.

The original Windows 10/11 WinUI projects under `src/` are not referenced or modified by this edition.

## Runtime requirements

- Windows 7 SP1 or later
- .NET Framework 4.8
- Administrator approval when installation begins

Windows 7 does not include ISO mounting or the modern Storage PowerShell module. This edition reads ISO/UDF media directly and uses WMI plus a narrowly scoped DiskPart script after validating the immutable installation plan.

## Preserved workflow

The separate WPF shell keeps the six original pages and their order:

1. Windows Image
2. Edition
3. Destination
4. Boot Configuration
5. Review
6. Install

It retains ISO/WIM/ESD inspection, real image indexes, disk and partition selection, Windows 11 compatibility warnings and explicit Advanced bypass, destructive confirmation, elevated worker isolation, detailed logs, boot-file verification, and restart controls. Windows XP and Vista remain routed to unavailable legacy engines rather than being handled by the modern deployment path.

## Windows 7 implementation

- WPF and .NET Framework 4.8 use native Windows 7 Aero controls, title bar behavior, and a Basic-compatible fallback.
- Windows shell stock icons provide the familiar Windows 7 icon style; the application-specific deployment icon is an original multi-resolution asset.
- DiscUtils reads ISO 9660/UDF media directly, so no unsupported `Mount-DiskImage` call is made.
- ManagedWimLib/wimlib applies WIM and ESD images on Windows 7, including LZMS-compressed ESD media.
- WMI enumerates disks and partitions. DiskPart receives only the already validated disk and partition numbers; stable model, serial/PNP identity, size, offset, and length are checked again immediately before use.
- BCDBoot arguments adapt to the host version. The `/f` switch is used only when that host BCDBoot actually supports it.

## Build and test

```powershell
dotnet build windows7\ESDInstaller.Windows7.sln -c Release
windows7\tests\ESDInstaller.Windows7.Tests\bin\Release\net48\ESDInstaller.Windows7.Tests.exe
```

The SDK-style projects use the NuGet .NET Framework 4.8 reference-assembly package, so Visual Studio's targeting pack is not required on the build computer. The runtime computer still needs .NET Framework 4.8.

To build the installer and portable archive after a Release build:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File windows7\installer\build-installer.ps1
```

The Windows 7 installer uses distinct application, registry, Start-menu, and uninstall names and does not replace or uninstall the Windows 10/11 WinUI edition.

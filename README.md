# WinDeploy

WinDeploy is a native Windows desktop utility for applying a Windows installation image directly to one existing partition, installing the matching boot files, verifying the result, and then restarting into the deployed system. It does not use Electron or embedded web content.

Two native editions are maintained in this repository:

- **Windows 10/11 edition:** C#, .NET 8, and WinUI 3.
- **Windows 7 edition:** C#, .NET Framework 4.8, and WPF, with Aero integration and a Windows 7 Basic fallback.

> [!CAUTION]
> **Destructive-use and liability notice:** WinDeploy performs privileged disk-formatting, image-deployment, and boot-configuration operations. A mistake, software defect, power loss, storage failure, or incorrect selection can permanently erase data, damage partition layouts, or leave a computer unbootable. You are solely responsible for verified backups, correct target selection, installation media, and recovery capability. To the fullest extent permitted by law, the author and contributors are not responsible for data loss, unintended wipes, failed or "dead" storage devices, downtime, or hardware, software, or consequential damage. Read the full [Disclaimer](DISCLAIMER.md) before use.

The application includes multiple safety checks and refuses workflows it cannot validate, but no disk-deployment utility can eliminate every risk.

## Platform support

| Edition | Host operating system | UI framework | Availability |
| --- | --- | --- | --- |
| Windows 10/11 | Windows 10 or Windows 11 | .NET 8 and WinUI 3 | Released |
| Windows 7 | Windows 7 SP1 or later | .NET Framework 4.8 and WPF | Source available; binary release pending hardware and VM testing |

The Windows 7-compatible edition has now been added under [`windows7/`](windows7/). It preserves the same six-page installation workflow, safety model, features, controls, localization, and elevated-worker separation while replacing Windows 10-only platform dependencies. No Windows 7 binary has been added to GitHub Releases yet because this edition is still awaiting user testing.

## Supported first-release workflow

- Host application: Windows 7 SP1, Windows 10, or Windows 11, using the corresponding native edition.
- Source files: `.iso`, `.wim`, and `.esd`.
- Image deployment: Windows 7, 8, 8.1, 10, and 11 WIM/ESD indexes.
- Destination: one existing, suitable partition on an enumerated physical disk.
- Firmware layouts: UEFI/GPT and Legacy BIOS/MBR when the image and machine support them.
- Boot files: an existing same-disk EFI System Partition for UEFI, or an existing active NTFS partition for BIOS.
- Languages: English, French, German, Luxembourgish, Serbian (Latin), Russian, Simplified Chinese, Spanish, Polish, Greek, and Danish, plus system-language selection.

Windows Vista media is routed to the separate legacy NT6 engine and Windows XP media to the legacy NT5 engine. Those engines deliberately report that they are unavailable instead of attempting an invalid WIM-era deployment. Split `install.swm` media, raw/unpartitioned disks, partition creation, and cross-disk EFI modification are blocked in this release. Unsupported Windows 11 hardware remains blocked by default; Advanced Mode provides a separately confirmed bypass for CPU, TPM, Secure Boot, RAM, storage, and UEFI policy checks while retaining architecture, disk-identity, partition, and boot safety checks.

## Safety architecture

The unelevated UI process (WinUI on Windows 10/11 or WPF on Windows 7) performs inspection and planning. Installation creates an immutable `InstallationPlan` containing the source metadata, WIM index, disk PnP ID and serial, disk size and scheme, partition number, offset, length, GUID, boot partition, and firmware mode. The confirmation page shows the target model, disk number, partition number, drive letter, label, and capacity.

Only after confirmation does WinDeploy launch `WinDeploy.Worker.exe` with UAC. The worker:

1. Reopens the WIM/ESD and verifies the selected index.
2. Re-enumerates the physical disk and checks its stable identity.
3. Rechecks the target and boot partition geometry, GUID, protected state, and firmware layout.
4. Formats only the selected partition as NTFS.
5. Runs DISM with `/Apply-Image`, `/CheckIntegrity`, and `/Verify` while reading real progress.
6. Runs BCDBoot against the validated same-disk boot partition.
7. Verifies critical installed files, Windows Boot Manager, and the architecture-appropriate UEFI fallback file (or the BIOS BCD store).
8. Reports success only if every critical operation returns success and every file check passes.

On a secondary UEFI disk, BCDBoot's `/s` behavior intentionally avoids modifying NVRAM. The standard UEFI fallback loader is verified; the user may need to select that disk once in the firmware boot menu. This follows Microsoft's documented [BCDBoot `/s` behavior](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/bcdboot-command-line-options-techref-di?view=windows-11).

## Build the Windows 10/11 edition

Requirements:

- Windows 10 version 1809 or later, or Windows 11
- .NET 8 SDK (a newer SDK capable of targeting .NET 8 also works)
- Windows App SDK NuGet packages (restored automatically)
- Windows 10/11 SDK

```powershell
dotnet restore WinDeploy.slnx
dotnet build src\WinDeploy.App\WinDeploy.App.csproj -c Release -r win-x64 -p:Platform=x64
dotnet run --project tests\WinDeploy.Core.Tests\WinDeploy.Core.Tests.csproj -c Release
```

The main executable is `WinDeploy.exe`. The elevated worker and its private runtime files in the `Worker` subdirectory are required and must remain with the application.

## Build the Windows 7 edition

Requirements:

- Windows 7 SP1 or later
- .NET Framework 4.8 on the runtime computer
- A current .NET SDK for building; the project restores its .NET Framework reference assemblies automatically

```powershell
dotnet build windows7\WinDeploy.Windows7.sln -c Release
windows7\tests\WinDeploy.Windows7.Tests\bin\Release\net48\WinDeploy.Windows7.Tests.exe
```

The Windows 7 edition reads ISO/UDF media directly because Windows 7 does not provide `Mount-DiskImage`. It uses ManagedWimLib/wimlib for WIM and ESD operations, WMI for disk enumeration, a validated narrowly scoped DiskPart operation, and the host's BCDBoot. See the dedicated [Windows 7 documentation](windows7/README.md) for details. Its installer and portable archive will remain unpublished until testing is complete.

## Logs

Installation logs are written to:

```text
%LOCALAPPDATA%\WinDeploy\Logs\Install-yyyy-MM-dd-HHmmss.log
```

Logs include the selected image/index, stable disk and partition identifiers, commands, output, exit codes, elapsed times, verification stages, and failures. They intentionally avoid collecting credentials or unrelated user data.

## Project layout

```text
src/
  WinDeploy.App/       WinUI 3 wizard, localization, disk map, UAC bridge
  WinDeploy.Core/      models, WIM API, disk/image services, validators, engines
  WinDeploy.Worker/    narrowly scoped elevated execution process
tests/
  WinDeploy.Core.Tests/ dependency-free safety and read-only integration tests
docs/
  SAFETY.md            threat model and first-release limitations
windows7/
  src/                 separate WPF/.NET Framework 4.8 application, core, and worker
  tests/               Windows 7-compatible safety tests
  installer/           installer source (no untested binary is published)
```

DISM `/Apply-Image` and its verification options are documented in Microsoft's [DISM image-management reference](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-image-management-command-line-options-s14?view=windows-11). WIM metadata is read with Microsoft's [Windows Imaging API](https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/wim/dd834949%28v%3Dmsdn.10%29?view=windows-11).

## License

WinDeploy is released under the [MIT License](LICENSE).

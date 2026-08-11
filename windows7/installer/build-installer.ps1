param([string]$Version = '0.1.10')
$ErrorActionPreference = 'Stop'
$repository = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$windows7 = Split-Path $PSScriptRoot -Parent
$source = Join-Path $windows7 'src\ESDInstaller.Windows7.App\bin\Release\net48'
$staging = Join-Path $repository "work\windows7-package-$Version"
$outputs = Join-Path $repository 'outputs'
$installer = Join-Path $outputs "ESD-Installer-Windows7-Setup-$Version.exe"
$icon = Join-Path $windows7 'src\ESDInstaller.Windows7.App\Assets\ESDInstaller.Windows7.ico'
$nsi = Join-Path $PSScriptRoot 'ESDInstaller.Windows7.nsi'
$compiler = Get-ChildItem -LiteralPath (Join-Path $repository 'work\tools') -Recurse -Filter makensis.exe | Select-Object -First 1
if ($null -eq $compiler) { throw 'makensis.exe was not found under work\tools.' }
if (-not (Test-Path (Join-Path $source 'ESDInstaller.Windows7.exe'))) { throw 'Build the Windows 7 solution first.' }
if (Test-Path $staging) { Remove-Item -LiteralPath $staging -Recurse -Force }
New-Item -ItemType Directory -Path $staging -Force | Out-Null
Copy-Item -Path (Join-Path $source '*') -Destination $staging -Recurse -Force
Get-ChildItem -LiteralPath $staging -Recurse -Filter '*.pdb' | Remove-Item -Force
Copy-Item -LiteralPath (Join-Path $repository 'LICENSE') -Destination (Join-Path $staging 'LICENSE.txt')
Copy-Item -LiteralPath (Join-Path $windows7 'README.md') -Destination (Join-Path $staging 'README.md')
Copy-Item -LiteralPath (Join-Path $windows7 'THIRD-PARTY-NOTICES.md') -Destination (Join-Path $staging 'THIRD-PARTY-NOTICES.md')
New-Item -ItemType Directory -Path $outputs -Force | Out-Null
if (Test-Path $installer) { Remove-Item -LiteralPath $installer -Force }
$bytes = (Get-ChildItem -LiteralPath $staging -Recurse -File | Measure-Object Length -Sum).Sum
$sizeKb = [Math]::Ceiling($bytes / 1KB)
& $compiler.FullName /V2 /INPUTCHARSET UTF8 "/DAPP_VERSION=$Version" "/DAPP_SOURCE=$staging" "/DAPP_ICON=$icon" "/DOUTPUT_FILE=$installer" "/DAPP_SIZE_KB=$sizeKb" $nsi
if ($LASTEXITCODE -ne 0 -or -not (Test-Path $installer)) { throw "NSIS failed with exit code $LASTEXITCODE." }
Get-Item $installer | Select-Object FullName,Length

param(
    [string]$Version = '0.1.7'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$publishPath = Join-Path $root "work\publish-$Version-final-win-x64"
$iconPath = Join-Path $root 'src\WinDeploy.App\Assets\WinDeploy.ico'
$scriptPath = Join-Path $PSScriptRoot 'WinDeploy.nsi'
$outputPath = Join-Path $root "outputs\WinDeploy-Setup-$Version.exe"
$compiler = Get-ChildItem -LiteralPath (Join-Path $root 'work\tools') -Recurse -Filter makensis.exe |
    Select-Object -First 1

if ($null -eq $compiler) { throw 'makensis.exe was not found under work\tools.' }
if (-not (Test-Path -LiteralPath (Join-Path $publishPath 'WinDeploy.exe'))) {
    throw "The self-contained publish directory is missing: $publishPath"
}
if (-not (Test-Path -LiteralPath $iconPath)) { throw "The installer icon is missing: $iconPath" }
if (Test-Path -LiteralPath $outputPath) { throw "The installer output already exists: $outputPath" }

$bytes = (Get-ChildItem -LiteralPath $publishPath -Recurse -File | Measure-Object Length -Sum).Sum
$estimatedSizeKb = [Math]::Ceiling($bytes / 1KB)
$arguments = @(
    '/V2',
    '/INPUTCHARSET', 'UTF8',
    "/DAPP_VERSION=$Version",
    "/DAPP_SOURCE=$publishPath",
    "/DAPP_ICON=$iconPath",
    "/DOUTPUT_FILE=$outputPath",
    "/DAPP_SIZE_KB=$estimatedSizeKb",
    $scriptPath
)

& $compiler.FullName @arguments
if ($LASTEXITCODE -ne 0) { throw "NSIS compilation failed with exit code $LASTEXITCODE." }
if (-not (Test-Path -LiteralPath $outputPath)) { throw 'NSIS did not create the expected installer.' }

$item = Get-Item -LiteralPath $outputPath
Write-Host "Created $($item.FullName)"
Write-Host "Installer bytes: $($item.Length)"
Write-Host "Installed size estimate: $estimatedSizeKb KB"

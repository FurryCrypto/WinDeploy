param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    [ValidateSet('x64', 'ARM64')]
    [string]$Platform = 'x64'
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$runtimeIdentifier = "win-$($Platform.ToLowerInvariant())"

dotnet restore (Join-Path $root 'ESDInstaller.slnx')
dotnet build (Join-Path $root 'src\ESDInstaller.App\ESDInstaller.App.csproj') -c $Configuration -p:Platform=$Platform -r $runtimeIdentifier --no-restore
dotnet run --project (Join-Path $root 'tests\ESDInstaller.Core.Tests\ESDInstaller.Core.Tests.csproj') -c $Configuration --no-restore

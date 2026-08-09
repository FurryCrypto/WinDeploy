param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    [ValidateSet('x64', 'ARM64')]
    [string]$Platform = 'x64'
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot

dotnet restore (Join-Path $root 'WinDeploy.slnx')
dotnet build (Join-Path $root 'src\WinDeploy.App\WinDeploy.App.csproj') -c $Configuration -p:Platform=$Platform --no-restore
dotnet run --project (Join-Path $root 'tests\WinDeploy.Core.Tests\WinDeploy.Core.Tests.csproj') -c $Configuration --no-restore

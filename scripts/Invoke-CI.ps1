#Requires -Version 7.0
<#
.SYNOPSIS
Builds the complete Windows solution and runs all current automated test suites.
.DESCRIPTION
Uses the same commands locally and in GitHub PR validation. Logs and test working
files go to ResultsDirectory, outside the repository by default.
#>
[CmdletBinding()]
param(
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration = 'Release',
  [string]$ResultsDirectory = (Join-Path ([IO.Path]::GetTempPath()) ('Dockzi-CI-' + [guid]::NewGuid().ToString('N')))
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false

$repositoryPath = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$solutionPath = Join-Path $repositoryPath 'source/Dockzi.slnx'
$testAssembly = Join-Path $repositoryPath 'output/tests/Xeno.Dockzi.Tests.dll'
$resultsPath = [IO.Path]::GetFullPath($ResultsDirectory)

New-Item -ItemType Directory -Force -Path $resultsPath | Out-Null
Write-Host "Validation logs: $resultsPath"

Push-Location -LiteralPath $repositoryPath
try {
  & dotnet --info 2>&1 | Tee-Object -FilePath (Join-Path $resultsPath 'dotnet-info.log')
  if ($LASTEXITCODE -ne 0) { throw 'Unable to run the selected .NET SDK.' }

  & dotnet restore $solutionPath --disable-parallel -m:1 -warnaserror --verbosity minimal 2>&1 |
    Tee-Object -FilePath (Join-Path $resultsPath 'restore.log')
  if ($LASTEXITCODE -ne 0) { throw 'Package restore failed.' }

  & dotnet build $solutionPath --configuration $Configuration --no-restore --no-incremental `
    --disable-build-servers -m:1 -p:UseSharedCompilation=false -warnaserror --verbosity minimal 2>&1 |
    Tee-Object -FilePath (Join-Path $resultsPath 'build.log')

  if ($LASTEXITCODE -ne 0) { throw 'Solution build failed.' }

  & (Join-Path $PSScriptRoot 'Test-DockziVersion.ps1') *>&1 |
    Tee-Object -FilePath (Join-Path $resultsPath 'versioning-tests.log')

  & (Join-Path $PSScriptRoot 'Test-ReleaseAutomation.ps1') *>&1 |
    Tee-Object -FilePath (Join-Path $resultsPath 'release-automation-tests.log')
}
finally
{
  Pop-Location
}

# Execute the embedded Microsoft.Testing.Platform runner directly. This keeps app
# logs and test working files away from the checkout without depending on dotnet
# test's working-directory-based global.json runner selection.
Push-Location -LiteralPath $resultsPath
try
{
  $platformResults = Join-Path $resultsPath 'TestResults'

  & dotnet exec $testAssembly --results-directory $platformResults --minimum-expected-tests 1 `
    --timeout 5m --diagnostic --diagnostic-verbosity Information `
    --diagnostic-output-directory $platformResults 2>&1 |
    Tee-Object -FilePath (Join-Path $resultsPath 'automated-tests.log')

  if ($LASTEXITCODE -ne 0) {
    throw 'Automated tests failed. See automated-tests.log and TestResults.' }
}
finally
{
  Pop-Location
}

Write-Host "$Configuration validation passed."

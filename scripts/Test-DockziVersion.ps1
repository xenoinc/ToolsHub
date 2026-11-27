#Requires -Version 5.1
# Standalone regression tests; no Pester installation required.
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$caseCount = 0
$scriptPath = Join-Path $PSScriptRoot 'Increment-DockziVersion.ps1'

function Test-VersionIncrement {
  param([string]$Version, [string]$ExpectedVersion, [switch]$ShouldFail, [switch]$Duplicate, [string]$Component = 'Build', [string]$MinimumVersion = '')
  $script:caseCount++

  $temporaryFile = [IO.Path]::GetTempFileName()
  try
  {
    $versionElement = "<DockziVersion>$Version</DockziVersion>"
    $content = "<Project>`r`n  <!-- preserve formatting and metadata -->`r`n  <PropertyGroup>$versionElement</PropertyGroup>`r`n</Project>`r`n"

    if ($Duplicate) {
      $content = $content.Replace('</Project>', "$versionElement</Project>") }

    [IO.File]::WriteAllText($temporaryFile, $content, [Text.UTF8Encoding]::new($false))

    $failed = $false
    $result = $null

    try { $result = & $scriptPath -PropsPath $temporaryFile -Component $Component -MinimumVersion $MinimumVersion }
    catch { $failed = $true }

    if ($ShouldFail) {
      if (-not $failed) {
        throw "Expected rejection for $Version." }

      if ([IO.File]::ReadAllText($temporaryFile) -cne $content) {
        throw 'Rejected input was modified.' }
    }
    else
     {
      if ($failed -or $result -cne $ExpectedVersion) {
        throw "Incorrect increment for $Version : $result" }

      $expected = $content.Replace($versionElement, "<DockziVersion>$ExpectedVersion</DockziVersion>")

      if ([IO.File]::ReadAllText($temporaryFile) -cne $expected) {
        throw 'Unrelated props content was modified.' }

      $xml = [xml][IO.File]::ReadAllText($temporaryFile)
      if ($xml.Project.PropertyGroup.DockziVersion -cne $ExpectedVersion) {
        throw 'Invalid generated XML.' }
    }
  } finally {
    Remove-Item -LiteralPath $temporaryFile -Force
  }
}

Test-VersionIncrement '0.9.1.57' '0.9.1.58'
Test-VersionIncrement '2.3.4.0' '2.3.4.1'
Test-VersionIncrement '2.3.4.65533' '2.3.4.65534'
Test-VersionIncrement '2.3.4.65534' -ShouldFail
Test-VersionIncrement '65535.3.4.1' -ShouldFail
Test-VersionIncrement '2.3.4' -ShouldFail
Test-VersionIncrement '2.3.4.1-beta' -ShouldFail
Test-VersionIncrement '2.3.4.1' -ShouldFail -Duplicate
Test-VersionIncrement '1.2.3.4' '1.2.4.4' -Component Revision
Test-VersionIncrement '1.2.65534.4' -Component Revision -ShouldFail
Test-VersionIncrement '1.2.3.4' '1.3.0.4' -Component Minor
Test-VersionIncrement '0.9.1.57' '1.0.0.57' -Component Major
Test-VersionIncrement '1.2.3.4' '2.0.0.4' -Component Major
Test-VersionIncrement '65534.2.3.4' -Component Major -ShouldFail
Test-VersionIncrement '1.2.3.6' '1.2.4.7' -MinimumVersion '1.2.4.5'
Test-VersionIncrement '1.2.3.6' '1.2.5.6' -Component Revision -MinimumVersion '1.2.4.5'
Test-VersionIncrement '0.9.1.57' '1.0.0.70' -Component Major -MinimumVersion '0.9.3.70'
Test-VersionIncrement '1.2.3.4' -MinimumVersion 'invalid' -ShouldFail
Write-Host "All $caseCount core version increment regression cases passed."

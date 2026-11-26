#Requires -Version 5.1
# Standalone regression tests; no Pester installation required.
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$scriptPath = Join-Path $PSScriptRoot 'Increment-DockziVersion.ps1'

function Test-VersionIncrement {
  param([string]$Version, [string]$ExpectedVersion, [switch]$ShouldFail, [switch]$Duplicate)

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

    try { $result = & $scriptPath -PropsPath $temporaryFile }
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
Write-Host 'All 8 core version increment regression cases passed.'

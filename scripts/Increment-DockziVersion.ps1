#Requires -Version 5.1
<#
.SYNOPSIS
Increments a selected core version component, preserving the Build counter.
#>
[CmdletBinding()]
param(
  [string]$PropsPath = (Join-Path $PSScriptRoot '../source/Directory.Build.props'),
  [ValidateSet('Build', 'Revision', 'Minor', 'Major')]
  [string]$Component = 'Build',
  [string]$MinimumVersion = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$resolvedPath = (Resolve-Path -LiteralPath $PropsPath).ProviderPath
$content = [IO.File]::ReadAllText($resolvedPath)
$pattern = [regex]'<DockziVersion>([0-9]+)\.([0-9]+)\.([0-9]+)\.([0-9]+)</DockziVersion>'
$versions = $pattern.Matches($content)

if ($versions.Count -ne 1) {
  throw 'Expected exactly one DockziVersion in Major.Minor.Revision.Build format.'
}

$versionMatch = $versions[0]
foreach ($componentIndex in 1..4) {
  if ([long]$versionMatch.Groups[$componentIndex].Value -gt 65534) {
    throw 'Assembly version components must be between 0 and 65534.'
  }
}

# User format is Major.Minor.Revision.Build (the last two names differ from System.Version).
$parts = @([int]$versionMatch.Groups[1].Value, [int]$versionMatch.Groups[2].Value,
  [int]$versionMatch.Groups[3].Value, [int]$versionMatch.Groups[4].Value)
if ($MinimumVersion) {
  if ($MinimumVersion -notmatch '^\d+\.\d+\.\d+\.\d+$') { throw 'MinimumVersion must have four components.' }
  $minimum = [Version]$MinimumVersion
  foreach ($value in @($minimum.Major, $minimum.Minor, $minimum.Build, $minimum.Revision)) {
    if ($value -gt 65534) { throw 'MinimumVersion components cannot exceed 65534.' }
  }
  $prefix = [Version]::new($parts[0], $parts[1], $parts[2])
  $minimumPrefix = [Version]::new($minimum.Major, $minimum.Minor, $minimum.Build)
  if ($prefix -le $minimumPrefix) {
    $parts[0] = $minimum.Major
    $parts[1] = $minimum.Minor
    $parts[2] = $minimum.Build
    $parts[3] = [Math]::Max($parts[3], $minimum.Revision)
  }
}
$index = switch ($Component) { 'Major' { 0 } 'Minor' { 1 } 'Revision' { 2 } 'Build' { 3 } }
if ($parts[$index] -eq 65534) { throw "Dockzi $Component has reached 65534; select a higher release component." }
$parts[$index]++
# Major/minor releases reset subordinate release components, while retaining Build.
if ($Component -eq 'Major') { $parts[1] = 0; $parts[2] = 0 }
if ($Component -eq 'Minor') { $parts[2] = 0 }
$nextVersion = $parts -join '.'

$updated = $content.Substring(0, $versionMatch.Index) + "<DockziVersion>$nextVersion</DockziVersion>" +
  $content.Substring($versionMatch.Index + $versionMatch.Length)

[IO.File]::WriteAllText($resolvedPath, $updated, [Text.UTF8Encoding]::new($false))
Write-Output $nextVersion

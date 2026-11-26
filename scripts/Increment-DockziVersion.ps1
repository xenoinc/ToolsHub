#Requires -Version 5.1
<#
.SYNOPSIS
Increments only the fourth component of the core Dockzi executable version.
#>
[CmdletBinding()]
param(
  [string]$PropsPath = (Join-Path $PSScriptRoot '../source/Directory.Build.props')
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
foreach ($component in 1..4) {
  if ([long]$versionMatch.Groups[$component].Value -gt 65534) {
    throw 'Assembly version components must be between 0 and 65534.'
  }
}

$buildNumber = [int]$versionMatch.Groups[4].Value
if ($buildNumber -eq 65534) {
  throw 'Dockzi Build has reached 65534. Update Major, Minor, or Revision manually before incrementing Build.'
}

$nextVersion = '{0}.{1}.{2}.{3}' -f $versionMatch.Groups[1].Value,
  $versionMatch.Groups[2].Value, $versionMatch.Groups[3].Value, ($buildNumber + 1)

$updated = $content.Substring(0, $versionMatch.Index) + "<DockziVersion>$nextVersion</DockziVersion>" +
  $content.Substring($versionMatch.Index + $versionMatch.Length)

[IO.File]::WriteAllText($resolvedPath, $updated, [Text.UTF8Encoding]::new($false))
Write-Output $nextVersion

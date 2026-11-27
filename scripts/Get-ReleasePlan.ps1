#Requires -Version 7.0
[CmdletBinding()]
param(
  [Parameter(Mandatory)][string]$CoreVersion,
  [Parameter(Mandatory)][ValidatePattern('^[0-9a-f]{40}$')][string]$Commit,
  [Parameter(Mandatory)][ValidateSet('develop', 'master')][string]$BaseBranch
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if ($CoreVersion -notmatch '^\d+\.\d+\.\d+\.\d+$') {
  throw 'Expected a four-part DockziVersion.'
}

$version = [Version]$CoreVersion

foreach ($component in @($version.Major, $version.Minor, $version.Build, $version.Revision)) {
  if ($component -gt 65534) {
    throw 'Version components cannot exceed 65534.' }
}

$preview = $BaseBranch -eq 'develop'
$suffix = if ($preview) { '-preview' } else { '' }
$packageVersion = '{0}.{1}.{2}-build.{3}' -f $version.Major, $version.Minor, $version.Build, $version.Revision

if (-not $preview) {
  $packageVersion += '.stable' }

[pscustomobject]@{
  CoreVersion = $version.ToString()
  PackageVersion = $packageVersion
  Tag = 'v' + $version.ToString() + $suffix
  Commit = $Commit
  BaseBranch = $BaseBranch
  Prerelease = $preview
}

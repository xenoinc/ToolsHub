#Requires -Version 7.0
[CmdletBinding()]
param(
  [Parameter(Mandatory)][ValidatePattern('^[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+$')][string]$Repository,
  [Parameter(Mandatory)][ValidateSet('develop', 'master')][string]$BaseBranch,
  [Parameter(Mandatory)][ValidatePattern('^[0-9a-f]{40}$')][string]$Commit,
  [Parameter(Mandatory)][string]$AssetsDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$headCommit = git rev-parse HEAD

if ($LASTEXITCODE -ne 0 -or $headCommit -ne $Commit) {
  throw 'The checkout must match the tested release commit.' }

[xml]$properties = [IO.File]::ReadAllText((Resolve-Path -LiteralPath 'source/Directory.Build.props').ProviderPath)

$plan = & (Join-Path $PSScriptRoot 'Get-ReleasePlan.ps1') -CoreVersion $properties.SelectSingleNode('/Project/PropertyGroup/DockziVersion').InnerText -Commit $Commit -BaseBranch $BaseBranch
$root = (Resolve-Path -LiteralPath $AssetsDirectory).ProviderPath
$required = @('Dockzi-win-Setup.exe', "Dockzi-$($plan.PackageVersion)-full.nupkg", 'releases.win.json', 'checksums-win-x64.json')

foreach ($name in $required) {
  if (-not (Test-Path -LiteralPath (Join-Path $root $name) -PathType Leaf)) {
    throw "Missing release asset: $name" }
}

$manifest = Get-Content -LiteralPath (Join-Path $root 'checksums-win-x64.json') -Raw | ConvertFrom-Json
if ($manifest.Version -ne $plan.PackageVersion) { throw 'Package version does not match the release commit.' }

foreach ($asset in $manifest.Files)
{
  if ([IO.Path]::GetFileName($asset.File) -ne $asset.File) {
    throw 'Invalid asset filename in checksum manifest.'
  }

  $path = Join-Path $root $asset.File
  if (-not (Test-Path -LiteralPath $path -PathType Leaf) -or (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash -ne $asset.SHA256) {
    throw "Asset checksum verification failed: $($asset.File)"
  }
}
$files = @(Get-ChildItem -LiteralPath $root -File | Where-Object {
  $_.Extension -in @('.exe', '.nupkg', '.json') -or $_.Name -like 'RELEASES*'
})

$tag = $plan.Tag
$remoteTags = @(git ls-remote --tags origin "refs/tags/$tag" "refs/tags/$tag^{}")
if ($LASTEXITCODE -ne 0) {
  throw 'Unable to inspect the remote release tag.' }

if ($remoteTags.Count -gt 0)
{
  $peeled = @($remoteTags | Where-Object { $_.EndsWith('^{}') })
  $target = if ($peeled.Count -gt 0) { ($peeled[0] -split '\s+')[0] } else { ($remoteTags[0] -split '\s+')[0] }

  if ($target -ne $Commit) {
    throw "Tag $tag already points to another commit. Advance DockziVersion before releasing again." }
} else {
  git config user.name 'github-actions[bot]'

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to configure the tag author.' }

  git config user.email '41898282+github-actions[bot]@users.noreply.github.com'

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to configure the tag email.' }

  git tag -a $tag $Commit -m "Dockzi $($plan.CoreVersion)"

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to create the release tag.' }

  git push origin "refs/tags/$tag"

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to push the release tag.'
  }
}

$json = gh release view $tag --repo $Repository --json isDraft,isPrerelease,assets 2>$null
if ($LASTEXITCODE -eq 0) {
  $release = $json | ConvertFrom-Json

  if ($release.isPrerelease -ne $plan.Prerelease) {
    throw 'Existing release has a different prerelease classification.' }

  if (-not $release.isDraft) {
    foreach ($file in $files) {
      if ($file.Name -notin $release.assets.name) {
        throw 'Published release is missing assets; refusing to modify a published release.' }
    }

    Write-Host "Release $tag is already published."
    return
  }
} else {
  $arguments = @('release', 'create', $tag, '--repo', $Repository, '--verify-tag', '--draft', '--generate-notes', '--title', "Dockzi $($plan.CoreVersion)", '--latest=false')
  
  if ($plan.Prerelease) {
    $arguments += '--prerelease' }

  & gh @arguments
  
  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to create the release draft.' }
}

# Upload only into a draft, so update clients cannot see an incomplete feed.
$arguments = @('release', 'upload', $tag, '--repo', $Repository, '--clobber') + @($files.FullName)

& gh @arguments

if ($LASTEXITCODE -ne 0) {
  throw 'Unable to upload release assets. Rerun to resume the draft.' }

$preview = $plan.Prerelease.ToString().ToLowerInvariant()
$latest = (-not $plan.Prerelease).ToString().ToLowerInvariant()

gh release edit $tag --repo $Repository --draft=false "--prerelease=$preview" "--latest=$latest"

if ($LASTEXITCODE -ne 0) {
  throw 'Unable to publish the completed release draft.' }

Write-Host "Published $tag."

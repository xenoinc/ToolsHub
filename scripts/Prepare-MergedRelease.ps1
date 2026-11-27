#Requires -Version 7.0
<#
.SYNOPSIS
Prepares a pinned release commit for a merged PR. Intended for a disposable CI checkout.
#>
[CmdletBinding()]
param(
  [Parameter(Mandatory)][ValidateSet('develop', 'master')][string]$BaseBranch,
  [Parameter(Mandatory)][ValidatePattern('^[0-9a-f]{40}$')][string]$MergeCommit,
  [Parameter(Mandatory)][ValidateRange(1, [int]::MaxValue)][int]$PullRequestNumber,
  [Parameter(Mandatory)][string]$OutputPath,
  [switch]$AllowLocalRepository
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
if ($env:GITHUB_ACTIONS -ne 'true' -and -not $AllowLocalRepository) {
  throw 'Run this helper in GitHub Actions, or explicitly opt into a disposable local repository.'
}

$dirty = git status --porcelain
if ($LASTEXITCODE -ne 0 -or $dirty) { throw 'A clean Git checkout is required.' }

$marker = "Dockzi-Version-Bump: PR #$PullRequestNumber ($MergeCommit)"
$commit = $null

for ($attempt = 1; $attempt -le 10; $attempt++)
{
  git fetch origin $BaseBranch
  if ($LASTEXITCODE -ne 0) { throw 'Unable to fetch the target branch.' }

  git merge-base --is-ancestor $MergeCommit "origin/$BaseBranch"

  if ($LASTEXITCODE -ne 0) { throw 'The PR merge commit is not on the target branch.' }

  if ($BaseBranch -eq 'master') {
    $commit = $MergeCommit
    break
  }

  $existing = git log "origin/$BaseBranch" --format=%H -1 --fixed-strings --grep="$marker"
  if ($LASTEXITCODE -ne 0) { throw 'Unable to inspect version history.' }
  if ($existing) {
    $commit = $existing.Trim()
    break
  }

  # Only the disposable, clean Actions checkout is reset; never force-push the branch.
  git reset --hard "origin/$BaseBranch"

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to reset the Actions checkout.' }

  git config user.name 'github-actions[bot]'

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to configure the commit author.' }

  git config user.email '41898282+github-actions[bot]@users.noreply.github.com'

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to configure the commit email.' }

  & (Join-Path $PSScriptRoot 'Increment-DockziVersion.ps1') -PropsPath './source/Directory.Build.props'

  git add -- source/Directory.Build.props

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to stage the version increment.' }

  git commit -m "chore: bump Dockzi build after PR #$PullRequestNumber" -m $marker

  if ($LASTEXITCODE -ne 0) {
    throw 'Unable to commit the version increment.' }

  git push origin "HEAD:$BaseBranch"

  if ($LASTEXITCODE -eq 0) {
    $commit = git rev-parse HEAD
    break
  }

  Start-Sleep -Seconds 2
}

if (-not $commit) {
  throw 'Unable to push the version bump. Check branch permissions, then rerun.' }

$content = git show "${commit}:source/Directory.Build.props"

if ($LASTEXITCODE -ne 0) {
  throw 'Unable to read version properties at the release commit.' }

[xml]$properties = $content -join "`n"
$versionNodes = $properties.SelectNodes('/Project/PropertyGroup/DockziVersion')

if ($versionNodes.Count -ne 1) {
  throw 'Expected exactly one DockziVersion.' }

$plan = & (Join-Path $PSScriptRoot 'Get-ReleasePlan.ps1') -CoreVersion $versionNodes[0].InnerText -Commit $commit -BaseBranch $BaseBranch
$plan | ConvertTo-Json | Set-Content -LiteralPath $OutputPath -Encoding utf8

Write-Host "Prepared $($plan.Tag) at $commit"

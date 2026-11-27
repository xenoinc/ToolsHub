#Requires -Version 7.0
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$checks = 0

function Assert-Release([bool]$Condition, [string]$Message) {
  if (-not $Condition) {
    throw $Message }

  $script:checks++
}

function Expect-Failure([scriptblock]$Action)
{
  $failed = $false
  try
  {
    & $Action | Out-Null
  }
  catch
  {
    Write-Host "Expected failure: $($_.Exception.Message)"
    $failed = $true
  }

  Assert-Release $failed 'Expected the operation to fail.'
}

function Invoke-FixtureGit {
  & git @args *> $null
  if ($LASTEXITCODE -ne 0) { throw "Fixture Git command failed: $args" }
}

function New-FixtureAssets([string]$Directory, [string]$Version)
{
  New-Item -ItemType Directory -Path $Directory | Out-Null

  foreach ($name in @('Dockzi-win-Setup.exe', "Dockzi-$Version-full.nupkg", 'releases.win.json')) {
    Set-Content -LiteralPath (Join-Path $Directory $name) -Value 'Fixture asset'
  }

  $manifest = @{ Version = $Version; Files = @(Get-ChildItem -LiteralPath $Directory -File | ForEach-Object {
    @{ File = $_.Name; SHA256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
  }) }

  $manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $Directory 'checksums-win-x64.json')
}

$planScript = Join-Path $PSScriptRoot 'Get-ReleasePlan.ps1'
$prepareScript = Join-Path $PSScriptRoot 'Prepare-MergedRelease.ps1'
$publishScript = Join-Path $PSScriptRoot 'Publish-GitHubRelease.ps1'
$sha = 'a' * 40
$preview = & $planScript -CoreVersion '1.2.3.4' -Commit $sha -BaseBranch develop
$stable = & $planScript -CoreVersion '1.2.3.4' -Commit $sha -BaseBranch master

Assert-Release ($preview.Tag -eq 'v1.2.3.4-preview' -and $preview.Prerelease) 'Incorrect preview tag.'
Assert-Release ($stable.Tag -eq 'v1.2.3.4' -and -not $stable.Prerelease) 'Incorrect stable tag.'
Assert-Release ($preview.PackageVersion -eq '1.2.3-build.4' -and $stable.PackageVersion -eq '1.2.3-build.4.stable') 'Incorrect preview/stable package mapping.'

Expect-Failure { & $planScript -CoreVersion '1.2.3' -Commit $sha -BaseBranch develop }
Expect-Failure { & $planScript -CoreVersion '1.2.3.65535' -Commit $sha -BaseBranch develop }

$temporaryParent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd([IO.Path]::DirectorySeparatorChar)
$root = Join-Path $temporaryParent ('DockziReleaseTests-' + [guid]::NewGuid().ToString('N'))

New-Item -ItemType Directory -Path $root | Out-Null

$originalGh = Get-Item Function:gh -ErrorAction SilentlyContinue
$global:DockziReleaseTestState = @{ Releases = @{}; Actions = [Collections.Generic.List[string]]::new(); FailNextUpload = $false }

# Never use real GitHub credentials or services in these regressions.
function global:gh
{
  $arguments = @($args)
  $action = $arguments[1]
  $tag = $arguments[2]
  $state = $global:DockziReleaseTestState
  $state.Actions.Add($action)
  $global:LASTEXITCODE = 0

  switch ($action) {
    'view' {
      if (-not $state.Releases.ContainsKey($tag)) { $global:LASTEXITCODE = 1; return }
      $state.Releases[$tag] | ConvertTo-Json -Depth 6
    }
    'create' {
      $state.Releases[$tag] = [pscustomobject]@{ isDraft = $true; isPrerelease = '--prerelease' -in $arguments; assets = @(); latest = $false }
    }
    'upload' {
      if ($state.FailNextUpload) { $state.FailNextUpload = $false; $global:LASTEXITCODE = 1; return }
      $state.Releases[$tag].assets = @($arguments | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf } | ForEach-Object {
        [pscustomobject]@{ name = [IO.Path]::GetFileName($_) }
      })
    }
    'edit' {
      $state.Releases[$tag].isDraft = $false
      $state.Releases[$tag].isPrerelease = '--prerelease=true' -in $arguments
      $state.Releases[$tag].latest = '--latest=true' -in $arguments
    }
    default { throw "Unexpected GitHub operation in test: $action" }
  }
}

Push-Location -LiteralPath $root

try
{
  Invoke-FixtureGit init --bare --initial-branch=develop (Join-Path $root 'origin.git')
  Invoke-FixtureGit clone (Join-Path $root 'origin.git') (Join-Path $root 'checkout')

  Set-Location -LiteralPath (Join-Path $root 'checkout')
  Invoke-FixtureGit config user.name 'Release regression'
  Invoke-FixtureGit config user.email 'release@example.invalid'
  Invoke-FixtureGit config commit.gpgsign false
  Invoke-FixtureGit config tag.gpgsign false

  New-Item -ItemType Directory -Path 'source' | Out-Null
  [IO.File]::WriteAllText((Join-Path (Get-Location) 'source/Directory.Build.props'), '<Project><PropertyGroup><DockziVersion>1.2.3.4</DockziVersion></PropertyGroup></Project>')

  Invoke-FixtureGit add .
  Invoke-FixtureGit commit -m 'Merged fixture PR'
  Invoke-FixtureGit push origin develop

  $merge = git rev-parse HEAD
  $output = Join-Path $root 'plan.json'

  & $prepareScript -BaseBranch develop -MergeCommit $merge -PullRequestNumber 101 -OutputPath $output -AllowLocalRepository | Out-Null
  
  $first = Get-Content -LiteralPath $output -Raw | ConvertFrom-Json
  Assert-Release ($first.CoreVersion -eq '1.2.3.5') 'Develop did not increment only Build.'

  Invoke-FixtureGit commit --allow-empty -m 'Another merged PR'
  Invoke-FixtureGit push origin develop

  $secondMerge = git rev-parse HEAD

  & $prepareScript -BaseBranch develop -MergeCommit $secondMerge -PullRequestNumber 102 -OutputPath $output -AllowLocalRepository | Out-Null

  $second = Get-Content -LiteralPath $output -Raw | ConvertFrom-Json

  Assert-Release ($second.CoreVersion -eq '1.2.3.6') 'A second PR must increment again.'

  & $prepareScript -BaseBranch develop -MergeCommit $merge -PullRequestNumber 101 -OutputPath $output -AllowLocalRepository | Out-Null
  $rerun = Get-Content -LiteralPath $output -Raw | ConvertFrom-Json

  Assert-Release ($rerun.Commit -eq $first.Commit -and $rerun.CoreVersion -eq $first.CoreVersion) 'Rerunning an older PR must retain its original commit/version.'

  Invoke-FixtureGit checkout -b master $first.Commit
  Invoke-FixtureGit commit --allow-empty -m 'Stable merge'
  Invoke-FixtureGit push origin master

  $masterMerge = git rev-parse HEAD
  
  & $prepareScript -BaseBranch master -MergeCommit $masterMerge -PullRequestNumber 103 -OutputPath $output -AllowLocalRepository | Out-Null

  $master = Get-Content -LiteralPath $output -Raw | ConvertFrom-Json
  Assert-Release ($master.CoreVersion -eq $first.CoreVersion -and $master.Commit -eq $masterMerge) 'Master must use the exact merge commit without incrementing.'

  $assets = Join-Path $root 'assets'
  New-FixtureAssets $assets $first.PackageVersion
  
  $stableAssets = Join-Path $root 'stable-assets'
  New-FixtureAssets $stableAssets $master.PackageVersion

  Invoke-FixtureGit checkout --detach $first.Commit
  $global:DockziReleaseTestState.FailNextUpload = $true

  Expect-Failure { & $publishScript -Repository 'example/Dockzi' -BaseBranch develop -Commit $first.Commit -AssetsDirectory $assets }
  Assert-Release $global:DockziReleaseTestState.Releases[$first.Tag].isDraft 'A failed upload must remain a draft.'

  & $publishScript -Repository 'example/Dockzi' -BaseBranch develop -Commit $first.Commit -AssetsDirectory $assets
  
  $release = $global:DockziReleaseTestState.Releases[$first.Tag]

  Assert-Release (-not $release.isDraft -and $release.isPrerelease -and -not $release.latest) 'Preview release classification is wrong.'
  Assert-Release ($release.assets.Count -eq 4) 'Release assets were not uploaded.'
  
  $uploadCount = @($global:DockziReleaseTestState.Actions | Where-Object { $_ -eq 'upload' }).Count
  & $publishScript -Repository 'example/Dockzi' -BaseBranch develop -Commit $first.Commit -AssetsDirectory $assets
  
  Assert-Release (@($global:DockziReleaseTestState.Actions | Where-Object { $_ -eq 'upload' }).Count -eq $uploadCount) 'Published releases must not be overwritten on retry.'

  Invoke-FixtureGit checkout --detach $master.Commit
  & $publishScript -Repository 'example/Dockzi' -BaseBranch master -Commit $master.Commit -AssetsDirectory $stableAssets
  $stableRelease = $global:DockziReleaseTestState.Releases[$master.Tag]

  Assert-Release (-not $stableRelease.isDraft -and -not $stableRelease.isPrerelease -and $stableRelease.latest) 'Stable release was not published as latest.'
  Invoke-FixtureGit commit --allow-empty -m 'Conflicting stable version'
  $conflict = git rev-parse HEAD
  Expect-Failure { & $publishScript -Repository 'example/Dockzi' -BaseBranch master -Commit $conflict -AssetsDirectory $stableAssets }

  Invoke-FixtureGit checkout --detach $first.Commit
  Set-Content -LiteralPath (Join-Path $assets 'Dockzi-win-Setup.exe') -Value 'Corrupted asset'
  Expect-Failure { & $publishScript -Repository 'example/Dockzi' -BaseBranch develop -Commit $first.Commit -AssetsDirectory $assets }

  Assert-Release (@($global:DockziReleaseTestState.Actions | Where-Object { $_ -eq 'create' }).Count -eq 2) 'Expected exactly one stable and one preview release.'
}
finally
{
  Pop-Location
  Remove-Item Function:gh

  if ($originalGh) {
    Set-Item Function:global:gh -Value $originalGh.ScriptBlock
  }

  Remove-Variable DockziReleaseTestState -Scope Global
  # Verify this uniquely created fixture stays directly under the temporary directory.
  $resolvedRoot = [IO.Path]::GetFullPath($root)

  if ([IO.Path]::GetDirectoryName($resolvedRoot) -eq $temporaryParent -and [IO.Path]::GetFileName($resolvedRoot).StartsWith('DockziReleaseTests-')) {
    Remove-Item -LiteralPath $resolvedRoot -Recurse -Force
  } else {
    throw 'Refusing cleanup outside the release-test fixture.'
  }
}
Write-Host "All $checks release automation regression checks passed. No GitHub services were contacted."

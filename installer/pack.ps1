#Requires -Version 5.1
<#
.SYNOPSIS
Publishes a self-contained Dockzi application and packages it with Velopack.
.DESCRIPTION
Normally invoked by build.ps1. Outputs Setup.exe, update packages, and a feed to publish.
Uses isolated staging; never packages the developer's output directory or user data.
#>
[CmdletBinding()]
param(
  [ValidatePattern('^\d+\.\d+\.\d+(-[0-9A-Za-z.-]+)?$')]
  [string]$Version = '0.6.0',
  [ValidateSet('win-x64', 'win-arm64')]
  [string]$Runtime = 'win-x64',
  [string]$UpdateUrl = '',
  [ValidatePattern('^[a-zA-Z0-9_-]+$')]
  [string]$Channel = 'win',
  [string]$SignTemplate = '',
  [switch]$SkipRestore
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$repositoryPath = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$publishPath = Join-Path $repositoryPath 'publish'
$stagingPath = [IO.Path]::GetFullPath((Join-Path $publishPath ".staging\$Runtime"))
$appPath = Join-Path $stagingPath 'app'
$dotnetPath = (Get-Command dotnet -CommandType Application -ErrorAction Stop).Source

function Assert-RegularDirectoryTree([string]$Path) {
  if (Test-Path -LiteralPath $Path) {
    $item = Get-Item -LiteralPath $Path -Force
    if (-not $item.PSIsContainer -or ($item.Attributes -band [IO.FileAttributes]::ReparsePoint)) {
      throw "Expected a regular directory: $Path"
    }
    $link = Get-ChildItem -LiteralPath $Path -Force -Recurse |
      Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint } | Select-Object -First 1
    if ($link) { throw "Refusing to package or clean linked content: $($link.FullName)" }
  }
}

function Publish-Project([string]$Project, [string]$Destination, [bool]$SelfContained) {
  $arguments = @('publish', (Join-Path $repositoryPath $Project), '--configuration', 'Release',
    '--runtime', $Runtime, '--self-contained', $SelfContained.ToString().ToLowerInvariant(),
    '--output', $Destination, '--disable-build-servers', '-m:1', '-p:UseSharedCompilation=false',
    '-p:PublishSingleFile=false', '-p:PublishTrimmed=false', '--verbosity', 'minimal')
  if ($SkipRestore) { $arguments += '--no-restore' }
  & $dotnetPath @arguments
  if ($LASTEXITCODE -ne 0) { throw "Publishing failed: $Project" }
}

Push-Location -LiteralPath $repositoryPath
try {
  if ($UpdateUrl) {
    $feedUri = $null
    if (-not [Uri]::TryCreate($UpdateUrl, [UriKind]::Absolute, [ref]$feedUri) -or $feedUri.Scheme -ne 'https') {
      throw 'UpdateUrl must be an absolute HTTPS URL for a Velopack release feed directory.'
    }
  }

  # Check the resolved cleanup target and every existing ancestor before recursive removal.
  if (-not $stagingPath.StartsWith([IO.Path]::GetFullPath($publishPath) + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Unexpected staging path: $stagingPath"
  }
  Assert-RegularDirectoryTree $publishPath
  if (Test-Path -LiteralPath $stagingPath) { Remove-Item -LiteralPath $stagingPath -Recurse -Force }
  New-Item -ItemType Directory -Path $appPath -Force | Out-Null
  if (-not $SkipRestore) {
    & $dotnetPath tool restore
    if ($LASTEXITCODE -ne 0) { throw 'Could not restore the pinned Velopack tool.' }
  }

  Publish-Project 'source\Xeno.Dockzi\Xeno.Dockzi.csproj' $appPath $true
  $bundles = @(
    @{ Name = 'PomodoroAddin'; Project = 'source\addins\PomodoroAddin\PomodoroAddin.csproj' },
    @{ Name = 'SidebarAddin'; Project = 'source\addins\SidebarAddin\SidebarAddin.csproj' },
    @{ Name = 'NotesAddin'; Project = 'source\addins\NotesAddin\NotesAddin.csproj' },
    @{ Name = 'VeraCryptAddin'; Project = 'source\Xeno.Dockzi.VeraCryptAddin\VeraCryptAddin.csproj' }
  )
  foreach ($bundle in $bundles) {
    $bundlePath = Join-Path $stagingPath $bundle.Name
    Publish-Project $bundle.Project $bundlePath $false
    # Copy only managed code and bundled manifests, keeping the self-contained host runtime intact.
    foreach ($file in Get-ChildItem -LiteralPath $bundlePath -File) {
      if ($file.Extension -ne '.dll' -and $file.Name -ne "$($bundle.Name).deps.json") { continue }
      if ($file.Name -eq 'Dockzi.dll') { continue }
      $destination = Join-Path $appPath $file.Name
      if (Test-Path -LiteralPath $destination) {
        if ((Get-FileHash -LiteralPath $destination).Hash -ne (Get-FileHash -LiteralPath $file.FullName).Hash) {
          throw "Conflicting bundled dependency: $($file.Name) from $($bundle.Name)"
        }
      } else {
        Copy-Item -LiteralPath $file.FullName -Destination $destination
      }
    }
    if (-not (Test-Path -LiteralPath (Join-Path $appPath "$($bundle.Name).dll"))) {
      throw "Missing bundled add-in: $($bundle.Name)"
    }
  }

  @{ UpdateUrl = $UpdateUrl; Channel = $Channel } | ConvertTo-Json |
    Set-Content -LiteralPath (Join-Path $appPath 'Dockzi.install.json') -Encoding UTF8

  foreach ($requiredFile in @('Dockzi.exe', 'Dockzi.dll', 'Velopack.dll', 'hostfxr.dll',
    'PresentationFramework.dll', 'PresentationCore.dll', 'WindowsBase.dll', 'Markdig.dll',
    'Mono.Addins.dll', 'Mono.Addins.CecilReflector.dll')) {
    if (-not (Test-Path -LiteralPath (Join-Path $appPath $requiredFile) -PathType Leaf)) {
      throw "The self-contained package is missing a required runtime dependency: $requiredFile"
    }
  }

  $forbidden = Get-ChildItem -LiteralPath $appPath -Recurse -File | Where-Object {
    $_.Name -in @('Dockzi.config', 'Dockzi.db', 'ToolsHub.db', 'SidebarLaunchers.json', 'Dockzi.portable') -or
    $_.Extension -in @('.log', '.bak') -or $_.Name -match '^(Sample|Xeno.Dockzi.Tests)'
  }
  if ($forbidden) { throw "Unexpected development/user files in package: $($forbidden.Name -join ', ')" }

  $packArguments = @('tool', 'run', 'vpk', '--', 'pack', '--packId', 'Dockzi', '--packVersion', $Version,
    '--packDir', $appPath, '--mainExe', 'Dockzi.exe', '--packTitle', 'Dockzi',
    '--packAuthors', 'Xeno Innovations, Inc.', '--runtime', $Runtime, '--channel', $Channel,
    '--icon', (Join-Path $repositoryPath 'source\Xeno.Dockzi\Resources\AppIcon.ico'),
    '--outputDir', $publishPath, '--noPortable', '--shortcuts', 'StartMenuRoot', '--skip-updates', '--yes')
  if ($SignTemplate) { $packArguments += @('--signTemplate', $SignTemplate) }
  & $dotnetPath @packArguments
  if ($LASTEXITCODE -ne 0) { throw 'Velopack packaging failed.' }

  # Inspect the actual release archive, including dependencies loaded dynamically by add-ins.
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  $packagePath = Join-Path $publishPath "Dockzi-$Version-full.nupkg"
  $archive = [IO.Compression.ZipFile]::OpenRead($packagePath)
  try {
    foreach ($requiredFile in @('Dockzi.exe', 'Dockzi.dll', 'Velopack.dll', 'hostfxr.dll', 'Dockzi.install.json',
      'PresentationFramework.dll', 'PresentationCore.dll', 'WindowsBase.dll', 'Markdig.dll',
      'PomodoroAddin.dll', 'SidebarAddin.dll', 'NotesAddin.dll', 'VeraCryptAddin.dll')) {
      if ($null -eq $archive.GetEntry("lib/app/$requiredFile")) { throw "Release archive is missing $requiredFile" }
    }
    if (@($archive.Entries | Where-Object { $_.FullName -match '(^|/)(Notes/|AddinRegistry/|Dockzi\.config$|SidebarLaunchers\.json$|Sample[^/]*\.dll$|Xeno\.Dockzi\.Tests)' }).Count -gt 0) {
      throw 'Release archive contains development or user data.'
    }
  } finally {
    $archive.Dispose()
  }

  $artifacts = @(Get-ChildItem -LiteralPath $publishPath -File | Where-Object {
    ($_.Extension -in @('.exe', '.nupkg', '.json') -or $_.Name -eq 'RELEASES') -and $_.Name -notlike 'checksums-*'
  } | ForEach-Object {
    @{ File = $_.Name; Size = $_.Length; SHA256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
  })
  @{ Version = $Version; Runtime = $Runtime; Channel = $Channel; Files = $artifacts } | ConvertTo-Json -Depth 5 |
    Set-Content -LiteralPath (Join-Path $publishPath "checksums-$Runtime.json") -Encoding UTF8
  Write-Host "Installer and update feed generated in $publishPath"
}
catch {
  Write-Error $_ -ErrorAction Continue
  exit 1
}
finally {
  Pop-Location
}

exit 0

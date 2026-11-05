#Requires -Version 5.1
<#
.SYNOPSIS
Clears generated output and rebuilds the ToolsHub solution in Release.
.DESCRIPTION
Preserves ToolsHub.db, the legacy SidebarLaunchers.json, logs, backups, and readme.txt.
Requires the .NET 10 SDK or newer. Close ToolsHub before building.
.PARAMETER SkipRestore
Uses the packages already restored on this machine.
#>
[CmdletBinding()]
param(
  [switch]$SkipRestore
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repositoryPath = [IO.Path]::GetFullPath($PSScriptRoot)
$solutionPath = Join-Path $repositoryPath 'source\ToolsHub.slnx'
$outputPath = [IO.Path]::GetFullPath((Join-Path $repositoryPath 'output'))

Push-Location -LiteralPath $repositoryPath
try
{
  if (-not (Test-Path -LiteralPath $solutionPath -PathType Leaf)) {
    throw "Solution not found: $solutionPath"
  }

  $dotnetPath = (Get-Command dotnet -CommandType Application -ErrorAction Stop).Source
  $sdkVersion = & $dotnetPath --version
  if ($LASTEXITCODE -ne 0 -or $sdkVersion -notmatch '^(\d+)\.' -or [int]$Matches[1] -lt 10) {
    throw 'Install the .NET 10 SDK or newer before building ToolsHub.'
  }

  if (-not $SkipRestore)
  {
    Write-Host 'Restoring packages...'
    & $dotnetPath restore $solutionPath --disable-parallel -m:1 --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
      exit $LASTEXITCODE
    }
  }

  # Verify the absolute cleanup target before any recursive deletion.
  if (-not [string]::Equals([IO.Path]::GetDirectoryName($outputPath), $repositoryPath, [StringComparison]::OrdinalIgnoreCase) -or
      [IO.Path]::GetFileName($outputPath) -ne 'output') {
    throw "Refusing to clean an unexpected output path: $outputPath"
  }

  if (Test-Path -LiteralPath $outputPath)
  {
    $outputDirectory = Get-Item -LiteralPath $outputPath -Force
    if (-not $outputDirectory.PSIsContainer -or ($outputDirectory.Attributes -band [IO.FileAttributes]::ReparsePoint)) {
      throw 'The output path must be a regular directory, not a symbolic link or junction.'
    }

    $linkedItem = Get-ChildItem -LiteralPath $outputPath -Force -Recurse |
      Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint } |
      Select-Object -First 1

    if ($linkedItem) {
      throw "Refusing recursive cleanup with a symbolic link or junction in output: $($linkedItem.FullName)"
    }

    Write-Host "Cleaning generated files in $outputPath..."
    foreach ($item in Get-ChildItem -LiteralPath $outputPath -Force) {
      if (-not $item.PSIsContainer -and
          ($item.Name -in @('ToolsHub.db', 'SidebarLaunchers.json', 'readme.txt') -or $item.Extension -in @('.log', '.bak'))) {
        continue
      }

      $targetPath = [IO.Path]::GetFullPath($item.FullName)
      if (-not $targetPath.StartsWith($outputPath + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to remove a path outside output: $targetPath"
      }

      try
      {
        Remove-Item -LiteralPath $targetPath -Recurse -Force
      }
      catch
      {
        throw "Could not clean '$targetPath'. Close ToolsHub and any active debugging/test sessions, then retry. $($_.Exception.Message)"
      }
    }
  }

  Write-Host 'Rebuilding all projects in Release...'
  # Shared output requires serial builds to avoid host/add-in copy races.
  & $dotnetPath build $solutionPath --configuration Release --no-restore --disable-build-servers -m:1 -t:Rebuild -p:UseSharedCompilation=false --verbosity minimal
  if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
  }

  Write-Host "Release build completed: $outputPath"
}
catch {
  Write-Error $_ -ErrorAction Continue
  exit 1
}
finally {
  Pop-Location
}

exit 0

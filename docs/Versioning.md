# Core version and automated releases

Edit `DockziVersion` in `source/Directory.Build.props`. The core project `source/Xeno.Dockzi/Xeno.Dockzi.csproj` produces `Dockzi.exe`; its assembly, file, and informational versions use **Major.Minor.Revision.Build**. Add-ins retain their own versions.

## Merged pull requests

`.github/workflows/increment-core-version.yml` implements the complete merged-PR pipeline:

| PR target | Version action | Tag example | GitHub release |
| --- | --- | --- | --- |
| `develop` | Increment only Build, e.g. `0.9.1.57` to `0.9.1.58` | `v0.9.1.58-preview` | Prerelease; never marked Latest |
| `master` | Keep the current core version at the PR merge commit | `v0.9.1.58` | Stable; marked Latest |

Only merged PRs trigger releases; closing an unmerged PR or directly pushing a branch does not. Merge, squash, and rebase PR merges are supported. Master is intended to promote a newly versioned develop build. Attempting another master release with the same version but a different commit fails rather than moving the existing tag.

1. Prepare the release commit. Develop writes the increment back to the branch. A PR/merge-SHA marker makes retries reuse the original version and commit, even after later merges. Master pins the exact PR merge commit.
2. Check out that commit on Windows, run all automated Release validation, and build the installer/update packages with the existing packaging script.
3. Verify asset checksums, create an annotated tag at the tested commit, upload assets into a draft, then publish the complete GitHub release. Failed uploads can resume on rerun. Published releases and tags are never overwritten.

Every release contains Setup.exe, the full `.nupkg`, channel feed JSON, generated asset metadata, and checksums. Releases run in the same workflow as version preparation because GitHub token-created commits/tags do not trigger ordinary push workflows. Per-branch [`queue: max` concurrency](https://docs.github.com/en/actions/reference/workflows-and-actions/workflow-syntax#example-queueing-multiple-pending-runs) preserves queued merge events; optimistic push retries handle intervening branch changes.

The workflow checks out merged base-branch code, never an unmerged PR head. Build jobs have read-only repository permissions. Version preparation and publication use the built-in `GITHUB_TOKEN` with `contents: write`. Enable Actions and ensure branch/tag rules permit the bot's version commit and release tags. Install the workflow and helper scripts on both `develop` and `master`. No repository policies are changed by these files.

## Update package versions

The CLI requires three-part SemVer2, so core `0.9.1.58` maps to package `0.9.1-build.58`. Develop uses that package version; master uses `0.9.1-build.58.stable`. The stable package sorts above its matching preview so preview users can receive the final stable build, while the next preview `0.9.1-build.59` sorts above both. GitHub's prerelease flag controls who receives each release, independently of the SemVer label. The EXE version stays `0.9.1.58` on master.

The existing `build.ps1 -Version` override remains available for deliberate manual releases. A plain stable SemVer such as `0.9.1` sorts above `0.9.1-build.N`; advance the core Revision before returning to automatic build packages after such an override. Add-in version increments remain independent future work.

## User preview preference

General settings includes **Receive preview updates**, off by default. It is stored as `General / ReceivePreviewUpdates` in `Dockzi.config`, not in the installed `Dockzi.install.json`. OK saves it independently of add-in edits; Cancel discards it. Failed writes restore the cached policy and leave the dialog open for retry.

Startup and manual checks read the saved preference. Stable checks use GitHub's latest-stable endpoint, so a large number of previews cannot hide the stable release beyond the first API page. Opted-in users include published GitHub prereleases; other users consider stable GitHub releases only. Turning previews off does not downgrade an installed preview: the next newer stable package becomes eligible. Explicitly empty update URLs still disable update checks.

## Local verification

`./scripts/Invoke-CI.ps1` runs the application tests, `Test-DockziVersion.ps1`, and `Test-ReleaseAutomation.ps1`. The release regressions use a disposable local Git remote and a mocked GitHub CLI, exercising version increments, retries, master promotion, tag collisions, checksums, and recovery of draft uploads without contacting GitHub or modifying real repository tags.

Normal builds do not modify the version. For a deliberate local increment, run `./scripts/Increment-DockziVersion.ps1`. Version components cannot exceed 65534; the increment script fails rather than rolling another component forward.

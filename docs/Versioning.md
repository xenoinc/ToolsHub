# Core executable version

Edit `DockziVersion` in `source/Directory.Build.props`. The core project is `source/Xeno.Dockzi/Xeno.Dockzi.csproj`, which produces `Dockzi.exe`. Its assembly, file, and informational versions all use this value. The format is **Major.Minor.Revision.Build**; only the fourth component is incremented automatically. For example, `0.9.1.57` becomes `0.9.1.58`.

`.github/workflows/increment-core-version.yml` handles merged PRs targeting `develop`, including squash and rebase merges. Closed, unmerged PRs and direct pushes do not increment the version. After a merge, the workflow commits the increment to `develop` using `github-actions[bot]`. Each merge increments once, even if only add-in files changed. Commit markers make successful reruns idempotent; fetch-and-retry handles simultaneous merges without force pushes or cancelling queued runs.

The workflow uses `pull_request_target` so merges from forks can use the base repository token. It only checks out the merged `develop` branch and never executes the PR head. Install the workflow on the repository's default branch as well as `develop`. Enable GitHub Actions and allow its token to write repository contents. Branch protection/rulesets must permit the automation commit to `develop`; otherwise the job fails and can be rerun after permissions are corrected. No token or branch protection settings are changed by these files.

Add-in projects retain their existing `AssemblyInfo.cs` versions and do not inherit the core version. Independent add-in increments are deferred. Installer/update packages map `DockziVersion` to the CLI-required SemVer2 format: `0.9.1.57` becomes `0.9.1-build.57`, so automated Build increments also advance the update package version. The existing `build.ps1 -Version` parameter can explicitly override this default.

For a deliberate local increment, run `./scripts/Increment-DockziVersion.ps1`. Normal local builds do not change the version. Components cannot exceed 65534; the script fails rather than rolling another component forward.

Run `./scripts/Test-DockziVersion.ps1` for the standalone script regression checks; the workflow runs these before editing the version.

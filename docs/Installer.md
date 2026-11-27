# Dockzi installer and updates

Dockzi uses **Velopack 1.2.161**, pinned in `Directory.Packages.props` and the repository-local `.config/dotnet-tools.json`. NSIS is not required. The .NET 10 SDK and Windows are required for packaging.

## Build

```powershell
.\build.ps1
.\build.ps1 -Version 0.9.2 -UpdateUrl 'https://your-server.example/dockzi/'
```

The first command builds a self-contained Windows x64 release configured for stable GitHub Releases from `https://github.com/xenoinc/ToolsHub`. The second overrides that source with an HTTPS Velopack feed directory. Pass `-UpdateUrl ''` explicitly to produce an installation without update checks.

`build.ps1` restores NuGet dependencies and the pinned Velopack CLI, rebuilds the solution, and invokes `installer/pack.ps1`. Packaging publishes the host and four bundled add-ins (Notes, Pomodoro, Sidebar, and VeraCrypt) to isolated staging. Shared dependency conflicts stop the build. Local configuration, notes, logs, caches, sample add-ins, and tests are excluded.

Artifacts are written directly into **publish**:

- `Dockzi-win-Setup.exe`: per-user installer; no administrator privileges required.
- `Dockzi-<version>-full.nupkg`: update package.
- `releases.win.json` and any generated release-index files: update feed.
- `checksums-win-x64.json`: SHA-256 hashes of generated artifacts.

Velopack controls exact artifact names. Prior release packages are retained so Velopack can create delta updates. The `.staging` directory is temporary build input and must not be uploaded. Never package the developer's `output` directory manually.

The default package version comes from `DockziVersion` in `source/Directory.Build.props`. The CLI requires three-part SemVer2, so core `0.9.1.57` maps to package `0.9.1-build.57`; core `0.9.1.58` maps to the newer package `0.9.1-build.58`. This label is a SemVer prerelease label. GitHub's draft/prerelease checkbox is a separate setting: the default source searches published GitHub releases that are not marked prerelease.

`-Version` remains available for deliberate overrides, including stable three-part releases. A stable package `0.9.1` sorts above every `0.9.1-build.N` package; advance the core Revision to `0.9.2` before publishing subsequent default build packages. Publish a higher package version for each update; changing only the GitHub tag does not make an existing package newer. Older packages named `Dockzi-0.6.0-full.nupkg` still have version `0.6.0`, even if their GitHub release tag says `v0.9.1.57`. `-Runtime win-arm64` can target ARM64. Use a separate `-Channel` and feed directory when distributing architectures or release channels independently.

Options:

```powershell
.\build.ps1 -SkipInstaller                 # Release compilation only
.\build.ps1 -SkipRestore                   # Reuse restored tools and packages, including RID runtime packs
.\installer\pack.ps1 -Version 0.9.2         # Package directly without the solution cleanup/rebuild
```

The first build needs network access to NuGet for the CLI, libraries, and self-contained runtime packs. Subsequent builds can reuse cached packages. Dependency updates should be reviewed changes to the pinned versions, rather than automatically selecting new versions during every build.

## Persistent files and add-ins

The installer owns `%LocalAppData%\Dockzi`, including its replaceable `current` directory. Packaged Dockzi stores its mutable files under **%LocalAppData%\XI\Dockzi**:

```text
Dockzi.config
Notes\
Logs\
AddinRegistry\
Addins\
AddinData\
Downloads\
```

The Mono.Addins registry/cache and optional user add-ins have independent paths. Bundled DLLs stay with the application and update together. Downloads is outside the scanned add-in directory. The online add-in repository, publisher verification, external service-module registration, and independent add-in updates are future work; installing this release does not implement an online marketplace. Only install add-in code you trust.

Uninstalling the application leaves its user-data directory intact. To move from a legacy portable installation, close Dockzi and copy its `Dockzi.config`, `Notes` folder, and any `SidebarLaunchers.json` into the user-data directory before launching the installed version. Keep the original files as a backup. Existing development/portable runs retain the legacy storage selection. A `Dockzi.portable` marker beside the packaged executable explicitly opts into portable storage.

Windows sign-in startup uses Velopack's stable root `Dockzi.exe` launcher. Logs, settings, and notes remain outside `current`, including after an update. Velopack startup hooks run before logging, the single-instance mutex, or add-in initialization.

## Publish updates

For the default GitHub source, create a published stable release in `xenoinc/ToolsHub` and attach the generated `releases.win.json`, `.nupkg` packages, and Setup.exe. Draft and prerelease GitHub releases are excluded. For an HTTPS feed override, upload the generated release indexes and `.nupkg` files to that feed directory. Distribute Setup.exe for first installs. Do not upload `.staging` or any user data. A different channel uses its corresponding `releases.<channel>.json` index. This build script generates files locally and does not upload or publish anything.

Configured installations automatically check for updates before creating add-ins or note windows. A newer version is downloaded and applied through the normal service-disposal/restart path before editing begins. An up-to-date, offline, or failed check continues normal startup without pop-ups; failures are logged. Startup feed checks have a 15-second deadline and downloads have a two-minute cancellation deadline. Development and portable copies skip startup updates.

**Check for updates…** remains available in the tray for an explicit check; this path asks before downloading/restarting. GitHub repository URLs use Velopack's `GithubSource`, which selects published stable releases, rather than treating the repository HTML page as a JSON feed. Bundled add-ins update with the host; per-user add-in files and note data remain intact.

Older installations with an empty `UpdateUrl` and no startup-check implementation require a one-time installation of a newly built Setup.exe. Changing `Dockzi.install.json` alone cannot add startup logic to an old executable. An explicitly empty update URL still disables updating.

For production, sign the application and installer using your certificate/signing service:

```powershell
.\build.ps1 -Version 0.9.2 -UpdateUrl 'https://your-server.example/dockzi/' -SignTemplate 'your-signing-command "{{file}}"'
```

`SignTemplate` is forwarded to Velopack. Keep certificate credentials in your signing service or CI secret storage. Without it, the generated installer is unsigned. SHA-256 checksum files help verify transfer integrity; they do not authenticate a publisher by themselves.

References: [Velopack .NET integration](https://docs.velopack.io/getting-started/csharp), [Windows installation layout](https://docs.velopack.io/packaging/operating-systems/windows).

The General setting **Enable Auto-Start with OS** registers a quoted launcher path with `--auto-start` in the current user's `HKCU\Software\Microsoft\Windows\CurrentVersion\Run` key, under the value `Dockzi`. Windows runs it when that user signs in. Manual launches and saving the enabled setting repair a registration whose old Dockzi executable has been removed. Repair preserves disabled settings, custom commands, and registrations pointing to another existing installation.

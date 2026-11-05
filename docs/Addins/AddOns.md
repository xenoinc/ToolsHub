# Extension Add-ins

## Extension Endpoints

| Node Name | Path |
|---|---|
| StartupAddin | Dockzi/AppStartup |
|  | Dockzi/Sidebar |
|  | Dockzi/SystemTray |

## Creating WPF Add-in
1. Create new project: WPF User Control
2. XML file change, ``UserControl`` to ``Window``
3. CS change, ``System.Windows.Controls.UserControl`` to ``System.Windows.Window``
    * Simply, ``UserControl`` to ``Window``


### Add-in events

The host and all in-process add-ins share `AppEvents.Instance`, an `IEventAggregator` from [Lite.EventIpc 1.2.0](https://github.com/SuessLabs/Lite.EventIpc). Use typed events instead of the former `MessagingCenter` API:

```csharp
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.Services.Messaging;

AppEvents.Instance.Publish(new SystemTrayRefreshRequest());
AppEvents.Instance.Publish(new SystemTrayNotificationRequest("Work completed"));
AppEvents.Instance.Publish(new SystemTrayChangeIconRequest(null)); // Restore the default icon.
```

Events also include `SystemTrayUpdateRequest` and `ShortcutsRefreshRequested`. Menu-item updates remain a publisher-only contract; the tray does not yet implement that feature. Keep subscribed `Action<TEvent>` delegates in fields because Lite.EventIpc holds them weakly, and pass those same delegates to `Unsubscribe` when disposing. UI subscribers must marshal callbacks to the UI thread when events originate from a timer or background task.

External add-ins using the old messaging API must be rebuilt with these contracts. This integration uses local event dispatch; no IPC transport or network listener is configured.

## Dependency injection rollout

The host uses `Microsoft.Extensions.DependencyInjection`, with registrations in `Services/ApplicationServices.cs`. `Program.Main` builds and validates one provider on the UI thread. Settings, the event aggregator, startup registration, and host managers have application lifetimes. Each Settings dialog opens a service scope and receives fresh page instances; closing the scope disposes the pages and activated disposable add-ins.

Mono.Addins still discovers extension types. `AddinFactory` uses `ActivatorUtilities.CreateInstance` to supply constructor dependencies, and owns disposable instances created through that activation boundary. Existing parameterless add-ins continue to work. A migrated add-in can accept dependencies directly:

```csharp
public PreferencesHandler(ShortcutsPreferences page)
{
  _page = page;
}
```

The first slice migrates the host, settings-dialog composition, and built-in Shortcuts add-in. New code should inject `SettingsManager` and `IEventAggregator`; it should not resolve services through a global container. `Program.Settings`, `SettingsService`, and `AppEvents.Instance` remain compatibility access for external add-ins during the rollout. Startup binds these to the same services supplied by DI.

The second slice migrates Pomodoro, Sidebar, and VeraCrypt. Their `ServiceModule` classes implement `IAddinServiceModule` and register dependencies before the provider is built. Startup calls `AddBundledAddinServices` to load registration modules from the installed `PomodoroAddin.dll`, `SidebarAddin.dll`, and `VeraCryptAddin.dll` files. Disabled bundles are registered too; registration does not construct their running managers or enable them in Mono.Addins.

Managers and their runtime services are application singletons. Preference pages are transient objects resolved inside each dialog scope. Inject `ISettingsService` for typed settings access and `IEventAggregator` for events. Pomodoro pages edit configuration without creating or restarting the running timer. The timer stops and detaches callbacks on shutdown. Sidebar owns and closes its window; reopening Settings does not create another Sidebar. VeraCrypt uses injectable operations and secret-protection interfaces, and volume operations occur only for explicit commands. Its previously unimplemented automatic mount/dismount settings are not activated by this rollout.

`Program.Settings`, the static `SettingsService`, and `AppEvents.Instance` remain available for external add-in compatibility. The three migrated bundles no longer use them. An external bundle can implement `IAddinServiceModule` and have its module registered at the composition root before building the provider; the current automatic loader is deliberately limited to the three bundled assembly names. Logging remains unchanged.

## Sidebar launchers

Sidebar owns its launcher list through `ILauncherStore` and `ILauncherShell`.
`SettingsLauncherStore` stores an ordered list of `Id`, `Title`, and `Target` values
under `Sidebar / LauncherItems` in `Dockzi.db`, using the host's Base64 JSON object
format. Appearance preferences use the same Sidebar property bag. The Shortcuts add-in's
property bag remains independent. Import, reorder, and remove operations save immediately.
`SettingsManager.SaveFileOrThrow` atomically replaces the settings file, and a failed
launcher save rolls back the cached value as well as keeping the previous displayed list.

When the launcher key is absent, the store imports the old `SidebarLaunchers.json`
file once and retains that file as a backup. Database entries, including an empty list,
always take precedence over the legacy file. Invalid database or migration data is
reported and preserved rather than silently replaced.

The Windows shell adapter extracts each target's icon, releases native icon handles,
and supplies an application icon when extraction fails. Launches use the shell so
Windows links retain their arguments and working directory; direct executables start
in their containing directory. `ToolbarViewModel` owns the displayed collection, while
`ToolbarView` handles drag gestures, hover animation, scrolling, and monitor placement.
Left/right layouts are vertical, top/bottom layouts horizontal. Saving sidebar
preferences refreshes an already-open dock. Display changes reposition the dock, and
closing it detaches display and collection event subscriptions.

Appearance changes now update that same window through `ISidebarWindow.ApplySettings`.
The preference page passes a `SidebarAppearance` snapshot for previews without writing
to `ISettingsService`; disposing the page restores committed settings. Apply/OK writes
the snapshot's settings, so subsequent Cancel restores the latest committed values.

`DockAutoHide` controls the idle delay and reversible slide. `ToolbarView` keeps its
native window inside the selected monitor, translates content inside a clipped viewport,
and limits the native paint/input region to the visible portion. A three-DIP edge strip
remains as the hover target. Drag operations, open context menus, and the file picker
keep the dock visible. Animation and running-app polling timers stop when the dock closes.

`IRunningApplications` supplies a snapshot of executable paths belonging to visible,
uncloaked top-level windows. Its Windows implementation resolves `.lnk` targets without
launching them and releases process/COM resources after use. The view model updates
`LauncherIcon.IsRunning` once per second; the template displays a dot for matching targets.

The host tracks active utility extensions and starts/stops them as add-ins are enabled
or disabled. `UtilityAddin.OnEnabled()` defaults to `Execute()` for compatibility.
Sidebar overrides it to show immediately after explicit activation; its startup
`Execute()` still respects the startup visibility preference. Repeated loads do not
create duplicate utility instances, and shutdown closes active utilities.

The host tracks active utility extensions and starts/stops them as add-ins are enabled
or disabled. `UtilityAddin.OnEnabled()` defaults to `Execute()` for compatibility.
Sidebar overrides it to show immediately after explicit activation; its startup
`Execute()` still respects the startup visibility preference. Repeated loads do not
create duplicate utility instances, and shutdown closes active utilities.

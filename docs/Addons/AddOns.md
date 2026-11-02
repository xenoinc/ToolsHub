# Extension Add-ins

## Extension Endpoints

| Node Name | Path |
|---|---|
| StartupAddin | ToolsHub/AppStartup |
|  | ToolsHub/Sidebar |
|  | ToolsHub/SystemTray |

## Creating WPF Add-in
1. Create new project: WPF User Control
2. XML file change, ``UserControl`` to ``Window``
3. CS change, ``System.Windows.Controls.UserControl`` to ``System.Windows.Window``
    * Simply, ``UserControl`` to ``Window``


### Add-in events

The host and all in-process add-ins share `AppEvents.Instance`, an `IEventAggregator` from [Lite.EventIpc 1.2.0](https://github.com/SuessLabs/Lite.EventIpc). Use typed events instead of the former `MessagingCenter` API:

```csharp
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services.Messaging;

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

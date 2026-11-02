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

AppEvents.Instance.Publish(new TrayRefreshRequested());
AppEvents.Instance.Publish(new TrayNotificationRequested("Work completed"));
AppEvents.Instance.Publish(new TrayIconChanged(null)); // Restore the default icon.
```

Events also include `TrayItemUpdateRequested` and `ShortcutsRefreshRequested`. Menu-item updates remain a publisher-only contract; the tray does not yet implement that feature. Keep subscribed `Action<TEvent>` delegates in fields because Lite.EventIpc holds them weakly, and pass those same delegates to `Unsubscribe` when disposing. UI subscribers must marshal callbacks to the UI thread when events originate from a timer or background task.

External add-ins using the old messaging API must be rebuilt with these contracts. This integration uses local event dispatch; no IPC transport or network listener is configured.

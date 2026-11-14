namespace Xeno.Dockzi.Services.Messaging;

using System;
using Lite.EventIpc;
using Xeno.Dockzi.ExtensionModel.SystemTray;

/// <summary>Owns tray handler delegates for Lite.EventIpc's weak subscriptions.</summary>
public sealed class TrayEventSubscriptions : IDisposable
{
  private readonly IEventAggregator _events;
  private readonly Action<SystemTrayRefreshRequest> _refresh;
  private readonly Action<SystemTrayNotificationRequest> _notify;
  private readonly Action<SystemTrayChangeIconRequest> _icon;
  private volatile bool _disposed;

  public TrayEventSubscriptions(
    IEventAggregator events,
    Action refresh,
    Action<string> notify,
    Action<System.Drawing.Icon> changeIcon,
    Action<Action> dispatch)
  {
    _events = events ?? throw new ArgumentNullException(nameof(events));
    ArgumentNullException.ThrowIfNull(refresh);
    ArgumentNullException.ThrowIfNull(notify);
    ArgumentNullException.ThrowIfNull(changeIcon);
    ArgumentNullException.ThrowIfNull(dispatch);

    // Keep delegates alive while subscribed and check disposal after queued UI dispatch.
    _refresh = _ => dispatch(() =>
    {
      if (!_disposed)
        refresh();
    });

    _notify = message => dispatch(() =>
    {
      if (!_disposed)
        notify(message.Message);
    });

    _icon = message => dispatch(() =>
    {
      if (!_disposed)
        changeIcon(message.Icon);
    });

    _events.Subscribe(_refresh);
    _events.Subscribe(_notify);
    _events.Subscribe(_icon);
  }

  public void Dispose()
  {
    if (_disposed)
      return;

    _disposed = true;
    _events.Unsubscribe(_refresh);
    _events.Unsubscribe(_notify);
    _events.Unsubscribe(_icon);
  }
}

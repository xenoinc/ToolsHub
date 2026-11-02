namespace Xeno.ToolsHub.Tests.SystemTests.Messaging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lite.EventIpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services.Messaging;

[TestClass]
public class AppEventsTests
{
  [TestMethod]
  public void ShortcutManagerPublishesRefreshOnSharedAggregator()
  {
    int count = 0;
    Action<TrayRefreshRequested> handler = _ => count++;
    AppEvents.Instance.Subscribe(handler);

    try
    {
      var shortcuts = new ShortcutsManager(new SettingsManager(StorageMethod.UnitTest));
      shortcuts.Refresh();
      shortcuts.Refresh();
      Assert.AreEqual(2, count);
    }
    finally
    {
      AppEvents.Instance.Unsubscribe(handler);
    }

    AppEvents.Instance.Publish(new TrayRefreshRequested());
    Assert.AreEqual(2, count);
  }

  [TestMethod]
  public void TypedNotificationAndMenuPayloadsDoNotTriggerOtherEvents()
  {
    var events = new EventAggregator();
    string received = null;
    TrayItemInfo item = null;
    int refreshCount = 0;

    Action<TrayNotificationRequested> notification = message => received = message.Message;
    Action<TrayItemUpdateRequested> update = message => item = message.Item;
    Action<TrayRefreshRequested> refresh = _ => refreshCount++;

    events.Subscribe(notification);
    events.Subscribe(update);
    events.Subscribe(refresh);

    var expected = new TrayItemInfo("TestAddin", "timer", "Timer", string.Empty, true);

    events.Publish(new TrayNotificationRequested("Timer completed"));
    events.Publish(new TrayItemUpdateRequested(expected));
    events.Publish(new ShortcutsRefreshRequested());

    Assert.AreEqual("Timer completed", received);
    Assert.AreSame(expected, item);
    Assert.AreEqual(0, refreshCount);

    GC.KeepAlive(notification);
    GC.KeepAlive(update);
    GC.KeepAlive(refresh);
  }

  [TestMethod]
  public void TraySubscriptionsRetainHandlersAcrossGarbageCollection()
  {
    var events = new EventAggregator();
    int refreshCount = 0;
    string notification = null;
    bool iconReset = false;

    using var subscriptions = new TrayEventSubscriptions(
      events, () => refreshCount++, message => notification = message, icon => iconReset = icon == null, action => action());

    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();

    events.Publish(new TrayRefreshRequested());
    events.Publish(new TrayNotificationRequested("Hello"));
    events.Publish(new TrayIconChanged(null));

    Assert.AreEqual(1, refreshCount);
    Assert.AreEqual("Hello", notification);
    Assert.IsTrue(iconReset);
  }

  [TestMethod]
  public void DisposingTraySubscriptionsStopsAllDeliveryIncludingQueuedEvents()
  {
    var events = new EventAggregator();
    var queued = new List<Action>();
    int callbackCount = 0;

    var subscriptions = new TrayEventSubscriptions(
      events, () => callbackCount++, _ => callbackCount++, _ => callbackCount++, queued.Add);

    events.Publish(new TrayRefreshRequested());
    events.Publish(new TrayNotificationRequested("queued"));
    events.Publish(new TrayIconChanged(null));
    Assert.AreEqual(3, queued.Count);

    subscriptions.Dispose();
    subscriptions.Dispose();

    foreach (var action in queued)
      action();

    events.Publish(new TrayRefreshRequested());
    events.Publish(new TrayNotificationRequested("disposed"));
    events.Publish(new TrayIconChanged(null));
    Assert.AreEqual(0, callbackCount);
    Assert.AreEqual(3, queued.Count);
  }

  [TestMethod]
  public async Task BackgroundPublisherUsesDispatcherBeforeUpdatingTray()
  {
    var events = new EventAggregator();
    var queued = new System.Collections.Concurrent.ConcurrentQueue<Action>();
    int callbackThread = 0;

    using var subscriptions = new TrayEventSubscriptions(
      events, () => callbackThread = Environment.CurrentManagedThreadId, _ => { }, _ => { }, queued.Enqueue);

    await Task.Run(() => events.Publish(new TrayRefreshRequested()));
    Assert.AreEqual(0, callbackThread);
    Assert.IsTrue(queued.TryDequeue(out var action));

    int dispatchThread = Environment.CurrentManagedThreadId;
    action();

    Assert.AreEqual(dispatchThread, callbackThread);
  }
}

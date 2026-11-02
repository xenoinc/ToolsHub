namespace Xeno.ToolsHub.Tests.SystemTests;

using System;
using Lite.EventIpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Addins;
using Xeno.ToolsHub.ExtensionModel.SystemTray;

[TestClass]
public class DependencyInjectionTests
{
  [TestMethod]
  public void HostRegistrationsValidateWithoutStartingTheUi()
  {
    using var provider = BuildProvider();
    Assert.IsNotNull(provider.GetRequiredService<IPreferencesDialogService>());
    Assert.AreSame(provider.GetRequiredService<SettingsManager>(), provider.GetRequiredService<SettingsManager>());
    Assert.AreSame(provider.GetRequiredService<IEventAggregator>(), provider.GetRequiredService<IEventAggregator>());
  }

  [TestMethod]
  public void InjectedShortcutEventsAreIsolatedBetweenProviders()
  {
    using var first = BuildProvider();
    using var second = BuildProvider();
    var events = first.GetRequiredService<IEventAggregator>();
    int firstCount = 0;
    int secondCount = 0;
    Action<SystemTrayRefreshRequest> firstHandler = _ => firstCount++;
    Action<SystemTrayRefreshRequest> secondHandler = _ => secondCount++;
    events.Subscribe(firstHandler);
    second.GetRequiredService<IEventAggregator>().Subscribe(secondHandler);

    first.GetRequiredService<ShortcutsManager>().Refresh();
    Assert.AreEqual(1, firstCount);
    Assert.AreEqual(0, secondCount);
    Assert.AreNotSame(first.GetRequiredService<ShortcutsManager>(), first.GetRequiredService<ShortcutsManager>());
    GC.KeepAlive(firstHandler);
    GC.KeepAlive(secondHandler);
  }

  [TestMethod]
  public void AddinActivationInjectsDependenciesAndScopeDisposesOwnedObjects()
  {
    using var provider = BuildProvider();
    InjectedAddin addin;
    AddinFactory factory;
    using (var scope = provider.CreateScope())
    {
      factory = scope.ServiceProvider.GetRequiredService<AddinFactory>();
      addin = (InjectedAddin)factory.Create(typeof(InjectedAddin));
      Assert.AreSame(provider.GetRequiredService<SettingsManager>(), addin.Settings);
      Assert.AreSame(provider.GetRequiredService<IEventAggregator>(), addin.Events);
      Assert.IsFalse(addin.IsDisposed);
    }

    Assert.IsTrue(addin.IsDisposed);
    Assert.ThrowsExactly<ObjectDisposedException>(() => factory.Create(typeof(InjectedAddin)));
    factory.Dispose();
  }

  [TestMethod]
  public void ParameterlessAddinsRemainSupported()
  {
    using var provider = BuildProvider();
    using var scope = provider.CreateScope();
    var factory = scope.ServiceProvider.GetRequiredService<AddinFactory>();
    Assert.IsInstanceOfType<LegacyAddin>(factory.Create(typeof(LegacyAddin)));
  }

  [TestMethod]
  public void BuiltInPreferencePagesHaveFreshStateAndAreDisposedWithTheirScope()
  {
    Exception failure = null;
    var thread = new System.Threading.Thread(() =>
    {
      try
      {
        using var provider = BuildProvider();
        Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.ShortcutsPreferences firstPage;
        using (var scope = provider.CreateScope())
        {
          var factory = scope.ServiceProvider.GetRequiredService<AddinFactory>();
          var handler = (Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.PreferencesHandler)factory.Create(
            typeof(Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.PreferencesHandler));
          handler.InitializePage();
          firstPage = (Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.ShortcutsPreferences)handler.Page;
          firstPage.IsModified = true;
        }

        Assert.IsTrue(firstPage.IsDisposed);
        using var secondScope = provider.CreateScope();
        var secondPage = secondScope.ServiceProvider.GetRequiredService<Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.ShortcutsPreferences>();
        Assert.AreNotSame(firstPage, secondPage);
        Assert.IsFalse(secondPage.IsModified);
      }
      catch (Exception ex)
      {
        failure = ex;
      }
    });
    thread.SetApartmentState(System.Threading.ApartmentState.STA);
    thread.Start();
    thread.Join();
    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }

  private static ServiceProvider BuildProvider()
  {
    var services = new ServiceCollection().AddToolsHub();
    services.AddSingleton(new SettingsManager(StorageMethod.UnitTest));
    return services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
  }

  public sealed class InjectedAddin : IDisposable
  {
    public InjectedAddin(SettingsManager settings, IEventAggregator events)
    {
      Settings = settings;
      Events = events;
    }

    public SettingsManager Settings { get; }

    public IEventAggregator Events { get; }

    public bool IsDisposed { get; private set; }

    public void Dispose()
    {
      IsDisposed = true;
    }
  }

  public sealed class LegacyAddin
  {
  }
}

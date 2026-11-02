namespace Xeno.ToolsHub.Tests.SystemTests;

using System;
using System.Threading;
using System.Windows.Forms;
using Lite.EventIpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PomodoroAddin.Domain;
using PomodoroAddin.Managers;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Addins;
using Xeno.ToolsHub.SidebarAddin.Managers;
using Xeno.ToolsHub.SidebarAddin.Services;
using Xeno.ToolsHub.VeraCryptAddin;
using Xeno.ToolsHub.VeraCryptAddin.Services;

[TestClass]
[DoNotParallelize]
public class BundledAddinTests
{
  [TestInitialize]
  public void Initialize() => TestHelpers.PrepareTestsFolder();

  [TestCleanup]
  public void Cleanup() => TestHelpers.CleanTestsFolder();

  [TestMethod]
  public void ModulesRegisterSharedManagersAndSafeLazyServices()
  {
    using var fixture = new Fixture();
    using var first = fixture.Provider.CreateScope();
    using var second = fixture.Provider.CreateScope();
    Assert.AreSame(first.ServiceProvider.GetRequiredService<PomodoroManager>(), second.ServiceProvider.GetRequiredService<PomodoroManager>());
    Assert.AreSame(first.ServiceProvider.GetRequiredService<SidebarManager>(), second.ServiceProvider.GetRequiredService<SidebarManager>());
    Assert.AreSame(first.ServiceProvider.GetRequiredService<VeraCryptManager>(), second.ServiceProvider.GetRequiredService<VeraCryptManager>());
    Assert.AreEqual(0, fixture.Timer.StartCount);
    Assert.AreEqual(0, fixture.Windows.Created);
    Assert.AreEqual(0, fixture.Volumes.MountCount);
    Assert.AreEqual(0, fixture.Volumes.DismountCount);
  }

  [TestMethod]
  public void BundledModuleDiscoveryRegistersAllInstalledBundles()
  {
    var services = new ServiceCollection().AddToolsHub();
    services.AddBundledAddinServices(System.IO.Path.GetDirectoryName(typeof(PomodoroManager).Assembly.Location));
    services.AddSingleton(new SettingsManager(StorageMethod.UnitTest));
    using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    Assert.IsNotNull(provider.GetRequiredService<PomodoroManager>());
    Assert.IsNotNull(provider.GetRequiredService<SidebarManager>());
    Assert.IsNotNull(provider.GetRequiredService<VeraCryptManager>());
  }

  [TestMethod]
  public void PomodoroTicksPauseResumeAndDisposalUseOneTimer()
  {
    using var fixture = new Fixture();
    fixture.Settings.SetValue(PomodoroAddin.Constants.AddinId, PomodoroAddin.Constants.KeyDuration, "2");
    var manager = fixture.Provider.GetRequiredService<PomodoroManager>();
    int iconEvents = 0;
    Action<SystemTrayChangeIconRequest> icon = _ => iconEvents++;
    fixture.Provider.GetRequiredService<IEventAggregator>().Subscribe(icon);
    manager.OnStart(string.Empty);
    Assert.AreEqual(2, manager.RemainingMinutes);
    fixture.Timer.RaiseTick();
    Assert.AreEqual(1, manager.RemainingMinutes);
    manager.OnPause(string.Empty);
    Assert.IsFalse(fixture.Timer.IsRunning);
    fixture.Timer.RaiseTick();
    Assert.AreEqual(1, manager.RemainingMinutes);
    manager.OnPause(string.Empty);
    Assert.IsTrue(fixture.Timer.IsRunning);
    fixture.Timer.RaiseTick();
    Assert.AreEqual(PomodoroAddin.TimerState.Done, manager.CurrentState);
    Assert.IsFalse(fixture.Timer.IsRunning);
    Assert.IsTrue(iconEvents >= 3);
    fixture.Provider.Dispose();
    int eventsBeforeDisposedTick = iconEvents;
    fixture.Timer.RaiseTick();
    Assert.AreEqual(eventsBeforeDisposedTick, iconEvents);
    Assert.AreEqual(0, fixture.Timer.SubscriberCount);
    Assert.IsTrue(fixture.Timer.IsDisposed);
    Assert.ThrowsExactly<ObjectDisposedException>(() => manager.OnStart(string.Empty));
    GC.KeepAlive(icon);
  }

  [TestMethod]
  public void PomodoroEditorsShareRunningManagerAndPersistWithoutRestartingTimer()
  {
    RunSta(() =>
    {
      using var fixture = new Fixture();
      var manager = fixture.Provider.GetRequiredService<PomodoroManager>();
      manager.OnStart(string.Empty);
      PomodoroAddin.Views.PreferencePage firstPage;
      using (var scope = fixture.Provider.CreateScope())
      {
        var handler = (PomodoroAddin.Handlers.PreferenceHandler)scope.ServiceProvider.GetRequiredService<AddinFactory>().Create(typeof(PomodoroAddin.Handlers.PreferenceHandler));
        handler.InitializePage();
        firstPage = (PomodoroAddin.Views.PreferencePage)handler.Page;
        Find<TextBox>(firstPage, "TxtDuration").Text = "7";
        Assert.IsTrue(firstPage.IsModified);
        Assert.IsTrue(firstPage.OnSave());
        Assert.AreEqual(7, manager.SettingTimerDuration);
        Assert.AreEqual(25, manager.RemainingMinutes);
      }

      Assert.IsTrue(firstPage.IsDisposed);
      using var nextScope = fixture.Provider.CreateScope();
      var secondPage = nextScope.ServiceProvider.GetRequiredService<PomodoroAddin.Views.PreferencePage>();
      Assert.AreEqual("7", Find<TextBox>(secondPage, "TxtDuration").Text);
      Assert.IsFalse(secondPage.IsModified);
      Assert.IsTrue(fixture.Timer.IsRunning);
      Assert.AreEqual(1, fixture.Timer.StartCount);
      fixture.Settings.SaveFile();
      var reloaded = new SettingsManager(StorageMethod.UnitTest);
      reloaded.LoadFile();
      Assert.AreEqual("7", reloaded.GetValue(PomodoroAddin.Constants.AddinId, PomodoroAddin.Constants.KeyDuration));
    });
  }

  [TestMethod]
  public void SidebarWindowHasSingleOwnerAndClosesAtShutdown()
  {
    using var fixture = new Fixture();
    var manager = fixture.Provider.GetRequiredService<SidebarManager>();
    manager.ShowSidebars();
    manager.ShowSidebars();
    Assert.AreEqual(1, fixture.Windows.Created);
    fixture.Windows.Last.Close();
    manager.ShowSidebars();
    Assert.AreEqual(2, fixture.Windows.Created);
    var last = fixture.Windows.Last;
    fixture.Provider.Dispose();
    Assert.AreEqual(1, last.CloseCount);
    Assert.AreEqual(0, last.SubscriberCount);
    Assert.ThrowsExactly<ObjectDisposedException>(() => manager.ShowSidebars());
  }

  [TestMethod]
  public void SidebarEditorLoadsIndependentFieldsAndCancelKeepsSettings()
  {
    RunSta(() =>
    {
      using var fixture = new Fixture();
      const string id = "Sidebar";
      fixture.Settings.SetValue(id, "BackgroundImage", "wallpaper.png");
      fixture.Settings.SetValue(id, "ScreenArea", "2");
      fixture.Settings.SetValue(id, "ScreenOrientation", "1");
      fixture.Settings.SetValue(id, "ScreenNumber", "1");
      fixture.Settings.SetValue(id, "Opacity", "70");
      using (var scope = fixture.Provider.CreateScope())
      {
        var page = scope.ServiceProvider.GetRequiredService<Xeno.ToolsHub.SidebarAddin.Views.PreferencesPage>();
        Assert.AreEqual(2, Find<ComboBox>(page, "ComboScreenArea").SelectedIndex);
        Assert.AreEqual(0, Find<ComboBox>(page, "ComboScreenOrientaiton").SelectedIndex);
        Find<ComboBox>(page, "ComboScreenArea").SelectedIndex = 3;
        Find<ComboBox>(page, "ComboScreenOrientaiton").SelectedIndex = 0;
        Assert.IsTrue(page.OnSave());
      }

      Assert.AreEqual("3", fixture.Settings.GetValue(id, "ScreenArea"));
      Assert.AreEqual("1", fixture.Settings.GetValue(id, "ScreenOrientation"));
      Assert.AreEqual("1", fixture.Settings.GetValue(id, "ScreenNumber"));
      Assert.AreEqual("wallpaper.png", fixture.Settings.GetValue(id, "BackgroundImage"));
      using (var scope = fixture.Provider.CreateScope())
      {
        var page = scope.ServiceProvider.GetRequiredService<Xeno.ToolsHub.SidebarAddin.Views.PreferencesPage>();
        Find<ComboBox>(page, "ComboScreenArea").SelectedIndex = 4;
      }

      Assert.AreEqual("3", fixture.Settings.GetValue(id, "ScreenArea"));
      Assert.AreEqual(0, fixture.Windows.Created);
    });
  }

  [TestMethod]
  public void VeraCryptUsesSubstituteOperationsAndPreservesExitCodes()
  {
    using var fixture = new Fixture();
    var settings = fixture.Provider.GetRequiredService<ISettingsService>();
    settings.SetValue("VeraCryptAddin", "InstallPath", "fake-veracrypt.exe");
    settings.SetValue("VeraCryptAddin", "HcDrive", "Z");
    settings.SetValue("VeraCryptAddin", "HcPath", "protected:volume.hc");
    settings.SetValue("VeraCryptAddin", "HcPass", "protected:test-value");
    settings.SetValue("VeraCryptAddin", "ForceDismounts", true);
    var manager = fixture.Provider.GetRequiredService<VeraCryptManager>();
    Assert.AreEqual(17, manager.OnMount(string.Empty));
    Assert.AreEqual("volume.hc", fixture.Volumes.Path);
    Assert.AreEqual("test-value", fixture.Volumes.Password);
    Assert.AreEqual(23, manager.OnDismount(string.Empty));
    Assert.IsTrue(fixture.Volumes.Force);
    Assert.AreEqual(1, fixture.Volumes.MountCount);
    Assert.AreEqual(1, fixture.Volumes.DismountCount);
  }

  [TestMethod]
  public void VeraCryptEditorPersistsProtectedValuesWithoutPerformingVolumeOperations()
  {
    RunSta(() =>
    {
      using var fixture = new Fixture();
      using (var scope = fixture.Provider.CreateScope())
      {
        var page = scope.ServiceProvider.GetRequiredService<Xeno.ToolsHub.VeraCryptAddin.Views.PreferencesPage>();
        Find<TextBox>(page, "TxtHcPath").Text = "sample.hc";
        Find<TextBox>(page, "TxtHcPass").Text = "test-value";
        Find<CheckBox>(page, "ChkOnStartMount").Checked = true;
        Find<CheckBox>(page, "ChkOnExitDismount").Checked = true;
        Assert.IsTrue(page.OnSave());
      }

      Assert.AreEqual("protected:sample.hc", fixture.Settings.GetValue("VeraCryptAddin", "HcPath"));
      Assert.AreEqual("protected:test-value", fixture.Settings.GetValue("VeraCryptAddin", "HcPass"));
      fixture.Settings.SaveFile();
      var reloaded = new SettingsManager(StorageMethod.UnitTest);
      reloaded.LoadFile();
      Assert.AreEqual("protected:sample.hc", reloaded.GetValue("VeraCryptAddin", "HcPath"));
      fixture.Provider.Dispose();
      Assert.AreEqual(0, fixture.Volumes.MountCount);
      Assert.AreEqual(0, fixture.Volumes.DismountCount);
    });
  }

  private static T Find<T>(Control parent, string name)
    where T : Control => (T)parent.Controls.Find(name, true)[0];

  private static void RunSta(Action action)
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        failure = ex;
      }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();
    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }

  private sealed class Fixture : IDisposable
  {
    public Fixture()
    {
      Settings.SetValue(PomodoroAddin.Constants.AddinId, PomodoroAddin.Constants.KeyAlertSound, "False");
      var services = new ServiceCollection().AddToolsHub();
      services.AddSingleton(Settings);
      services.AddSingleton<IPomodoroTimer>(_ => Timer);
      services.AddSingleton<ISidebarWindowFactory>(Windows);
      services.AddSingleton<IVeraCryptOperations>(Volumes);
      services.AddSingleton<ISecretProtector>(new TestSecrets());
      new PomodoroAddin.ServiceModule().ConfigureServices(services);
      new Xeno.ToolsHub.SidebarAddin.ServiceModule().ConfigureServices(services);
      new Xeno.ToolsHub.VeraCryptAddin.ServiceModule().ConfigureServices(services);
      Provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
    }

    public SettingsManager Settings { get; } = new(StorageMethod.UnitTest);

    public TestTimer Timer { get; } = new();

    public TestWindows Windows { get; } = new();

    public TestVolumes Volumes { get; } = new();

    public ServiceProvider Provider { get; }

    public void Dispose() => Provider.Dispose();
  }

  private sealed class TestTimer : IPomodoroTimer
  {
    public event EventHandler Tick;

    public bool IsRunning { get; private set; }

    public bool IsDisposed { get; private set; }

    public int StartCount { get; private set; }

    public int SubscriberCount => Tick?.GetInvocationList().Length ?? 0;

    public void Start()
    {
      IsRunning = true;
      StartCount++;
    }

    public void Stop() => IsRunning = false;

    public void RaiseTick() => Tick?.Invoke(this, EventArgs.Empty);

    public void Dispose()
    {
      IsDisposed = true;
      IsRunning = false;
    }
  }

  private sealed class TestWindows : ISidebarWindowFactory
  {
    public int Created { get; private set; }

    public TestWindow Last { get; private set; }

    public ISidebarWindow Create()
    {
      Created++;
      return Last = new TestWindow();
    }
  }

  private sealed class TestWindow : ISidebarWindow
  {
    public event EventHandler Closed;

    public int CloseCount { get; private set; }

    public int SubscriberCount => Closed?.GetInvocationList().Length ?? 0;

    public void Show()
    {
    }

    public void Close()
    {
      CloseCount++;
      Closed?.Invoke(this, EventArgs.Empty);
    }
  }

  private sealed class TestVolumes : IVeraCryptOperations
  {
    public int MountCount { get; private set; }

    public int DismountCount { get; private set; }

    public string Path { get; private set; }

    public string Password { get; private set; }

    public bool Force { get; private set; }

    public bool IsInstalled(string executable) => true;

    public int Mount(string executable, string drive, string path, string password)
    {
      MountCount++;
      Path = path;
      Password = password;
      return 17;
    }

    public int Dismount(string executable, string drive, string path, bool force)
    {
      DismountCount++;
      Force = force;
      return 23;
    }
  }

  private sealed class TestSecrets : ISecretProtector
  {
    public string Encrypt(string value) => "protected:" + value;

    public string Decrypt(string value) => value.StartsWith("protected:", StringComparison.Ordinal) ? value.Substring(10) : value;
  }
}

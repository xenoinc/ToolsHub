namespace Xeno.ToolsHub.Tests.SystemTests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Models;
using Xeno.ToolsHub.SidebarAddin.Services;
using Xeno.ToolsHub.SidebarAddin.ViewModels;
using Xeno.ToolsHub.SidebarAddin.Views;

[TestClass]
[DoNotParallelize]
public class SidebarLauncherTests
{
  [TestInitialize]
  public void Initialize() => TestHelpers.PrepareTestsFolder();

  [TestCleanup]
  public void Cleanup() => TestHelpers.CleanTestsFolder();

  [TestMethod]
  public void SidebarListPersistsItsOwnOrderWithoutChangingHostShortcuts()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    settings.SetValue("ToolsHub.Shortcuts", "ShortcutItems", "untouched");
    settings.SaveFile();
    string original = File.ReadAllText(settings.SettingsFilePath);
    var store = new JsonLauncherStore(settings);
    var model = new ToolbarViewModel(store, new FakeShell(), new ApplicationSettings(settings));
    Assert.IsTrue(model.AddPaths(new[] { "one.lnk", "two.exe", "three.url" }, 0));
    Assert.IsTrue(model.AddPaths(new[] { "TWO.EXE" }, 1));
    Assert.HasCount(3, model.Items);
    Assert.IsTrue(model.Move(model.Items[0], 3));
    Assert.IsTrue(model.Move(model.Items[1], 0));
    CollectionAssert.AreEqual(new[] { "three.url", "two.exe", "one.lnk" }, store.Load().Select(item => item.Target).ToArray());
    var reloaded = new ToolbarViewModel(new JsonLauncherStore(settings), new FakeShell(), new ApplicationSettings(settings));
    Assert.IsTrue(reloaded.Remove(reloaded.Items[1]));
    CollectionAssert.AreEqual(new[] { "three.url", "one.lnk" }, store.Load().Select(item => item.Target).ToArray());
    Assert.AreEqual(original, File.ReadAllText(settings.SettingsFilePath));
  }

  [TestMethod]
  public void SaveFailureKeepsItemsAndCorruptFileIsNotOverwritten()
  {
    var settings = new ApplicationSettings(new SettingsManager(StorageMethod.UnitTest));
    var store = new MemoryStore();
    var model = new ToolbarViewModel(store, new FakeShell(), settings);
    Assert.IsTrue(model.AddPaths(new[] { "one.exe", "two.exe" }, 0));
    var original = model.Items[0];
    store.FailSave = true;
    Assert.IsFalse(model.Move(original, 2));
    Assert.AreSame(original, model.Items[0]);
    Assert.IsFalse(model.Remove(original));
    Assert.IsFalse(model.AddPaths(new[] { "three.exe" }, 1));
    Assert.HasCount(2, model.Items);
    Assert.IsFalse(string.IsNullOrEmpty(model.Error));

    string path = Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "corrupt.json");
    File.WriteAllText(path, "{broken");
    var corrupt = new ToolbarViewModel(new JsonLauncherStore(path), new FakeShell(), settings);
    Assert.IsFalse(corrupt.AddPaths(new[] { "new.exe" }, 0));
    Assert.AreEqual("{broken", File.ReadAllText(path));
  }

  [TestMethod]
  public void IconClickLaunchesAndFailedLaunchReportsAnError()
  {
    var shell = new FakeShell();
    var model = new ToolbarViewModel(new MemoryStore(), shell, new ApplicationSettings(new SettingsManager(StorageMethod.UnitTest)));
    Assert.IsTrue(model.AddPaths(new[] { "one.lnk" }, 0));
    model.Launch(model.Items[0]);
    Assert.AreEqual("one.lnk", shell.Launched.Target);
    shell.FailLaunch = true;
    model.Launch(model.Items[0]);
    StringAssert.Contains(model.Error, "Unable to launch");
  }

  [TestMethod]
  public void NativeShellImportsLinksAndProvidesExecutableAndFallbackIcons()
  {
    RunSta(() =>
    {
      var shell = new WindowsLauncherShell();
      string exe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");
      var item = shell.Import(exe);
      Assert.AreEqual(Path.GetFullPath(exe), item.Target);
      Assert.IsNotNull(shell.GetIcon(item.Target));
      Assert.IsNotNull(shell.GetIcon(Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "missing.exe")));
      var start = WindowsLauncherShell.CreateStartInfo(item);
      Assert.IsTrue(start.UseShellExecute);
      Assert.AreEqual(Path.GetDirectoryName(exe), start.WorkingDirectory);
      string link = Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "sample.lnk");
      File.WriteAllText(link, "test link placeholder");
      var linkStart = WindowsLauncherShell.CreateStartInfo(shell.Import(link));
      Assert.AreEqual(link, linkStart.FileName);
      Assert.AreEqual(string.Empty, linkStart.WorkingDirectory);
      Assert.ThrowsExactly<FileNotFoundException>(() => shell.Import(link + ".missing"));
      var web = shell.Import("https://example.com/app");
      Assert.AreEqual("example.com", web.Title);
      Assert.AreEqual("https://example.com/app", WindowsLauncherShell.CreateStartInfo(web).FileName);
    });
  }

  [TestMethod]
  [DataRow(1, -1920.0, 460.0)]
  [DataRow(2, -80.0, 460.0)]
  [DataRow(3, -1000.0, 40.0)]
  [DataRow(4, -1000.0, 880.0)]
  public void DockBoundsRespectEachEdgeAndNegativeMonitorCoordinates(int edge, double x, double y)
  {
    var area = new Rect(-1920, 40, 1920, 1040);
    var bounds = DockLayout.Calculate(area, edge, 80, 200);
    Assert.AreEqual(x, bounds.X);
    Assert.AreEqual(y, bounds.Y);
    Assert.IsTrue(area.Contains(bounds));
    Assert.AreEqual(area, DockLayout.Calculate(area, edge, 10000, 10000));
  }

  [TestMethod]
  public void ToolbarBindsIconsAndHandlesButtonClick()
  {
    RunSta(() =>
    {
      var settings = new SettingsManager(StorageMethod.UnitTest);
      settings.SetValue("Sidebar", "ScreenArea", "3");
      var shell = new FakeShell();
      var model = new ToolbarViewModel(new MemoryStore(), shell, new ApplicationSettings(settings));
      Assert.AreEqual(Orientation.Horizontal, model.Orientation);
      var view = new ToolbarView(model);
      try
      {
        Assert.IsTrue(model.AddPaths(new[] { "one.exe", "two.lnk" }, 0));
        Assert.HasCount(2, model.Items);
        var launchers = (ItemsControl)view.FindName("Launchers");
        view.Show();
        view.Dispatcher.Invoke(new Action(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        view.UpdateLayout();
        Assert.AreEqual(2, launchers.Items.Count);
        var presenter = (ContentPresenter)launchers.ItemContainerGenerator.ContainerFromIndex(0);
        presenter.ApplyTemplate();
        var button = FindVisual<Button>(presenter);
        Assert.IsNotNull(button);
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.AreEqual("one.exe", shell.Launched.Target);
        Assert.IsTrue(model.Move(model.Items[1], 0));
        Assert.AreEqual("two.lnk", model.Items[0].Item.Target);
        Assert.AreEqual("one.exe", shell.Launched.Target);
      }
      finally
      {
        view.Close();
      }
    });
  }

  private static T FindVisual<T>(DependencyObject parent)
    where T : DependencyObject
  {
    for (int index = 0; index < VisualTreeHelper.GetChildrenCount(parent); index++)
    {
      var child = VisualTreeHelper.GetChild(parent, index);
      if (child is T found)
        return found;
      var nested = FindVisual<T>(child);
      if (nested != null)
        return nested;
    }

    return null;
  }

  private static void RunSta(Action action)
  {
    Exception error = null;
    var thread = new Thread(() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        error = ex;
      }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    Assert.IsTrue(thread.Join(TimeSpan.FromSeconds(20)), "Sidebar UI test did not finish.");
    if (error != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error).Throw();
  }

  private sealed class FakeShell : ILauncherShell
  {
    public DockLauncher Launched { get; private set; }

    public bool FailLaunch { get; set; }

    public DockLauncher Import(string path) => new() { Title = path, Target = path };

    public ImageSource GetIcon(string target) => null;

    public void Launch(DockLauncher item)
    {
      if (FailLaunch)
        throw new IOException("Test launch failure");
      Launched = item;
    }
  }

  private sealed class MemoryStore : ILauncherStore
  {
    private IReadOnlyList<DockLauncher> _items = Array.Empty<DockLauncher>();

    public bool FailSave { get; set; }

    public IReadOnlyList<DockLauncher> Load() => _items;

    public void Save(IReadOnlyList<DockLauncher> items)
    {
      if (FailSave)
        throw new IOException("Test save failure");
      _items = items.ToList();
    }
  }
}

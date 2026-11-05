namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.SidebarAddin.Domain;
using Xeno.Dockzi.SidebarAddin.Models;
using Xeno.Dockzi.SidebarAddin.Services;
using Xeno.Dockzi.SidebarAddin.ViewModels;
using Xeno.Dockzi.SidebarAddin.Views;

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
    settings.SetValue("ShortcutsAddin", "ShortcutItems", "untouched");
    settings.SaveFile();
    var store = new SettingsLauncherStore(settings);
    var model = new ToolbarViewModel(store, new FakeShell(), new ApplicationSettings(settings));
    Assert.IsTrue(model.AddPaths(new[] { "one.lnk", "two.exe", "three.url" }, 0));
    Assert.IsTrue(model.AddPaths(new[] { "TWO.EXE" }, 1));
    Assert.HasCount(3, model.Items);
    Assert.IsTrue(model.Move(model.Items[0], 3));
    Assert.IsTrue(model.Move(model.Items[1], 0));
    CollectionAssert.AreEqual(new[] { "three.url", "two.exe", "one.lnk" }, store.Load().Select(item => item.Target).ToArray());
    var loadedSettings = new SettingsManager(StorageMethod.UnitTest);
    loadedSettings.LoadFile();
    var reloaded = new ToolbarViewModel(new SettingsLauncherStore(loadedSettings), new FakeShell(), new ApplicationSettings(loadedSettings));
    Assert.IsTrue(reloaded.Remove(reloaded.Items[1]));
    settings.LoadFile();
    CollectionAssert.AreEqual(new[] { "three.url", "one.lnk" }, store.Load().Select(item => item.Target).ToArray());
    Assert.AreEqual("untouched", settings.GetValue("ShortcutsAddin", "ShortcutItems"));
    Assert.IsFalse(File.Exists(Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json")));
  }

  [TestMethod]
  public void LegacySidebarLaunchersImportOnceAndAnEmptySavedListDoesNotReimport()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    settings.SetValue("Sidebar", "ScreenArea", "3");
    settings.SetValue("OtherAddin", "Secret", "opaque-value");
    string legacy = Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json");
    const string json = "[{\"Id\":\"legacy-id\",\"Title\":\"Existing launcher\",\"Target\":\"app.exe\"}]";
    File.WriteAllText(legacy, json);
    var store = new SettingsLauncherStore(settings);
    Assert.AreEqual("legacy-id", store.Load()[0].Id);
    var loaded = new SettingsManager(StorageMethod.UnitTest);
    loaded.LoadFile();
    Assert.AreEqual("app.exe", new SettingsLauncherStore(loaded).Load()[0].Target);
    Assert.AreEqual("3", loaded.GetValue("Sidebar", "ScreenArea"));
    Assert.AreEqual("opaque-value", loaded.GetValue("OtherAddin", "Secret"));
    Assert.AreEqual(json, File.ReadAllText(legacy));
    store.Save(Array.Empty<DockLauncher>());
    loaded.LoadFile();
    Assert.HasCount(0, new SettingsLauncherStore(loaded).Load());
  }

  [TestMethod]
  public void DatabaseLaunchersTakePrecedenceAndFailedSaveRestoresPreviousData()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    var store = new SettingsLauncherStore(settings);
    var model = new ToolbarViewModel(store, new FakeShell(), new ApplicationSettings(settings));
    Assert.IsTrue(model.AddPaths(new[] { "saved.exe" }, 0));
    string legacy = Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json");
    File.WriteAllText(legacy, "{broken");
    Assert.AreEqual("saved.exe", store.Load()[0].Target);
    string original = File.ReadAllText(settings.SettingsFilePath);
    var attributes = File.GetAttributes(settings.SettingsFilePath);
    try
    {
      File.SetAttributes(settings.SettingsFilePath, attributes | FileAttributes.ReadOnly);
      Assert.IsFalse(model.AddPaths(new[] { "new.exe" }, 1));
      Assert.HasCount(1, model.Items);
      Assert.AreEqual("saved.exe", store.Load()[0].Target);
      Assert.AreEqual(original, File.ReadAllText(settings.SettingsFilePath));
    }
    finally
    {
      File.SetAttributes(settings.SettingsFilePath, attributes);
    }

    Assert.HasCount(0, Directory.GetFiles(Path.GetDirectoryName(settings.SettingsFilePath), ".Dockzi-*.tmp"));
  }

  [TestMethod]
  public void InvalidDatabaseLaunchersArePreservedInsteadOfUsingLegacyData()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    settings.SetValue("Sidebar", "LauncherItems", "invalid-base64!");
    settings.SaveFile();
    string original = File.ReadAllText(settings.SettingsFilePath);
    string legacy = Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json");
    File.WriteAllText(legacy, "[]");
    var model = new ToolbarViewModel(new SettingsLauncherStore(settings), new FakeShell(), new ApplicationSettings(settings));
    Assert.IsFalse(model.AddPaths(new[] { "new.exe" }, 0));
    Assert.IsFalse(string.IsNullOrEmpty(model.Error));
    Assert.AreEqual(original, File.ReadAllText(settings.SettingsFilePath));
    Assert.AreEqual("[]", File.ReadAllText(legacy));
  }

  [TestMethod]
  public void ExplorerDropsBecomeSavedClickableLaunchersAndRespectSourceEffects()
  {
    RunSta(() =>
    {
      var shell = new FakeShell();
      var store = new MemoryStore();
      var model = new ToolbarViewModel(store, shell, new ApplicationSettings(new SettingsManager(StorageMethod.UnitTest)));
      var drops = new LauncherDropHandler(model);
      var data = new DataObject(DataFormats.FileDrop, new[] { "application.exe", "desktop.lnk" });
      Assert.AreEqual(DragDropEffects.Copy, drops.GetEffect(data, DragDropEffects.Copy | DragDropEffects.Move));
      Assert.AreEqual(DragDropEffects.Link, drops.GetEffect(data, DragDropEffects.Link));
      Assert.AreEqual(DragDropEffects.None, drops.GetEffect(data, DragDropEffects.Move));
      Assert.IsTrue(drops.Drop(data, 0));
      Assert.HasCount(2, store.Load());
      Assert.IsNull(shell.Launched);
      model.Launch(model.Items[1]);
      Assert.AreEqual("desktop.lnk", shell.Launched.Target);
      var reorder = new DataObject(LauncherDropHandler.ItemDragFormat, model.Items[1]);
      Assert.AreEqual(DragDropEffects.Move, drops.GetEffect(reorder, DragDropEffects.Move));
      Assert.IsTrue(drops.Drop(reorder, 0));
      Assert.AreEqual("desktop.lnk", store.Load()[0].Target);
      var unsupported = new DataObject(DataFormats.Text, "This is not an app or a URL");
      Assert.AreEqual(DragDropEffects.None, drops.GetEffect(unsupported, DragDropEffects.All));
      Assert.IsFalse(drops.Drop(unsupported, 0));
      var web = new DataObject(DataFormats.Text, "https://example.com/app");
      Assert.IsTrue(drops.Drop(web, 2));
      Assert.AreEqual("https://example.com/app", store.Load()[2].Target);
    });
  }

  [TestMethod]
  public void AutoHideWaitsThenSlidesAndReversesWhenHovered()
  {
    var hide = new DockAutoHide();
    for (int tick = 0; tick < 6; tick++)
      hide.Tick(0.1, true, false);
    Assert.AreEqual(0, hide.Progress);
    hide.Tick(0.05, true, false);
    Assert.IsTrue(hide.Progress > 0 && hide.Progress < 1);
    double sliding = hide.Progress;
    hide.Tick(0.02, true, true);
    Assert.IsTrue(hide.Progress < sliding);
    for (int tick = 0; tick < 10; tick++)
      hide.Tick(0.1, true, false);
    Assert.AreEqual(1, hide.Progress);
    for (int tick = 0; tick < 3; tick++)
      hide.Tick(0.1, false, false);
    Assert.AreEqual(0, hide.Progress);
  }

  [TestMethod]
  [DataRow(1)]
  [DataRow(2)]
  [DataRow(3)]
  [DataRow(4)]
  public void HiddenDockKeepsOnlyAnEdgeStripAndNeverPaintsOnAdjacentMonitor(int edge)
  {
    var size = edge >= 3 ? new Size(272, 80) : new Size(80, 272);
    var viewport = new Rect(size);
    for (int frame = 0; frame <= 100; frame++)
    {
      var visible = DockAutoHide.VisibleArea(size, edge, frame / 100.0);
      Assert.IsTrue(viewport.Contains(visible));
    }

    var strip = DockAutoHide.VisibleArea(size, edge, 1);
    Assert.AreEqual(3, edge >= 3 ? strip.Height : strip.Width);
    if (edge == 1)
      Assert.AreEqual(0, strip.Left);
    else if (edge == 2)
      Assert.AreEqual(size.Width, strip.Right);
    else if (edge == 3)
      Assert.AreEqual(0, strip.Top);
    else
      Assert.AreEqual(size.Height, strip.Bottom);
  }

  [TestMethod]
  public void RunningIndicatorsFollowOpenWindowsForExecutableAndLinkTargets()
  {
    var running = new FakeRunningApplications();
    var model = new ToolbarViewModel(new MemoryStore(), new FakeShell(),
      new ApplicationSettings(new SettingsManager(StorageMethod.UnitTest)), running);
    model.AddPaths(new[] { "one.exe", "one.lnk", "two.exe" }, 0);
    int changes = 0;
    model.Items[0].PropertyChanged += (_, _) => changes++;
    running.Open.Add("one.exe");
    model.RefreshRunningState();
    Assert.IsTrue(model.Items[0].IsRunning);
    Assert.IsTrue(model.Items[1].IsRunning);
    Assert.IsFalse(model.Items[2].IsRunning);
    model.RefreshRunningState();
    Assert.AreEqual(1, changes);
    running.Open.Clear();
    model.RefreshRunningState();
    Assert.IsFalse(model.Items[0].IsRunning);
    Assert.IsFalse(model.Items[1].IsRunning);
  }

  [TestMethod]
  public void NativeRunningApplicationsRecognizesAVisibleWindow()
  {
    RunSta(() =>
    {
      var view = new Window { Title = "Sidebar running indicator test", Width = 100, Height = 80, ShowActivated = false };
      try
      {
        view.Show();
        var running = new WindowsRunningApplications();
        Assert.IsTrue(running.GetOpenExecutables().Contains(Environment.ProcessPath));
        string link = Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "running-app.lnk");
        object shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
        object shortcut = null;
        try
        {
          shortcut = ((dynamic)shell).CreateShortcut(link);
          ((dynamic)shortcut).TargetPath = Environment.ProcessPath;
          ((dynamic)shortcut).Save();
          Assert.AreEqual(Environment.ProcessPath, running.ResolveExecutable(link));
          Assert.IsTrue(running.GetOpenExecutables().Contains(running.ResolveExecutable(link)));
        }
        finally
        {
          if (shortcut != null)
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
          System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
        }
      }
      finally
      {
        view.Close();
      }
    });
  }

  [TestMethod]
  public void NativeAutoHideClipsItsInputRegionAndRestoresTheSameWindow()
  {
    RunSta(() =>
    {
      var screen = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
      int edge = System.Windows.Forms.Cursor.Position.X < screen.Left + (screen.Width / 2) ? 2 : 1;
      var settings = new SettingsManager(StorageMethod.UnitTest);
      settings.SetValue("Sidebar", "ScreenArea", edge.ToString());
      var model = new ToolbarViewModel(new MemoryStore(), new FakeShell(), new ApplicationSettings(settings));
      var view = new ToolbarView(model);
      IntPtr region = CreateRectRgn(0, 0, 0, 0);
      try
      {
        view.Show();
        var frame = new System.Windows.Threading.DispatcherFrame();
        var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(1.4) };
        timer.Tick += (_, _) =>
        {
          timer.Stop();
          frame.Continue = false;
        };
        timer.Start();
        System.Windows.Threading.Dispatcher.PushFrame(frame);
        Assert.AreEqual(1, view.HiddenProgress, 0.001);
        IntPtr handle = new System.Windows.Interop.WindowInteropHelper(view).Handle;
        Assert.IsTrue(GetWindowRect(handle, out var bounds));
        Assert.IsTrue(bounds.Left >= screen.Left && bounds.Right <= screen.Right);
        Assert.IsTrue(bounds.Top >= screen.Top && bounds.Bottom <= screen.Bottom);
        Assert.AreNotEqual(0, GetWindowRgn(handle, region));
        GetRgnBox(region, out var hidden);
        double dpi = VisualTreeHelper.GetDpi(view).DpiScaleX;
        Assert.IsTrue(hidden.Right - hidden.Left <= Math.Ceiling(3 * dpi) + 1);
        view.ApplySettings(new SidebarAppearance(edge, 1, 90, true, false));
        Assert.AreEqual(0, view.HiddenProgress);
        Assert.AreNotEqual(0, GetWindowRgn(handle, region));
        GetRgnBox(region, out var shown);
        Assert.AreEqual(bounds.Right - bounds.Left, shown.Right - shown.Left, 1);
      }
      finally
      {
        view.Close();
        DeleteObject(region);
      }
    });
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

    var corruptSettings = new SettingsManager(StorageMethod.UnitTest);
    string path = Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "SidebarLaunchers.json");
    File.WriteAllText(path, "{broken");
    var corrupt = new ToolbarViewModel(new SettingsLauncherStore(corruptSettings), new FakeShell(), settings);
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
      var preferences = new FakePreferencesDialog();
      var view = new ToolbarView(model, preferences);
      try
      {
        Assert.IsTrue(model.AddPaths(new[] { "one.exe", "two.lnk" }, 0));
        Assert.HasCount(2, model.Items);
        var launchers = (ItemsControl)view.FindName("Launchers");
        view.Show();
        view.Dispatcher.Invoke(new Action(() => { }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        view.UpdateLayout();
        Assert.AreEqual(2, launchers.Items.Count);
        var addButton = (Button)view.FindName("AddLauncherButton");
        Assert.AreEqual(Visibility.Visible, addButton.Visibility);
        Assert.IsTrue(addButton.AllowDrop);
        Assert.AreEqual(Dock.Right, DockPanel.GetDock(addButton));
        var surface = (Border)view.FindName("DockSurface");
        var settingsMenu = surface.ContextMenu.Items.OfType<MenuItem>().Single(item => (string)item.Header == "Sidebar settings…");
        settingsMenu.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
        Assert.AreEqual("Sidebar", preferences.SelectedPage);
        var presenter = (ContentPresenter)launchers.ItemContainerGenerator.ContainerFromIndex(0);
        presenter.ApplyTemplate();
        var button = FindVisual<Button>(presenter);
        Assert.IsNotNull(button);
        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Assert.AreEqual("one.exe", shell.Launched.Target);
        view.ApplySettings(new SidebarAppearance(2, 1, 60, false, false));
        view.UpdateLayout();
        Assert.AreEqual(Orientation.Vertical, FindVisual<StackPanel>(launchers).Orientation);
        Assert.AreEqual(0.6, view.Opacity, 0.001);
        Assert.IsFalse(view.Topmost);
        Assert.IsFalse(model.AutoHide);
        Assert.AreEqual(Dock.Bottom, DockPanel.GetDock(addButton));
        Assert.AreEqual(Visibility.Visible, addButton.Visibility);
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

  [System.Runtime.InteropServices.DllImport("user32.dll")]
  [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
  private static extern bool GetWindowRect(IntPtr window, out NativeRectangle rectangle);

  [System.Runtime.InteropServices.DllImport("user32.dll")]
  private static extern int GetWindowRgn(IntPtr window, IntPtr region);

  [System.Runtime.InteropServices.DllImport("gdi32.dll")]
  private static extern IntPtr CreateRectRgn(int left, int top, int right, int bottom);

  [System.Runtime.InteropServices.DllImport("gdi32.dll")]
  private static extern int GetRgnBox(IntPtr region, out NativeRectangle rectangle);

  [System.Runtime.InteropServices.DllImport("gdi32.dll")]
  [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
  private static extern bool DeleteObject(IntPtr value);

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

  [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
  private struct NativeRectangle
  {
    public int Left;

    public int Top;

    public int Right;

    public int Bottom;
  }

  private sealed class FakePreferencesDialog : IPreferencesDialogService
  {
    public string SelectedPage { get; private set; }

    public void Show() => SelectedPage = "General";

    public void Show(string pageTitle) => SelectedPage = pageTitle;
  }

  private sealed class FakeRunningApplications : IRunningApplications
  {
    public HashSet<string> Open { get; } = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlySet<string> GetOpenExecutables() => Open;

    public string ResolveExecutable(string target) => target == "one.lnk" ? "one.exe" : target;
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

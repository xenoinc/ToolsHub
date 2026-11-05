namespace Xeno.Dockzi.Tests.SystemTests.Shortcuts
{
  using System;
  using System.Threading;
  using System.Windows.Forms;
  using System.Text.Json;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.Dockzi.Config;
  using Xeno.Dockzi.LocalAddins.Shortcuts;
  using Xeno.Dockzi.LocalAddins.Shortcuts.Prefs;
  using Xeno.Dockzi.Managers;

  [TestClass]
  [DoNotParallelize]
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public class ShortcutsPreferencesTests
  {
    [TestInitialize]
    public void Initialize()
    {
      TestHelpers.PrepareTestsFolder();
    }

    [TestCleanup]
    public void Cleanup()
    {
      TestHelpers.CleanTestsFolder();
    }

    [TestMethod]
    public void GeneratedSamplesLoadWithoutChangingCacheOrLosingRows()
    {
      RunSta(() =>
      {
        var settings = new SettingsManager(StorageMethod.UnitTest);
        var generator = new ShortcutsManager(settings);
        generator.OnGenerateSampleShortcuts(string.Empty);
        var reloadedSettings = new SettingsManager(StorageMethod.UnitTest);
        reloadedSettings.LoadFile();
        var manager = new ShortcutsManager(reloadedSettings);
        var originalCache = manager.ShortcutItems;
        using var page = new TestPage(manager);

        page.LoadPage();

        var list = Find<ListView>(page, "LvShortcuts");
        Assert.AreEqual(2, list.Items.Count);
        Assert.AreSame(originalCache, manager.ShortcutItems);
        Assert.AreEqual(2, originalCache.Count);
        Assert.AreEqual("C-Drive", list.Items[0].SubItems[1].Text);
        Assert.AreEqual("D-Drive", list.Items[1].SubItems[1].Text);
        Assert.AreEqual(2, JsonSerializer.Deserialize<ShortcutItems>(Find<TextBox>(page, "TxtRawFile").Text).Count);
        Assert.IsFalse(page.IsModified);
      });
    }

    [TestMethod]
    public void SelectingAndEditingRowsPreservesOtherFieldsAndSavesAllItems()
    {
      RunSta(() =>
      {
        var settings = new SettingsManager(StorageMethod.UnitTest);
        settings.SetObject(ShortcutsManager.ShortcutsAddinId, ShortcutsManager.ShortcutItemsKey, new ShortcutItems
        {
          new ShortcutItem("First", "first.exe", "--first"),
          new ShortcutItem("Second", "second.exe", "--second") { RunAsAdmin = true },
        });
        var manager = new ShortcutsManager(settings);
        using var page = new TestPage(manager);
        page.LoadPage();
        var list = Find<ListView>(page, "LvShortcuts");
        _ = list.Handle;
        list.Items[0].Selected = true;
        list.Items[0].Selected = false;
        list.Items[1].Selected = true;
        Assert.AreEqual("second.exe", Find<TextBox>(page, "TxtPath").Text);
        Assert.AreEqual("--second", Find<TextBox>(page, "TxtPathArgs").Text);
        Assert.IsFalse(page.IsModified);
        Find<TextBox>(page, "TxtTitle").Text = "Renamed";
        Assert.IsTrue(page.IsModified);
        Assert.AreEqual(2, manager.ShortcutItems.Count);
        Assert.IsTrue(page.OnSave());
        Assert.IsFalse(page.IsModified);

        var persisted = new SettingsManager(StorageMethod.UnitTest);
        persisted.LoadFile();
        var saved = persisted.GetObject<ShortcutItems>(ShortcutsManager.ShortcutsAddinId, ShortcutsManager.ShortcutItemsKey);
        Assert.AreEqual(2, saved.Count);
        Assert.AreEqual("First", saved[0].Title);
        Assert.AreEqual("--first", saved[0].Params);
        Assert.AreEqual("Renamed", saved[1].Title);
        Assert.AreEqual("second.exe", saved[1].Target);
        Assert.AreEqual("--second", saved[1].Params);
        Assert.IsTrue(saved[1].RunAsAdmin);
      });
    }

    [TestMethod]
    public void InvalidRowDoesNotOverwriteSavedShortcutsOrClearModifiedFlag()
    {
      RunSta(() =>
      {
        var settings = new SettingsManager(StorageMethod.UnitTest);
        var manager = new ShortcutsManager(settings);
        manager.OnGenerateSampleShortcuts(string.Empty);
        using var page = new TestPage(manager);
        page.LoadPage();
        var list = Find<ListView>(page, "LvShortcuts");
        _ = list.Handle;
        list.Items[0].Selected = true;
        Find<TextBox>(page, "TxtTitle").Text = " ";
        Assert.IsFalse(page.OnSave());
        Assert.IsTrue(page.IsModified);
        var saved = settings.GetObject<ShortcutItems>(ShortcutsManager.ShortcutsAddinId, ShortcutsManager.ShortcutItemsKey);
        Assert.AreEqual(2, saved.Count);
        Assert.AreEqual("C-Drive", saved[0].Title);
      });
    }

    [TestMethod]
    public void EmptyShortcutsLoadNormally()
    {
      RunSta(() =>
      {
        var manager = new ShortcutsManager(new SettingsManager(StorageMethod.UnitTest));
        using var page = new TestPage(manager);
        page.LoadPage();
        Assert.AreEqual(0, Find<ListView>(page, "LvShortcuts").Items.Count);
        Assert.AreEqual(0, manager.ShortcutItems.Count);
        Assert.IsFalse(page.IsModified);
      });
    }

    private static T Find<T>(Control parent, string name)
      where T : Control
    {
      return (T)parent.Controls.Find(name, true)[0];
    }

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

    private sealed class TestPage : ShortcutsPreferences
    {
      public TestPage(ShortcutsManager shortcuts)
        : base(shortcuts)
      {
      }

      public void LoadPage() => OnLoad(EventArgs.Empty);
    }
  }
}

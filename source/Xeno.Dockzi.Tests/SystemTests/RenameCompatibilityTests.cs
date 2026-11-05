namespace Xeno.Dockzi.Tests.SystemTests;

using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.LocalAddins.Shortcuts;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.PomodoroAddin.Managers;
using Xeno.Dockzi.Services;

[TestClass]
[DoNotParallelize]
public class RenameCompatibilityTests
{
  [TestInitialize]
  public void Initialize() => TestHelpers.PrepareTestsFolder();

  [TestCleanup]
  public void Cleanup() => TestHelpers.CleanTestsFolder();

  [TestMethod]
  [DataRow("ToolsHub.db")]
  [DataRow("Dockzi.db")]
  public void LegacyDatabaseImportsWithoutModifyingOriginalAndDockziDataTakesPrecedence(string legacyFile)
  {
    var legacy = new SettingsManager(StorageMethod.UnitTest);
    legacy.SetValue("Sidebar", "ScreenArea", "2");
    legacy.SetObject("ShortcutsAddin", "ShortcutItems", new ShortcutItems { new("Existing shortcut", "app.exe", "--test") });
    string original = legacy.ToString;
    string legacyPath = Path.Combine(Path.GetDirectoryName(legacy.SettingsFilePath), legacyFile);
    File.WriteAllText(legacyPath, original);
    var imported = SettingsLoader.Load(StorageMethod.UnitTest);
    Assert.IsTrue(File.Exists(imported.SettingsFilePath));
    Assert.AreEqual("Dockzi.config", Path.GetFileName(imported.SettingsFilePath));
    Assert.AreEqual("2", imported.GetValue("Sidebar", "ScreenArea"));
    Assert.AreEqual("--test", imported.GetObject<ShortcutItems>("ShortcutsAddin", "ShortcutItems")[0].Params);
    Assert.AreEqual(original, File.ReadAllText(legacyPath));
    imported.SetValue("Sidebar", "ScreenArea", "3");
    imported.SaveFileOrThrow();
    Assert.AreEqual("3", SettingsLoader.Load(StorageMethod.UnitTest).GetValue("Sidebar", "ScreenArea"));
    Assert.AreEqual(original, File.ReadAllText(legacyPath));
  }

  [TestMethod]
  public void InterimDockziDatabaseTakesPrecedenceOverToolsHubDatabase()
  {
    string directory = Helpers.GetStorageFolder(StorageMethod.UnitTest);
    var legacy = new SettingsManager(StorageMethod.UnitTest);
    legacy.SetValue("Sidebar", "ScreenArea", "1");
    File.WriteAllText(Path.Combine(directory, "ToolsHub.db"), legacy.ToString);
    legacy.SetValue("Sidebar", "ScreenArea", "3");
    File.WriteAllText(Path.Combine(directory, "Dockzi.db"), legacy.ToString);
    Assert.AreEqual("3", SettingsLoader.Load(StorageMethod.UnitTest).GetValue("Sidebar", "ScreenArea"));
  }

  [TestMethod]
  public void RenamedAssembliesKeepEmbeddedManifestsAndPomodoroSound()
  {
    var host = typeof(MainHandler).Assembly;
    Assert.AreEqual("Dockzi", host.GetName().Name);
    Assert.IsTrue(host.GetManifestResourceNames().Contains("Xeno.Dockzi.Dockzi.addin.xml"));
    Assert.IsTrue(host.GetManifestResourceNames().Contains("Xeno.Dockzi.LocalAddins.Shortcuts.Shortcuts.addin"));
    Assert.IsTrue(typeof(PomodoroManager).Assembly.GetManifestResourceNames().Contains("Xeno.Dockzi.PomodoroAddin.chime2.wav"));
  }
}

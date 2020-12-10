/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    ShortcutsTests.cs
 * Description:
 *  Shortcut Manager Unit Tests
 */

namespace Xeno.ToolsHub.Tests.SystemTests.Shortcuts
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.LocalAddins.Shortcuts;
  using Xeno.ToolsHub.Managers;
  using Xeno.ToolsHub.Services.Logging;

  /// <summary>Shortcut Manager Unit Tests.</summary>
  [TestClass]
  public class ShortcutsTests
  {
    private SettingsManager _settings = new SettingsManager(Config.StorageMethod.UnitTest);

    [TestInitialize]
    public void Initialize()
    {
      TestHelpers.PrepareTestsFolder();
      _settings.Clear();
    }

    [TestCleanup]
    public void Cleanup()
    {
      _settings.Clear();
      TestHelpers.CleanTestsFolder();
    }

    [TestMethod]
    public void LoadMissingShortcutsTest()
    {
      _settings.Clear();
      ShortcutItems items = _settings.GetObject<ShortcutItems>("RandomCrap", "InvalidKey");

      Assert.AreEqual(items, null);
    }

    [TestMethod]
    public void SaveJsonInJsonTest()
    {
      _settings.Clear();

      _settings.SetValue("MyTitle", "MyKey", "MyValue");
      string testValue = _settings.GetValue("MyTitle", "MyKey", string.Empty);
      Assert.AreEqual(testValue, "MyValue");

      _settings.SaveFile();

      // Strap-on a new JSON element
      var shortcutItems = Helpers.SettingsHelpers.GenerateShortcutsMixed();
      string json = JsonConvert.SerializeObject(shortcutItems, Formatting.None);

      _settings.SetValue("MyJsonTest", "TestShortcuts", json);
      _settings.SaveFile();

      string storedJson = _settings.GetValue("MyJsonTest", "TestShortcuts", string.Empty);
      Assert.AreEqual(json, storedJson);

      // Log.Debug(storedJson);
      Log.Debug("BEFORE:" + System.Environment.NewLine + _settings.ToString);

      // Clean the slate and re-load
      _settings.SaveFile();
      _settings.Clear();
      _settings.LoadFile();

      ShortcutItems items2 = _settings.GetObject<ShortcutItems>("MyJsonTest", "TestShortcuts");
      Log.Debug(_settings.ToString);

      Assert.IsNotNull(items2);
      Assert.AreEqual(items2.Count, 6);
    }
  }
}

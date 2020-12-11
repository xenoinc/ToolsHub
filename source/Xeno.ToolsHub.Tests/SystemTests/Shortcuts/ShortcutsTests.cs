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
    public void SaveValueTest()
    {
      const string TestProperty = "MyTitle";
      const string TestKey1 = "MyKey";
      const string TestKey2 = "MyOtherKey";
      const string TestValue1 = "MyValue";
      const string TestValue2 = "ASDFjkl;";

      // Get value in-memory
      _settings.SetValue(TestProperty, TestKey1, TestValue1);
      string ret = _settings.GetValue(TestProperty, TestKey1, string.Empty);

      Assert.AreEqual(ret, TestValue1);

      // Clean the slate and re-load
      _settings.SaveFile();
      _settings.Clear();
      _settings.LoadFile();

      // Now add something else.
      _settings.SetValue(TestProperty, TestKey2, TestValue2);

      ret = _settings.GetValue(TestProperty, TestKey1, string.Empty);
      string ret2 = _settings.GetValue(TestProperty, TestKey2, string.Empty);

      Assert.AreEqual(TestValue1, ret, "Invalid first setting value.");
      Assert.AreEqual(TestValue2, ret2, "Invalid second setting value.");
    }

    /// <summary>Settings objects get saved as Base64, not plain-text.</summary>
    [TestMethod]
    public void SaveObjectTest()
    {
      // Create object to be saved as Base64
      ShortcutItems shortcutItems = Helpers.SettingsHelpers.GenerateShortcutsMixed();
      _settings.SetObject("MyJsonTest", "TestShortcuts", shortcutItems);

      // Clean the slate and re-load
      _settings.SaveFile();
      _settings.Clear();
      _settings.LoadFile();

      ShortcutItems items2 = _settings.GetObject<ShortcutItems>("MyJsonTest", "TestShortcuts");
      Log.Debug(_settings.ToString);

      Assert.IsNotNull(items2, "Failed to get test Shortcut Items");
      Assert.AreEqual(items2.Count, 6, "Missing ShortcutItems");
    }
  }
}

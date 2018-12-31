/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    ShortcutsTests.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.Tests.SystemTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.LocalAddins.Shortcuts;
  using Xeno.ToolsHub.Managers;
  using Xeno.ToolsHub.Services.Logging;

  [TestClass]
  public class ShortcutsTests
  {
    [TestInitialize]
    public void Initialize()
    {
      TestHelpers.PrepareTestsFolder();
      Settings.Clear();
    }

    [TestCleanup]
    public void Cleanup()
    {
      Settings.Clear();
      TestHelpers.CleanTestsFolder();
    }

    [TestMethod]
    public void LoadMissingShortcutsTest()
    {
      Settings.Clear();
      ShortcutItems items = Settings.GetObject<ShortcutItems>("RandomCrap", "InvalidKey");

      Assert.AreEqual(items, null);
    }

    [TestMethod]
    public void SaveJsonInJsonTest()
    {
      Settings.Clear();

      Settings.SetValue("MyTitle", "MyKey", "MyValue");
      string testValue = Settings.GetValue("MyTitle", "MyKey", string.Empty);
      Assert.AreEqual(testValue, "MyValue");

      Settings.SaveFile();

      // Strap-on a new JSON element
      var items = new ShortcutItems
      {
         new ShortcutItem { Title = "FolderA", Target = @"C:\Test1" },
         new ShortcutItem { Title = "FolderB", Target = @"C:\Test2\Sub1" },
         new ShortcutItem { Title = "FolderC", Target = @"C:\Test3\Sub1\Sub2" },
         new ShortcutItem { Title = "Web1", Target = @"https://www.xenoinc.com/" },
         new ShortcutItem { Title = "-", Target = string.Empty },
         new ShortcutItem { Title = "Manage Shortcuts", Target = "SomePath" },
       };

      string json = JsonConvert.SerializeObject(items, Formatting.None);

      Settings.SetValue("MyJsonTest", "TestShortcuts", json);
      Settings.SaveFile();

      string storedJson = Settings.GetValue("MyJsonTest", "TestShortcuts", string.Empty);
      Assert.AreEqual(json, storedJson);

      // Log.Debug(storedJson);
      Log.Debug("BEFORE:" + System.Environment.NewLine + Settings.ToString);

      // Clean the slate and re-load
      Settings.SaveFile();
      Settings.Clear();
      Settings.LoadFile();

      ShortcutItems items2 = Settings.GetObject<ShortcutItems>("MyJsonTest", "TestShortcuts");
      Log.Debug(Settings.ToString);

      Assert.IsNotNull(items2);
      Assert.AreEqual(items2.Count, 6);
    }
  }
}

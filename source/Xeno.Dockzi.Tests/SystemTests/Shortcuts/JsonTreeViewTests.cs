/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    JsonTreeViewTests.cs
 * Description:
 *  Saved shortcuts to Json tests
 */

using System;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.LocalAddins.Shortcuts;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Tests.Helpers;

namespace Xeno.Dockzi.Tests.SystemTests.Shortcuts
{
  [TestClass]
  public class JsonTreeViewTests
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
    public void MakeJsonObject()
    {
      _settings.Clear();

      ShortcutItems items = SettingsHelpers.GenerateShortcutsMixed();
      Assert.AreNotEqual(0, items.Count);

      string json = JsonSerializer.Serialize(items, JsonOptions.Compact);
      Console.WriteLine("JSON: ---------" + Environment.NewLine + json + Environment.NewLine + "---------");

      var obj = JsonSerializer.Deserialize<ShortcutItems>(json);

      Assert.IsNotNull(obj);
      Assert.AreEqual(items.Count, obj.Count);
    }
  }
}

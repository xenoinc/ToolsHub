/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    JsonTreeViewTests.cs
 * Description:
 *  Saved shortcuts to Json tests
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Tests.Helpers;

namespace Xeno.ToolsHub.Tests.SystemTests.Shortcuts
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

      var items = SettingsHelpers.GenerateShortcutsMixed();
      Assert.AreNotEqual(0, items.Count);

      string json = JsonConvert.SerializeObject(items, Formatting.None);
      Console.WriteLine("JSON: ---------" + Environment.NewLine + json + Environment.NewLine + "---------");

      // Fails because we start with "[{" versus "{"
      // See also: https://stackoverflow.com/questions/17518886/c-sharp-parsing-json-error-reading-json-object
      var @object = JObject.Parse(json);

      Assert.AreNotSame(null, @object);
    }
  }
}

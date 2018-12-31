/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertiesManagerTests.cs
 * Description:
 *  Tests for the main property store
 */

namespace Xeno.ToolsHub.Tests.SystemTests
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.Models.PropertyService;
  using Xeno.ToolsHub.Services.Logging;
  using Xeno.ToolsHub.Tests.Helpers;

  [TestClass]
  public class PropertiesStoreTests
  {
    [TestInitialize]
    public void TestInit()
    {
      TestHelpers.PrepareTestsFolder();
    }

    [TestCleanup]
    public void TestCleanup()
    {
      TestHelpers.CleanTestsFolder();
    }

    [TestMethod]
    public void CreateSingleBagStoreTest()
    {
      var store = new PropertiesStore();

      var guid = System.Guid.NewGuid().ToString();
      store.Add(new Properties(guid)
      {
        Items = new Properties.ItemCollection
        {
          { "key1", "value1" },
          { "key2", "value2" }
        }
      });

      var bag = store.Find(guid);

      Assert.IsTrue(bag.Items.ContainsKey("key1"));
      Assert.IsTrue(bag.Items.ContainsKey("key2"));

      Assert.AreEqual(bag.Items["key1"], "value1");
      Assert.AreNotEqual(bag.Items["key2"], "value1");
    }

    [TestMethod]
    public void PostAddItemTest()
    {
      var store = PropertyHelpers.SamplePropertiesStore();
      var bag1 = store.Find("GUID1");
      var bagBad = store.Find("InvalidGUID");

      // Validate our objects
      Assert.IsNotNull(bag1);
      Assert.IsNull(bagBad);

      // Attempt to add after the fact
      bag1.Add("A-NewKey3", "A-NewValue3");

      // Output data
      var json = JsonConvert.SerializeObject(store, Formatting.Indented);
      Log.Debug(json);

      var tempBag1 = store.Find("GUID1");
      json = JsonConvert.SerializeObject(tempBag1, Formatting.Indented);
      Log.Debug(json);

      var value = tempBag1.Items["A-NewKey3"];
      Assert.IsNotNull(value);
      Assert.AreEqual("A-NewValue3", value);
    }

    [TestMethod]
    public void SaveClearAndLoadPropertiesFileTest()
    {
      Log.Debug($"Tmp Path: {Managers.Settings.SettingsFilePath}");

      var store = PropertyHelpers.SamplePropertiesStore();
      Managers.Settings.PropertiesStore = store;

      Managers.Settings.SaveFile();
      store.ClearAll();
      Assert.AreEqual(store.PropertyBags.Count, 0);

      Managers.Settings.LoadFile();
      Assert.AreNotEqual(store.PropertyBags.Count, 0);

      string json = JsonConvert.SerializeObject(store.PropertyBags, Formatting.Indented);
      Log.Debug(json);
    }

    [TestMethod]
    public void SaveJsonInJsonTest()
    {
      var store = PropertyHelpers.SamplePropertiesStore();
    }
  }
}

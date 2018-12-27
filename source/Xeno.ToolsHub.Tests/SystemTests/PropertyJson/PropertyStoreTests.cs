/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertyStore.cs
 * Description:
 *  Tests for the main property store
 */

namespace Xeno.ToolsHub.Tests.SystemTests.PropertyJson
{
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.Config;
  using Xeno.ToolsHub.Services.PropertyService;

  [TestClass]
  public class PropertyStoreTests
  {
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
      var store = PropertyHelpers.CreateStore();
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
    public void SavePropertiesFileTests()
    {
      var store = PropertyHelpers.CreateStore();
      store.SettingsFile = System.Guid.NewGuid().ToString();
      // make a path thingy here

    }
  }
}

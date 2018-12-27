/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    PropertyTests.cs
 * Description:
 *
 * Reference:
 *  - https://www.newtonsoft.com/json/help/html/LINQtoJSON.htm
 *  - https://stackoverflow.com/questions/22123619/replacing-only-a-certain-part-of-a-json-object
 */

namespace Xeno.ToolsHub.Tests.SystemTests.PropertyJson
{
  using System.Collections.Generic;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.Config;
  using Xeno.ToolsHub.Services.PropertyService;

  [TestClass]
  public class PropertyBagTests
  {
    [TestMethod]
    public void CanCreateBagsTest()
    {
      int numBags = 5;
      var bags = PropertyHelpers.CreateBags(numBags);
      Assert.AreEqual(bags.Count, numBags);
    }

    [TestMethod]
    public void Create1PropertyAndSerializationTest()
    {
      string guid = System.Guid.NewGuid().ToString();
      Properties bag1 = new Properties(guid)
      {
        Items = new Properties.ItemCollection { { "key1", "value" } }
      };

      string json = JsonConvert.SerializeObject(bag1, Formatting.Indented);
      Log.Debug(json);

      var bag2 = JsonConvert.DeserializeObject<Properties>(json);

      Assert.IsNotNull(bag2);
      Assert.AreEqual(guid, bag2.Id);
    }

    [TestMethod]
    public void Create2PropertiesAndSerializationTest()
    {
      var bag1 = PropertyHelpers.CreateBag("GUID-1");
      var bag2 = PropertyHelpers.CreateBag("GUID-2");
      var bag3 = PropertyHelpers.CreateBag("GUID-3");

      var bags1 = new List<Properties>() { bag1, bag2, bag3 };
      var json = JsonConvert.SerializeObject(bags1, Formatting.Indented);
      var bags2 = JsonConvert.DeserializeObject<List<Properties>>(json);

      Log.Debug(json);

      Assert.IsNotNull(bag2);
      Assert.AreEqual(bags1.Count, bags2.Count);
    }
  }
}

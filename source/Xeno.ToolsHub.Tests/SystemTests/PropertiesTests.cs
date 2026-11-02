/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    PropertyTests.cs
 * Description:
 *
 * Reference:
 *  - https://stackoverflow.com/questions/22123619/replacing-only-a-certain-part-of-a-json-object
 */

namespace Xeno.ToolsHub.Tests.SystemTests
{
  using System.Collections.Generic;
  using System.Text.Json;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Config;
  using Xeno.ToolsHub.Models.PropertyService;
  using Xeno.ToolsHub.Services.Logging;
  using Xeno.ToolsHub.Tests.Helpers;

  [TestClass]
  public class PropertiesTests
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

      string json = JsonSerializer.Serialize(bag1, JsonOptions.Indented);
      Log.Debug(json);

      var bag2 = JsonSerializer.Deserialize<Properties>(json);

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
      var json = JsonSerializer.Serialize(bags1, JsonOptions.Indented);
      var bags2 = JsonSerializer.Deserialize<List<Properties>>(json);

      Log.Debug(json);

      Assert.IsNotNull(bag2);
      Assert.AreEqual(bags1.Count, bags2.Count);
    }
  }
}

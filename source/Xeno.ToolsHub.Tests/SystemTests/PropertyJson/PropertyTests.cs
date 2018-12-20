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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.Services.PropertyService;

namespace Xeno.ToolsHub.Tests.SystemTests.PropertyJson
{
  [TestClass()]
  public class PropertyTests
  {
    [TestMethod]
    public void PropertyBagSerializationTest()
    {
      var bag = CreateBag("{3E4B84CB-C0EF-449B-AA8D-FEB2EEF9D26D}");

      string json = JsonConvert.SerializeObject(bag);

      var obj = JsonConvert.DeserializeObject<PropertyBag>(json);

      Assert.AreEqual<PropertyBag>(bag, obj);
    }

    [TestMethod]
    public void PropertyStoreSerializationTest()
    {
      var bag1 = CreateBag("ABCD");
      var bag2 = CreateBag("1234");
      var bag3 = CreateBag("!@#$");

      var bags = new List<PropertyBag>() { bag1, bag2, bag3 };
      var json = JsonConvert.SerializeObject(bags, Formatting.Indented);
      var obj = JsonConvert.DeserializeObject<List<PropertyBag>>(json);

      Log.Debug(json);

      Assert.AreEqual<List<PropertyBag>>(bags, obj);
    }

    private PropertyBag CreateBag(string addinGuid)
    {
      return new PropertyBag()
      {
        AddinGuid = addinGuid,
        Properties = new Dictionary<string, string>()
        {
          {"Key1", "True"},
          {"Key2", "False"},
          {"Key3", "comma,separated,values"},
        }
      };
    }
  }
}

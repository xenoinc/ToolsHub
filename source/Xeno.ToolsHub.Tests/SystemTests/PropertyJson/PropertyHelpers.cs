/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertyHelpers.cs
 * Description:
 *  Helpers
 */

namespace Xeno.ToolsHub.Tests.SystemTests.PropertyJson
{
  using System.Collections.Generic;
  using Xeno.ToolsHub.Services.PropertyService;

  public static class PropertyHelpers
  {
    public static Properties CreateBag(string addinGuid)
    {
      return new Properties(addinGuid)
      {
        Items = new Properties.ItemCollection
        {
          { "Key1", "True" },
          { "Key2", "False" },
          { "Key3", "comma,separated,values" }
        }
      };
    }

    public static List<Properties> CreateBags(int num = 3)
    {
      var bags = new List<Properties>();

      for (var ndx = 0; ndx < num; ndx++)
      {
        bags.Add(CreateBag(System.Guid.NewGuid().ToString()));
      }

      return bags;
    }

    public static PropertiesStore CreateStore()
    {
      var store = new PropertiesStore()
      {
        PropertyBags = new List<Properties>()
        {
          new Properties("GUID1")
          {
            Items = new Properties.ItemCollection
            {
              { "A-Key1", "A-Value1" },
              { "A-Key2", "A-Value2" }
            }
          },
          new Properties("GUID2")
          {
            Items = new Properties.ItemCollection
            {
              { "B-Key1", "B-Value1" },
              { "B-Key2", "B-Value2" },
            }
          }
        }
      };

      return store;
    }
  }
}

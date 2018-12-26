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
    public static PropertyBag CreateBag(string addinGuid)
    {
      return new PropertyBag(addinGuid)
      {
        Properties = new PropertyBag.Items
        {
          { "Key1", "True" },
          { "Key2", "False" },
          { "Key3", "comma,separated,values" }
        }
      };
    }

    public static List<PropertyBag> CreateBags(int num = 3)
    {
      var bags = new List<PropertyBag>();

      for (var ndx = 0; ndx < num; ndx++)
      {
        bags.Add(CreateBag(System.Guid.NewGuid().ToString()));
      }

      return bags;
    }

    public static PropertyStore CreateStore()
    {
      var store = new PropertyStore()
      {
        PropertyBags = new List<PropertyBag>()
        {
          new PropertyBag("GUID1")
          {
            Properties = new PropertyBag.Items
            {
              { "A-Key1", "A-Value1" },
              { "A-Key2", "A-Value2" }
            }
          },
          new PropertyBag("GUID2")
          {
            Properties = new PropertyBag.Items
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

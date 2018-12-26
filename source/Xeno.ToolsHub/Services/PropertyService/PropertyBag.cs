/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    PropertyService.cs
 * Description:
 *  Property collection for an entity.
 *  We're wrapping Dictionary<> as class, Items (cleaner)
 *
 *  Tried wrapping Dict in PropBag, however it wouldn't serialize Id
 */

namespace Xeno.ToolsHub.Services.PropertyService
{
  using System.Collections.Generic;

  public class PropertyBag
  {
    public PropertyBag(string guid)
    {
      this.Id = guid;
      this.Properties = new Items();
    }

    /// <summary>Gets or sets property key and value</summary>
    public Items Properties { get; set; }

    /// <summary>Gets or sets owner of the properties (GUID)</summary>
    public string Id { get; set; }

    public void Add(string key, string value)
    {
      this.Properties.Add(key, value);
    }

    public class Items : Dictionary<string, string>
    {
      public Items()
      {
      }
    }
  }
}

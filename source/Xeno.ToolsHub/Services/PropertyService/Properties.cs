/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    Properties.cs
 * Description:
 *  Property collection for an entity.
 *  We're wrapping Dictionary<> as class, ItemCollection (cleaner)
 *
 *  Tried wrapping Dict<> in Properties, however it wouldn't serialize Id
 */

namespace Xeno.ToolsHub.Services.PropertyService
{
  using System.Collections.Generic;

  public class Properties
  {
    public Properties(string guid)
    {
      this.Id = guid;
      this.Items = new ItemCollection();
    }

    /// <summary>Gets or sets property key and value</summary>
    /// <value>Property items</value>
    public ItemCollection Items { get; set; }

    /// <summary>Gets or sets owner of the properties (GUID)</summary>
    /// <value>GUID string</value>
    public string Id { get; set; }

    public void Add(string key, string value)
    {
      this.Items.Add(key, value);
    }

    public class ItemCollection : Dictionary<string, string>
    {
      public ItemCollection()
      {
      }
    }
  }
}

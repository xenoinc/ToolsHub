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
    public Properties(string propertyId)
    {
      this.Id = propertyId;
      this.Items = new ItemCollection();
    }

    /// <summary>Gets or sets property key and value</summary>
    /// <value>Property items</value>
    public ItemCollection Items { get; set; }

    /// <summary>Gets or sets owner of the property set Id</summary>
    /// <value>Property Id (GUID)</value>
    public string Id { get; set; }

    public void Add(string key, string value)
    {
      this.Items.Add(key, value);
    }

    public string Find(string key)
    {
      string value = null;
      if (this.Items.ContainsKey(key))
        return this.Items[key];

      return value;
    }

    public class ItemCollection : Dictionary<string, string>
    {
      public ItemCollection()
      {
      }

      public string GetValueOrDefault(string key, string defaultValue)
      {
        string value;
        return this.TryGetValue(key.ToString(), out value) ? value : defaultValue;
      }

      // Use this if we weren't subclassing <string, string>
      ////public TValue GetValueOrDefault<TKey, TValue>(TKey key, TValue defaultValue)
      ////{
      ////  TValue value;
      ////  return this.TryGetValue(key.ToString(), out value) ? value : defaultValue;
      ////}
      ////
      ////public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, System.Func<TValue> defaultValueProvider)
      ////{
      ////  TValue value;
      ////  return dictionary.TryGetValue(key, out value) ? value
      ////       : defaultValueProvider();
      ////}

    }
  }
}

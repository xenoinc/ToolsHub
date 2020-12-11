/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertiesManager.cs
 * Description:
 *  Housing store for all add-in properties.
 */

namespace Xeno.ToolsHub.Models.PropertyService
{
  using System.Collections.Generic;

  public class PropertiesStore
  {
    public PropertiesStore()
    {
      this.PropertyBags = new List<Properties>();
    }

    /// <summary>Gets or sets the bag of Properties (set of a set).</summary>
    // TODO: Change to Dictionary<string, Properties>() so we don't have duplicate Properties.Id
    public List<Properties> PropertyBags { get; set; }

    /// <summary>Add new property bag to list</summary>
    /// <param name="propertyBag">Property collection</param>
    public void Add(Properties propertyBag)
    {
      bool found = false;
      foreach (var bag in PropertyBags)
      {
        if (bag.Id == propertyBag.Id)
          found = true;
      }

      if (!found)
        this.PropertyBags.Add(propertyBag);
    }

    /// <summary>Remove all properties in the store</summary>
    public void ClearAll()
    {
      PropertyBags.Clear();
    }

    /// <summary>Find a property collection based on Id</summary>
    /// <param name="propertyId">Unique Id of collection</param>
    /// <returns>PropertyBag or null if empty</returns>
    public Properties Find(string propertyId)
    {
      foreach (var bag in this.PropertyBags)
      {
        if (bag.Id == propertyId)
          return bag;
      }

      return null;
    }

    /// <summary>Get property value from Id and key in the bag.</summary>
    /// <param name="propertyId">PropertySet Id</param>
    /// <param name="key">Key value.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <returns>Property value or default.</returns>
    public string GetValue(string propertyId, string key, string defaultValue)
    {
      string value = defaultValue;

      Properties bag = this.Find(propertyId);
      if (bag != null)
        value = bag.Items.GetValueOrDefault(key, defaultValue);

      return value;
    }

    /// <summary>Set key value from PropertyId in bag.</summary>
    /// <param name="propertyId">PropertySet Id</param>
    /// <param name="key">Key value.</param>
    /// <param name="value">Setting value.</param>
    public void SetValue(string propertyId, string key, string value)
    {
      Properties bag = this.Find(propertyId);
      if (bag != null)
      {
        bag.Items[key] = value;
      }
      else
      {
        bag = new Properties(propertyId);
        bag.Add(key, value);
        this.PropertyBags.Add(bag);
      }
    }
  }
}

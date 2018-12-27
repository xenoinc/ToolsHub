/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertyStore.cs
 * Description:
 *  Housing store for all add-in properties
 */

namespace Xeno.ToolsHub.Services.PropertyService
{
  using System.Collections.Generic;
  using Newtonsoft.Json;

  public class PropertyStore
  {
    public PropertyStore()
    {
      this.PropertyBags = new List<PropertyBag>();
    }

    public List<PropertyBag> PropertyBags { get; set; }

    public void Add(PropertyBag propertyBag)
    {
      if (!this.PropertyBags.Contains(propertyBag))
        this.PropertyBags.Add(propertyBag);
    }

    public PropertyBag Find(string guid)
    {
      foreach (var bag in this.PropertyBags)
      {
        if (bag.Id == guid)
          return bag;
      }

      return null;
    }

    public void Save()
    {
      var json = JsonConvert.SerializeObject(this, Formatting.Indented);

      // Save file here
      ;
    }

    public void Load()
    {
    }
  }
}

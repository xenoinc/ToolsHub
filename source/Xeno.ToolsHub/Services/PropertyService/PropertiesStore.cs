/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-26
 * Author:  Damian Suess
 * File:    PropertyStore.cs
 * Description:
 *  Housing store for all add-in properties
 */

namespace Xeno.ToolsHub.Services.PropertyService
{
  using System;
  using System.Collections.Generic;
  using Xeno.ToolsHub.Config;

  public class PropertiesStore
  {
    public PropertiesStore()
      : this(string.Empty)
    {
    }

    public PropertiesStore(string settingsFile)
    {
      this.PropertyBags = new List<Properties>();
      this.SettingsFile = settingsFile;
    }

    public List<Properties> PropertyBags { get; set; }

    public string SettingsFile { get; set; }

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

    /// <summary>Find a property collection based on Id</summary>
    /// <param name="guid">Unique Id of collection</param>
    /// <returns>PropertyBag</returns>
    public Properties Find(string guid)
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
      try
      {
        if (string.IsNullOrEmpty(SettingsFile))
          return;

        Helpers.FileSerialize(PropertyBags, this.SettingsFile, true);
      }
      catch (Exception ex)
      {
        Log.Error($"Error saving properties file, '{this.SettingsFile}'. Exception: {ex.Message}");
      }
    }

    public void Load()
    {
      try
      {
        if (!string.IsNullOrEmpty(this.SettingsFile))
          PropertyBags = Helpers.FileDeserialize<List<Properties>>(this.SettingsFile);
      }
      catch (Exception ex)
      {
        Log.Error($"Error loading properties file, '{this.SettingsFile}'. Exception: {ex.Message}");
      }
    }
  }
}

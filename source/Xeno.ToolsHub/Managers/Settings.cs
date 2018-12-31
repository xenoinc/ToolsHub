/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-8-13
 * Author:  Damian Suess
 * File:    SettingsManager.cs
 * Description:
 *  Preference manager used by application and add-ins
 *
 * TODO:
 *  - Load via IoC not singleton
 */

namespace Xeno.ToolsHub.Managers
{
  using System;
  using System.Collections.Generic;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.Config;
  using Xeno.ToolsHub.Models.PropertyService;
  using Xeno.ToolsHub.Services.Logging;

  public static class Settings
  {
    private static readonly string _fileName = "Properties.json";

    private static PropertiesStore _propStore = new PropertiesStore();

    public static PropertiesStore PropertiesStore
    {
      get { return _propStore; }
      set { _propStore = value; }
    }

    /// <summary>Gets or sets the app's default storage method</summary>
    /// <value>The app's default storage method</value>
    public static StorageMethod StorageMethod { get; set; }

    /// <summary>Gets the settings file</summary>
    /// <value>Full path to ToolsHub.json</value>
    public static string SettingsFilePath
    {
      get
      {
        string pth = string.Empty;
        pth = Helpers.GetStorageFolder(StorageMethod);

        // Append file to path
        if (!string.IsNullOrEmpty(_fileName))
          pth = System.IO.Path.Combine(pth, _fileName);

        return pth;
      }
    }

    public static string ToString => JsonConvert.SerializeObject(_propStore.PropertyBags, Formatting.Indented);

    public static void Clear()
    {
      _propStore.ClearAll();
    }

    /// <summary>Get settings value</summary>
    /// <param name="propertyId">Property unique Id</param>
    /// <param name="key">Key name</param>
    /// <param name="defValue">Default value</param>
    /// <returns>Setting's value</returns>
    public static string GetValue(string propertyId, string key, string defValue)
    {
      return _propStore.GetValue(propertyId, key, defValue);
    }

    /// <summary>Get JSON object</summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="propertyId">Property unique Id</param>
    /// <param name="key">Key name</param>
    /// <returns>Setting object</returns>
    public static T GetObject<T>(string propertyId, string key)
    {
      try
      {
        string json = GetValue(propertyId, key, string.Empty);
        return JsonConvert.DeserializeObject<T>(json);
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public static void SetValue(string propertyId, string name, string value)
    {
      _propStore.SetValue(propertyId, name, value);
    }

    public static void SetObject(string propertyId, string key, object o)
    {
      _propStore.SetValue(propertyId, key, JsonConvert.SerializeObject(_propStore.PropertyBags, Formatting.None));
    }

    /// <summary>Load properties file into memory</summary>
    public static void LoadFile()
    {
      try
      {
        if (!string.IsNullOrEmpty(SettingsFilePath))
        {
          _propStore.PropertyBags = JsonConvert.DeserializeObject<List<Properties>>(
            System.IO.File.ReadAllText(SettingsFilePath));
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Error loading properties file, '{SettingsFilePath}'. Exception: {ex.Message}");
      }
    }

    /// <summary>Save the stored properties</summary>
    public static void SaveFile()
    {
      try
      {
        if (string.IsNullOrEmpty(SettingsFilePath))
          return;

        string data = JsonConvert.SerializeObject(_propStore.PropertyBags, Formatting.Indented);
        if (SettingsFilePath != null)
          System.IO.File.WriteAllText(SettingsFilePath, data);
      }
      catch (Exception ex)
      {
        Log.Error($"Error saving properties file, '{SettingsFilePath}'. Exception: {ex.Message}");
      }
    }
  }
}

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

  public class SettingsManager
  {
    private PropertiesStore _propStore = new PropertiesStore();

    public SettingsManager()
      : this(StorageMethod.Unknown)
    {
    }

    public SettingsManager(StorageMethod storageMethod)
    {
      StorageMethod = storageMethod;
    }

    public PropertiesStore PropertiesStore
    {
      get { return _propStore; }
      set { _propStore = value; }
    }

    /// <summary>Gets or sets the app's default storage method</summary>
    /// <value>The app's default storage method</value>
    public StorageMethod StorageMethod { get; set; }

    /// <summary>Gets the settings file</summary>
    /// <value>Full path to ToolsHub.json</value>
    public string SettingsFilePath
    {
      get
      {
        string pth = string.Empty;
        pth = Helpers.GetStorageFolder(StorageMethod);

        // Append file to path
        if (!string.IsNullOrEmpty(Constants.SettingsFile))
          pth = System.IO.Path.Combine(pth, Constants.SettingsFile);

        return pth;
      }
    }

    public new string ToString => JsonConvert.SerializeObject(_propStore.PropertyBags, Formatting.Indented);

    public void Clear()
    {
      _propStore.ClearAll();
    }

    /// <summary>Get JSON object</summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <param name="propertyId">Property unique Id</param>
    /// <param name="key">Key name</param>
    /// <returns>Setting object</returns>
    public T GetObject<T>(string propertyId, string key)
    {
      try
      {
        string data = GetValue(propertyId, key, string.Empty);
        data = Helpers.Base64Decode(data);
        return JsonConvert.DeserializeObject<T>(data);
      }
      catch (Exception ex)
      {
        Log.Error($"Issue decoding setting object from id: {propertyId}, key: {key}. {ex.Message}");
        throw ex;
      }
    }

    /// <summary>Get settings value</summary>
    /// <param name="propertyId">Property unique Id</param>
    /// <param name="key">Key name</param>
    /// <param name="defValue">Default value</param>
    /// <returns>Setting's value</returns>
    public string GetValue(string propertyId, string key, string defValue = "")
    {
      return _propStore.GetValue(propertyId, key, defValue);
    }

    public void SetObject(string propertyId, string key, object o)
    {
      string data = JsonConvert.SerializeObject(o, Formatting.None);
      data = Helpers.Base64Encode(data);
      _propStore.SetValue(propertyId, key, data);
    }

    public void SetValue(string propertyId, string key, string value)
    {
      _propStore.SetValue(propertyId, key, value);
    }

    /// <summary>Load properties file into memory</summary>
    public void LoadFile()
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
    public void SaveFile()
    {
      try
      {
        if (string.IsNullOrEmpty(SettingsFilePath))
          return;

        if (SettingsFilePath != null)
        {
          string data = JsonConvert.SerializeObject(_propStore.PropertyBags, Formatting.Indented);
          System.IO.File.WriteAllText(SettingsFilePath, data);
          Log.Debug("Settings saved");
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Error saving properties file, '{SettingsFilePath}'. Exception: {ex.Message}");
      }
    }
  }
}

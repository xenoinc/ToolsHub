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

namespace Xeno.Dockzi.Managers
{
  using System;
  using System.Collections.Generic;
  using System.Text.Json;
  using Xeno.Dockzi.Config;
  using Xeno.Dockzi.Models.PropertyService;
  using Xeno.Dockzi.Services.Logging;

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
    /// <value>Full path to Dockzi.config</value>
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

    public new string ToString => JsonSerializer.Serialize(_propStore.PropertyBags, JsonOptions.Indented);

    /// <summary>Clear setting cache from memory.</summary>
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
        if (string.IsNullOrWhiteSpace(data))
          return default;
        data = Helpers.Base64Decode(data);
        return string.IsNullOrWhiteSpace(data) ? default : JsonSerializer.Deserialize<T>(data, JsonOptions.Compact);
      }
      catch (Exception ex)
      {
        Log.Error($"Issue decoding setting object from id: {propertyId}, key: {key}. {ex.Message}");
        throw;
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

    /// <summary>Saves object as Base64.</summary>
    /// <param name="propertyId">Property Id.</param>
    /// <param name="key">Key.</param>
    /// <param name="o">Object type.</param>
    public void SetObject(string propertyId, string key, object o)
    {
      string data = JsonSerializer.Serialize(o, JsonOptions.Compact);
      data = Helpers.Base64Encode(data);
      _propStore.SetValue(propertyId, key, data);
    }

    /// <summary>Saves object as plain text.</summary>
    /// <param name="propertyId">Property Id.</param>
    /// <param name="key">Key.</param>
    /// <param name="value">Text value.</param>
    public void SetValue(string propertyId, string key, string value)
    {
      _propStore.SetValue(propertyId, key, value);
    }

    /// <summary>Load properties file into memory</summary>
    public void LoadFile()
    {
      try
      {
        if (!string.IsNullOrEmpty(SettingsFilePath) && System.IO.File.Exists(SettingsFilePath))
        {
          _propStore.PropertyBags = JsonSerializer.Deserialize<List<Properties>>(
            System.IO.File.ReadAllText(SettingsFilePath), JsonOptions.Compact) ?? new List<Properties>();
        }
      }
      catch (Exception ex)
      {
        Log.Error($"Error loading properties file, '{SettingsFilePath}'. Exception: {ex.Message}");
      }
    }

    /// <summary>Atomically save settings, allowing callers to handle write failures.</summary>
    public void SaveFileOrThrow()
    {
      string path = System.IO.Path.GetFullPath(SettingsFilePath);
      string directory = System.IO.Path.GetDirectoryName(path);
      System.IO.Directory.CreateDirectory(directory);
      string temporary = System.IO.Path.Combine(directory, $".Dockzi-{Guid.NewGuid():N}.tmp");
      try
      {
        string data = JsonSerializer.Serialize(_propStore.PropertyBags, JsonOptions.Indented);
        System.IO.File.WriteAllText(temporary, data);
        System.IO.File.Move(temporary, path, true);
        Log.Debug("Settings saved");
      }
      finally
      {
        if (System.IO.File.Exists(temporary))
          System.IO.File.Delete(temporary);
      }
    }

    /// <summary>Save the stored properties</summary>
    public void SaveFile()
    {
      try
      {
        SaveFileOrThrow();
      }
      catch (Exception ex)
      {
        Log.Error($"Error saving properties file, '{SettingsFilePath}'. Exception: {ex.Message}");
      }
    }
  }
}

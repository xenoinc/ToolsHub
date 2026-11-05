namespace Xeno.Dockzi.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services.Logging;

public static class SettingsLoader
{
  public static SettingsManager Load() => Load(SelectStorageMethod());

  public static SettingsManager Load(StorageMethod method)
  {
    var settings = new SettingsManager(method);
    if (!File.Exists(settings.SettingsFilePath))
    {
      string legacy = LegacyPaths(method).FirstOrDefault(File.Exists);
      if (legacy != null)
      {
        string directory = Path.GetDirectoryName(settings.SettingsFilePath);
        Directory.CreateDirectory(directory);
        File.Copy(legacy, settings.SettingsFilePath);
        string legacySidebar = Path.Combine(Path.GetDirectoryName(legacy), "SidebarLaunchers.json");
        string sidebar = Path.Combine(directory, "SidebarLaunchers.json");
        if (!File.Exists(sidebar) && File.Exists(legacySidebar))
          File.Copy(legacySidebar, sidebar);
        Log.Info("Imported legacy settings into Dockzi.config; the original files have been preserved.");
      }
    }

    settings.LoadFile();
    return settings;
  }

  public static StorageMethod SelectStorageMethod()
  {
    foreach (var method in new[] { StorageMethod.PortableApp, StorageMethod.AllUsers, StorageMethod.SingleUser })
    {
      if (File.Exists(Path.Combine(Helpers.GetStorageFolder(method), Constants.SettingsFile)) || LegacyPaths(method).Any(File.Exists))
        return method;
    }

    return StorageMethod.PortableApp;
  }

  private static IEnumerable<string> LegacyPaths(StorageMethod method)
  {
    yield return Path.Combine(Helpers.GetStorageFolder(method), "Dockzi.db");
    yield return Path.Combine(Helpers.GetStorageFolder(method), "ToolsHub.db");
    if (method == StorageMethod.AllUsers || method == StorageMethod.SingleUser)
    {
      var folder = method == StorageMethod.AllUsers ? Environment.SpecialFolder.CommonApplicationData : Environment.SpecialFolder.LocalApplicationData;
      yield return Path.Combine(Environment.GetFolderPath(folder), "XI", "ToolsHub", "Dockzi.db");
      yield return Path.Combine(Environment.GetFolderPath(folder), "XI", "ToolsHub", "ToolsHub.db");
    }
  }
}

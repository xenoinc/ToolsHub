namespace Xeno.ToolsHub.Services;

using System.IO;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.Managers;

public static class SettingsLoader
{
  public static SettingsManager Load()
  {
    var settings = new SettingsManager(SelectStorageMethod());
    settings.LoadFile();
    return settings;
  }

  public static StorageMethod SelectStorageMethod()
  {
    foreach (var method in new[] { StorageMethod.PortableApp, StorageMethod.AllUsers, StorageMethod.SingleUser })
    {
      if (File.Exists(Path.Combine(Helpers.GetStorageFolder(method), Constants.SettingsFile)))
        return method;
    }

    return StorageMethod.PortableApp;
  }
}

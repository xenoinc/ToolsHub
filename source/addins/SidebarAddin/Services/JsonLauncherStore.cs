namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.SidebarAddin.Models;

/// <summary>Stores only sidebar launchers, independently of the host shortcut property bag.</summary>
public sealed class JsonLauncherStore : ILauncherStore
{
  private readonly string _path;

  public JsonLauncherStore(SettingsManager settings)
    : this(Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json"))
  {
  }

  public JsonLauncherStore(string path) => _path = path;

  public IReadOnlyList<DockLauncher> Load()
  {
    if (!File.Exists(_path))
      return Array.Empty<DockLauncher>();
    var items = JsonSerializer.Deserialize<List<DockLauncher>>(File.ReadAllText(_path), JsonOptions.Compact)
      ?? throw new InvalidDataException("The sidebar launcher file contains no list.");
    if (items.Any(item => item == null || string.IsNullOrWhiteSpace(item.Id) || string.IsNullOrWhiteSpace(item.Target))
      || items.Select(item => item.Id).Distinct(StringComparer.Ordinal).Count() != items.Count)
      throw new InvalidDataException("The sidebar launcher file contains invalid or duplicate entries.");
    return items;
  }

  public void Save(IReadOnlyList<DockLauncher> items)
  {
    string directory = Path.GetDirectoryName(Path.GetFullPath(_path));
    Directory.CreateDirectory(directory);
    string temporary = Path.Combine(directory, $".SidebarLaunchers-{Guid.NewGuid():N}.tmp");
    try
    {
      File.WriteAllText(temporary, JsonSerializer.Serialize(items, JsonOptions.Indented));
      File.Move(temporary, _path, true);
    }
    finally
    {
      if (File.Exists(temporary))
        File.Delete(temporary);
    }
  }
}

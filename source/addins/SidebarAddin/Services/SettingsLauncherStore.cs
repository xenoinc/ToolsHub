namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.SidebarAddin.Models;
using JsonOptions = Xeno.ToolsHub.Config.JsonOptions;

/// <summary>Stores sidebar launchers in the Sidebar property bag in ToolsHub.db.</summary>
public sealed class SettingsLauncherStore : ILauncherStore
{
  private readonly string _legacyPath;
  private readonly SettingsManager _settings;

  public SettingsLauncherStore(SettingsManager settings)
  {
    _settings = settings;
    _legacyPath = Path.Combine(Path.GetDirectoryName(settings.SettingsFilePath), "SidebarLaunchers.json");
  }

  public IReadOnlyList<DockLauncher> Load()
  {
    if (_settings.PropertiesStore.Find(Constants.AddinId)?.Items.ContainsKey(Constants.KeyLaunchers) == true)
    {
      var saved = _settings.GetObject<List<DockLauncher>>(Constants.AddinId, Constants.KeyLaunchers);
      Validate(saved);
      return saved;
    }

    if (!File.Exists(_legacyPath))
      return Array.Empty<DockLauncher>();

    // Import once; a saved empty list also takes precedence over the legacy file.
    // Preserve the original file as a backup rather than deleting user data.
    var imported = JsonSerializer.Deserialize<List<DockLauncher>>(File.ReadAllText(_legacyPath), JsonOptions.Compact);
    Validate(imported);
    Save(imported);
    return imported;
  }

  public void Save(IReadOnlyList<DockLauncher> items)
  {
    Validate(items);
    var bag = _settings.PropertiesStore.Find(Constants.AddinId);
    string previous = null;
    bool hadValue = bag != null && bag.Items.TryGetValue(Constants.KeyLaunchers, out previous);
    try
    {
      _settings.SetObject(Constants.AddinId, Constants.KeyLaunchers, items);
      _settings.SaveFileOrThrow();
    }
    catch
    {
      if (bag == null)
        _settings.PropertiesStore.PropertyBags.Remove(_settings.PropertiesStore.Find(Constants.AddinId));
      else if (hadValue)
        bag.Items[Constants.KeyLaunchers] = previous;
      else
        bag.Items.Remove(Constants.KeyLaunchers);
      throw;
    }
  }

  private static void Validate(IReadOnlyList<DockLauncher> items)
  {
    if (items == null)
      throw new InvalidDataException("The saved sidebar launcher list is empty or invalid.");
    if (items.Any(item => item == null || string.IsNullOrWhiteSpace(item.Id) || string.IsNullOrWhiteSpace(item.Target))
      || items.Select(item => item.Id).Distinct(StringComparer.Ordinal).Count() != items.Count)
      throw new InvalidDataException("The saved sidebar launcher list contains invalid or duplicate entries.");
  }
}

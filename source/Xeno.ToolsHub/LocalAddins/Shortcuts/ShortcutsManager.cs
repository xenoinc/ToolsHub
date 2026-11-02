/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-9
 * Author:  Damian Suess
 * File:    ShortcutsSysTray.cs
 * Description:
 *  Shortcuts System Tray add-in
 *  Features:
 *    - Loads stored Shortcuts into memory
 *    - Add shortcuts links to system tray 'Shortcuts' sub-menu
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts;

using System;
using System.Collections.Generic;
using Lite.EventIpc;
using System.Windows.Forms;
using System.Text.Json;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.Services.Messaging;

public class ShortcutsManager
{
  public const string ShortcutsAddinId = "ShortcutsAddin";

  public const string ShortcutItemsKey = "ShortcutItems";

  private readonly SettingsManager _settings;
  private readonly IEventAggregator _events;

  public ShortcutsManager()
    : this(Program.Settings)
  {
  }

  public ShortcutsManager(SettingsManager settings)
    : this(settings, AppEvents.Instance)
  {
  }

  public ShortcutsManager(SettingsManager settings, IEventAggregator events)
  {
    _events = events ?? throw new ArgumentNullException(nameof(events));
    _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    ShortcutItems = _settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey) ?? new ShortcutItems();
  }

  public ShortcutItems ShortcutItems { get; set; }

  /// <summary>Load shortcuts into systray from config file</summary>
  /// <returns>Menu item</returns>
  public List<ToolStripItem> LoadAsMenuItems()
  {
    List<ToolStripItem> shortcutItems = new List<ToolStripItem>();

    ShortcutItems shortcuts = _settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
    if (shortcuts == null)
    {
      Log.Debug($"No shortcuts found. Loading default");
      var item = new ExtensionModel.SystemTray.TrayItem("Create test JSON...", string.Empty, true, OnGenerateSampleShortcuts);

      ToolStripMenuItem menu = new ToolStripMenuItem("Shortcuts");
      menu.DropDownItems.Add(item);
      shortcutItems.Add(menu);
    }
    else
    {
      // TODO: Let the user decide if they want a parent-menu item or not; fornow, make one
      ToolStripMenuItem menu = new ToolStripMenuItem("Shortcuts");

      foreach (ShortcutItem shortcut in shortcuts)
      {
        var subItem = new ExtensionModel.SystemTray.TrayItem(shortcut.Title, shortcut.Target, true, _ => OnExecuteShortcut(shortcut));
        menu.DropDownItems.Add(subItem);
      }

      shortcutItems.Add(menu);
    }

    return shortcutItems;
  }

  public int OnExecuteShortcut(string target)
  {
    return OnExecuteShortcut(new ShortcutItem { Target = target });
  }

  public int OnExecuteShortcut(ShortcutItem shortcut)
  {
    string target = shortcut?.Target;
    try
    {
      var startInfo = ShortcutLauncher.CreateStartInfo(shortcut);
      Log.Debug($"Launching shortcut '{target}' with working directory '{startInfo.WorkingDirectory}'");
      using var process = System.Diagnostics.Process.Start(startInfo);
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to execute shortcut target, '{target}' - {ex}");

      _events.Publish(new SystemTrayNotificationRequest($"Cannot resolve target, '{target}'"));
    }

    return 0;
  }

  public int OnGenerateSampleShortcuts(string target)
  {
    Log.Debug($"Generating a sample ShortcutItems");

    // TODO: Open dialog to create custom shorts
    var items = new ShortcutItems
    {
      new ShortcutItem { Title = "C-Drive", Target = @"C:\" },
      new ShortcutItem { Title = "D-Drive", Target = @"D:\" },
    };

    ShortcutItems = items;
    SaveShortcuts();

    return 0;
  }

  /// <summary>Inform parent objects to refresh</summary>
  public void Refresh()
  {
    // TODO: Inform parents to refresh (SysTray/Sidebar)
    _events.Publish(new SystemTrayRefreshRequest());
  }

  public void SaveShortcuts()
  {
    _settings.SetObject(ShortcutsAddinId, ShortcutItemsKey, ShortcutItems);
    _settings.SaveFile();

    Log.Debug("Shortcut settings saved");
    Refresh();
  }

  /// <summary>
  ///   Validate JSON text and report errors if any
  /// </summary>
  /// <param name="rawJson">JSON in</param>
  /// <param name="errMessage">Error message if any found</param>
  /// <returns>Returns true if JSON format is solid</returns>
  public bool ValidateJson(string rawJson, out string errMessage)
  {
    try
    {
      using var document = JsonDocument.Parse(rawJson, new JsonDocumentOptions
      {
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip,
      });
      errMessage = string.Empty;
      return true;
    }
    catch (Exception ex)
    {
      errMessage = ex.Message;
    }

    return false;
  }

  /// <summary>ShortcutItems as text</summary>
  /// <returns>JSON format of ShortcutItems</returns>
  public override string ToString()
  {
    return JsonSerializer.Serialize(ShortcutItems, JsonOptions.Indented);
  }
}

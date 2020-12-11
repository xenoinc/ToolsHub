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

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  using System;
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Newtonsoft.Json;
  using Xeno.ToolsHub.ExtensionModel.SystemTray;
  using Xeno.ToolsHub.Services.Logging;
  using Xeno.ToolsHub.Services.Messaging;

  public class ShortcutsManager
  {
    public const string ShortcutsAddinId = "ShortcutsAddin";

    public const string ShortcutItemsKey = "ShortcutItems";

    public ShortcutsManager()
    {
      // Settings.LoadFile();
      var test = Program.Settings.GetValue("ShortcutsAddin", "ShortcutItems");
      var shortcuts = Program.Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);

      this.ShortcutItems = shortcuts == null ? new ShortcutItems() : shortcuts;
    }

    public ShortcutItems ShortcutItems { get; set; }

    /// <summary>Load shortcuts into systray from config file</summary>
    /// <returns>Menu item</returns>
    public List<MenuItem> LoadAsMenuItems()
    {
      List<MenuItem> shortcutItems = new List<MenuItem>();

      ShortcutItems shortcuts = Program.Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
      if (shortcuts == null)
      {
        Log.Debug($"No shortcuts found. Loading default");
        var item = new ExtensionModel.SystemTray.TrayItem("Create test JSON...", string.Empty, true, OnGenerateSampleShortcuts);

        MenuItem menu = new MenuItem("Shortcuts");
        menu.MenuItems.Add(0, item);
        shortcutItems.Add(menu);
      }
      else
      {
        // TODO: Let the user decide if they want a parent-menu item or not; fornow, make one
        MenuItem menu = new MenuItem("Shortcuts");

        List<MenuItem> items = new List<MenuItem>();
        int ndx = 0;
        foreach (ShortcutItem shortcut in shortcuts)
        {
          var subItem = new ExtensionModel.SystemTray.TrayItem(shortcut.Title, shortcut.Target, true, OnExecuteShortcut);
          menu.MenuItems.Add(ndx, subItem);
          ndx++;
        }

        shortcutItems.Add(menu);
      }

      return shortcutItems;
    }

    public int OnExecuteShortcut(string target)
    {
      Log.Debug($"Executing MyMethod with target, '{target}'");

      try
      {
        System.Diagnostics.Process.Start(target);
      }
      catch (Exception ex)
      {
        Log.Error($"Failed to execute shortcut target, '{target}' - {ex}");

        MessagingCenter.Send<SystemTrayMessages, string>(
          new SystemTrayMessages(),
          SystemTrayMessages.Notify,
          $"Cannot resolve target, '{target}'");
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
      MessagingCenter.Send<SystemTrayMessages>(new SystemTrayMessages(), SystemTrayMessages.Refresh);
    }

    public void SaveShortcuts()
    {
      Program.Settings.SetObject(ShortcutsAddinId, ShortcutItemsKey, ShortcutItems);
      Program.Settings.SaveFile();

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
        var obj = Newtonsoft.Json.Linq.JToken.Parse(rawJson);
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
      return JsonConvert.SerializeObject(ShortcutItems, Formatting.Indented);
    }
  }
}

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
  using Xeno.ToolsHub.Managers;
  using Xeno.ToolsHub.Services.Logging;

  public class ShortcutsManager
  {
    public const string ShortcutsAddinId = "ShortcutsAddin";

    public const string ShortcutItemsKey = "ShortcutItems";

    public ShortcutsManager()
    {
      //Settings.LoadFile();
      string test = Settings.GetValue("ShortcutsAddin", "ShortcutItems");

      var shortcuts = Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
      this.ShortcutItems = shortcuts == null ? new ShortcutItems() : shortcuts;
    }

    public ShortcutItems ShortcutItems { get; set; }

    /// <summary>Load shortcuts into systray from config file</summary>
    /// <returns>Menu item</returns>
    public List<MenuItem> LoadAsMenuItems()
    {
      List<MenuItem> shortcutItems = new List<MenuItem>();

      ShortcutItems shortcuts = Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
      if (shortcuts == null)
      {
        Log.Debug($"No shortcuts found. Loading default");
        var item = new ExtensionModel.SystemTray.TrayItem("Create test JSON...", "", true, OnGenerateSampleShortcuts);

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

      System.Diagnostics.Process.Start(target);
      return 0;
    }

    public int OnGenerateSampleShortcuts(string target)
    {
      // TODO: 1. Open dialog to create custom shorts
      // TODO: 2. Force SysTray to refresh itself
      Log.Debug($"Generating a sample ShortcutItems");

      ////string pth1 = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
      ////string pth2 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

      var items = new ShortcutItems
      {
        new ShortcutItem { Title = "Work", Target = @"C:\work" },
        // new ShortcutItem { Title = "Lab", Target = @"C:\work\lab" },
        // new ShortcutItem { Title = "Docs", Target = @"C:\work\docs" },
        new ShortcutItem { Title = "X-Drive", Target = @"X:\" },
      };

      ShortcutItems = items;
      Save();

      // TODO: Managers.SystemTray.SystemTrayManager.Refresh();
      return 0;
    }

    /// <summary>Inform parent objects to refresh</summary>
    public void Refresh()
    {
      // TODO: Inform parents (SysTray/Sidebar) to refresh
      // This may require MessagingCenter
    }

    public void Save()
    {
      Settings.SetObject(ShortcutsAddinId, ShortcutItemsKey, ShortcutItems);
      Settings.SaveFile();

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
        errMessage = "";
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

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
      var shortcuts = Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
      this.ShortcutItems = shortcuts == null ? new ShortcutItems() : shortcuts;
    }

    private ShortcutItems ShortcutItems { get; set; }

    /// <summary>Load shortcuts into systray from config file</summary>
    /// <returns>Menu item</returns>
    public MenuItem LoadAsMenuItems()
    {
      var menu = new MenuItem("Shortcuts");

      ShortcutItems shortcuts = Settings.GetObject<ShortcutItems>(ShortcutsAddinId, ShortcutItemsKey);
      if (shortcuts == null)
      {
        Log.Debug($"No shortcuts found. Loading default");
        var item = new ExtensionModel.SystemTray.TrayItem("Create test JSON...", "", true, OnJsonGenerator);
        menu.MenuItems.Add(0, item);
      }
      else
      {
        List<MenuItem> items = new List<MenuItem>();
        int ndx = 0;
        foreach (ShortcutItem shortcut in shortcuts)
        {
          var subItem = new ExtensionModel.SystemTray.TrayItem(shortcut.Title, shortcut.Target, true, OnExecuteShortcut);
          menu.MenuItems.Add(ndx, subItem);
          ndx++;
        }
      }

      return menu;
    }

    /// <summary>ShortcutItems as text</summary>
    /// <returns>JSON format of ShortcutItems</returns>
    public override string ToString()
    {
      return JsonConvert.SerializeObject(ShortcutItems, Formatting.Indented);
    }

    public int OnExecuteShortcut(string target)
    {
      Log.Debug($"Executing MyMethod with target, '{target}'");

      System.Diagnostics.Process.Start(target);
      return 0;
    }

    public int OnJsonGenerator(string target)
    {
      // TODO: 1. Open dialog to create custom shorts
      // TODO: 2. Force SysTray to refresh itself

      Log.Debug($"Generating a sample ShortcutItems");

      ////string pth1 = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
      ////string pth2 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

      var items = new ShortcutItems
      {
        new ShortcutItem { Title = "Dev", Target = @"C:\dev" },
        new ShortcutItem { Title = "Lab", Target = @"C:\work\lab" },
        new ShortcutItem { Title = "Docs", Target = @"C:\work\docs" },
        new ShortcutItem { Title = "X-Drive", Target = @"X:\" },
        // new ShortcutItem { Title = "-", Target = "" },
        // new ShortcutItem { Title = "Manage Shortcuts", Target = managePath },
      };

      SaveObject(items);

      // TODO: Managers.SystemTray.SystemTrayManager.Refresh();
      return 0;
    }

    public void SaveObject(object o)
    {
      Save(JsonConvert.SerializeObject(o, Formatting.Indented));
    }

    public void Save(string rawJson)
    {
      Settings.SetObject(ShortcutsAddinId, ShortcutItemsKey, ShortcutItems);
      Settings.SaveFile();
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
  }
}

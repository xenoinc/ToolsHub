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

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class ShortcutsSysTray
  {
    public ShortcutsSysTray()
    {
    }

    private string ShortcutsFile => "shortcuts.json";

    private string LocalShortcutsPath => Path.Combine(Constants.LocalFolder, ShortcutsFile);

    /// <summary>Load shortcuts into systray from config file</summary>
    /// <returns></returns>
    public MenuItem LoadAsMenuItems()
    {
      var menu = new MenuItem("Shortcuts");

      if (!System.IO.File.Exists(LocalShortcutsPath))
      {
        Log.Debug($"Missing local '{ShortcutsFile}' file");

        var item = new Managers.SystemTray.TrayItem("Create test JSON...", "", true, TestJsonGenerator);
        menu.MenuItems.Add(0, item);

        // Generate test items
        //var item = new Managers.SystemTray.TrayItem("Temp folder", @"c:\temp", true);
        //menu.MenuItems.Add(0, item);
        //
        //item = new Managers.SystemTray.TrayItem("Dev folder", @"C:\dev", true, MethodRouter);
        //menu.MenuItems.Add(1, item);
      }
      else
      {
        var shortcuts = Helpers.FileDeserialize<ShortcutItems>(LocalShortcutsPath);

        List<MenuItem> items = new List<MenuItem>();
        int ndx = 0;
        foreach (ShortcutItem shortcut in shortcuts)
        {
          var subItem = new Managers.SystemTray.TrayItem(shortcut.Title, shortcut.Target, true, OnExecuteShortcut);
          menu.MenuItems.Add(ndx, subItem);
          ndx++;
        }
      }

      return menu;
    }

    public int OnExecuteShortcut(string target)
    {
      Log.Debug($"Executing MyMethod with target, '{target}'");

      System.Diagnostics.Process.Start(target);
      return 0;
    }

    public int TestJsonGenerator(string target)
    {
      //TODO: 1. Open dialog to create custom shorts
      //TODO: 2. Force SysTray to refresh itself

      Log.Debug($"Generating a test, {ShortcutsFile}");

      string pth1 = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
      //string pth2 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

      string managePath = Path.Combine(pth1, ShortcutsFile);

      var items = new ShortcutItems
      {
        new ShortcutItem { Title = "Dev", Target = @"C:\dev" },
        new ShortcutItem { Title = "Lab", Target = @"C:\work\lab" },
        new ShortcutItem { Title = "Docs", Target = @"C:\work\docs" },
        new ShortcutItem { Title = "X-Drive", Target = @"X:\" },
        new ShortcutItem { Title = "-", Target = "" },
        new ShortcutItem { Title = "Manage Shortcuts", Target = managePath },
      };

      Helpers.FileSerialize(items, LocalShortcutsPath, true);

      //TODO: Managers.SystemTray.SystemTrayManager.Refresh();

      return 0;
    }
  }
}

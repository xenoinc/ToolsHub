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
using System.Windows.Forms;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class ShortcutsSysTray
  {
    public ShortcutsSysTray()
    {
    }

    /// <summary>
    ///   Load shortcuts into systray from config file
    /// </summary>
    /// <returns></returns>
    public MenuItem LoadAsMenuItems()
    {
      if (!System.IO.File.Exists(Constants.LocalShortcutsPath))
      {
        Log.Debug($"Missing local '{Constants.ShortcutsFile}' file");
        return new MenuItem("Shortcuts");
      }

      var shortcuts = Helpers.FileDeserialize<Shortcuts>(Constants.LocalShortcutsPath);

      var menu = new MenuItem("Shortcuts");
      List<MenuItem> items = new List<MenuItem>();
      int ndx = 0;

      foreach (Shortcut shortcut in shortcuts)
      {
        // TODO: add real details to launcher

        var target = shortcut.Target;

        var subItem = new Managers.SystemTray.TrayItem(shortcut.Title, "tag_addin1-Sub1", true);
        menu.MenuItems.Add(ndx, subItem);
        ndx++;
      }

      return menu;
    }
  }
}

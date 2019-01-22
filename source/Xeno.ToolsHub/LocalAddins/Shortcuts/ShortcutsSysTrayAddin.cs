/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-10
 * Author:  Damian Suess
 * File:    ShortcutsAddin.cs
 * Description:
 *  Extends the functionality of a Shortcut.
 *  If you wish to extend ToolsHub in a more broad spectrum, you'll need
 *  to create an ApplicationAddin
 *
 */

using System.Collections.Generic;
using System.Windows.Forms;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  //[Mono.Addins.Extension(
  //  Id = "ShortcutsAddinBoom",
  //  NodeName = "SysTrayAddin",
  //  Path = "/ToolsHub/SystemTray")]
  public class ShortcutsSysTrayAddin
    : Xeno.ToolsHub.ExtensionModel.SystemTray.SysTrayAddin // ExtensionModel.ISystemTrayExtension
  {
    public ShortcutsSysTrayAddin()
    {
    }

    public override bool IsInitialized => true;

    public string Title => "Shortcuts Add-in";

    public void Execute()
    {
    }

    public override List<MenuItem> MenuItems()
    {
      ShortcutsManager shortcutTray = new ShortcutsManager();
      var items = shortcutTray.LoadAsMenuItems();
      return items;
    }
  }
}

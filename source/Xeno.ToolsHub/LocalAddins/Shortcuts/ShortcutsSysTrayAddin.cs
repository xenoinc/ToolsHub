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
    : Xeno.ToolsHub.Managers.SystemTray.SysTrayAddin // ExtensionModel.ISystemTrayExtension
  {
    public ShortcutsSysTrayAddin()
    {
    }

    public string Title => "Shortcuts Add-in";

    public override bool IsInitialized => true;

    public void Execute()
    {
    }

    public override void Initialize()
    {
    }

    public override List<MenuItem> MenuItems()
    {
      ShortcutsLoader shortcutTray = new ShortcutsLoader();
      var item = shortcutTray.LoadAsMenuItems();

      var items = new List<MenuItem>();
      items.Add(item);

      return items;
    }
  }
}

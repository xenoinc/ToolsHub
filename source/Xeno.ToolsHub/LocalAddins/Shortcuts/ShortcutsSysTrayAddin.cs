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
  // [Mono.Addins.Extension(
  //  Id = "ShortcutsAddinBoom",
  //  NodeName = "SysTrayAddin",
  //  Path = "/ToolsHub/SystemTray")]
  public class ShortcutsSysTrayAddin
    : Xeno.ToolsHub.ExtensionModel.SystemTray.SysTrayAddin // ExtensionModel.ISystemTrayExtension
  {
    private readonly ShortcutsManager _shortcuts;

    public ShortcutsSysTrayAddin()
      : this(new ShortcutsManager())
    {
    }

    public ShortcutsSysTrayAddin(ShortcutsManager shortcuts)
    {
      _shortcuts = shortcuts;
    }

    public override bool IsInitialized => true;

    public string Title => "Shortcuts Add-in";

    public void Execute()
    {
    }

    public override List<ToolStripItem> MenuItems()
    {
      var items = _shortcuts.LoadAsMenuItems();
      return items;
    }
  }
}

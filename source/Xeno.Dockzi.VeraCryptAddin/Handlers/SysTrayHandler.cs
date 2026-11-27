/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-27
 * Author:  Damian Suess
 * File:    SysTrayHandler.cs
 * Description:
 *  System Tray extension point for VeraCrypt add-in
 */

using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.Dockzi.ExtensionModel.SystemTray;

namespace Xeno.Dockzi.VeraCryptAddin.Handlers;

public class SysTrayHandler : Xeno.Dockzi.ExtensionModel.SystemTray.SysTrayAddin
{
  private readonly VeraCryptManager _veraCrypt;

  public SysTrayHandler(VeraCryptManager manager)
  {
    _veraCrypt = manager;
    this.IsInitialized = true;
  }

  public override bool IsInitialized { get; }

  public override List<ToolStripItem> MenuItems()
  {
    var menu = new ToolStripMenuItem(Constants.AddinTitle);
    var mount = new TrayItem("Mount", string.Empty, false, _veraCrypt.OnMount);
    var dismount = new TrayItem("Dismount", string.Empty, false, _veraCrypt.OnDismount);
    var explore = new ToolStripMenuItem("Open in Explorer");
    menu.DropDownItems.AddRange(new ToolStripItem[] { mount, dismount, new ToolStripSeparator(), explore });

    void RefreshState()
    {
      mount.Enabled = _veraCrypt.CanMount;
      dismount.Enabled = _veraCrypt.CanDismount;
      explore.Enabled = false;

      for (int index = explore.DropDownItems.Count - 1; index >= 0; index--)
        explore.DropDownItems[index].Dispose();

      explore.DropDownItems.Clear();
      foreach (string drive in _veraCrypt.GetMountedDrives())
      {
        explore.DropDownItems.Add(new TrayItem(drive, drive, true, _veraCrypt.OnOpenInExplorer));
        explore.Enabled = true;
      }
    }

    menu.DropDownOpening += (_, _) => RefreshState();
    RefreshState();

    return new List<ToolStripItem> { menu };
  }
}

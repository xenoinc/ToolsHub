/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-27
 * Author:  Damian Suess
 * File:    SysTrayHandler.cs
 * Description:
 *  System Tray extension point for VeraCrypt add-in
 */

using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel.SystemTray;

namespace Xeno.ToolsHub.VeraCryptAddin.Handlers
{
  public class SysTrayHandler : Xeno.ToolsHub.ExtensionModel.SystemTray.SysTrayAddin
  {
    private VeraCryptManager _veraCrypt;

    public SysTrayHandler()
    {
      _veraCrypt = new VeraCryptManager();
      this.IsInitialized = true;
    }

    public override bool IsInitialized { get; }

    public override List<MenuItem> MenuItems()
    {
      // TODO: Use MsgCenter to ask VCManager what we have already mounted. Then reflect in display text
      // For now we're just manually mounting/dismounting for first pass

      MenuItem menu = new MenuItem("VeraCrypt");
      menu.MenuItems.Add(0, new TrayItem("Mount", "PATH_TO_HC", true, _veraCrypt.OnMount));
      menu.MenuItems.Add(1, new TrayItem("Dismount", "PATH_TO_HC", true, _veraCrypt.OnDismount));

      return new List<MenuItem>() { menu };
    }
  }
}

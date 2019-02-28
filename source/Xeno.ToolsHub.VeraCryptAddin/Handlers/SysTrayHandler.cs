/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-27
 * Author:  Damian Suess
 * File:    SysTrayHandler.cs
 * Description:
 *  System Tray extension point for VeraCrypt add-in
 */

using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
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
      // TODO: Use Domain.Utils to ask VeraCryptManager if the HC file was already mounted
      // and display a "checked" box if it was.
      // For now we're just manually mounting/dismounting for first pass
      var volume = new Domain.VolumeInfo
      {
        Title = "Sample file",
        FilePath = string.Empty,
        IndexId = Helpers.GenerateGuid(),
        Password = string.Empty
      };

      bool isMounted = false;   // await Domain.DriveUtils.IsMounted(volume.FilePath);

      // When creating a SysTray entry, use the IndexId to tell which drive it is
      // and the display text is, "Title". However below we're just testing individual components
      MenuItem menu = new MenuItem("VeraCrypt");
      menu.MenuItems.Add(0, new TrayItem("Mount", volume.IndexId, true, _veraCrypt.OnMount) { Checked = isMounted });
      menu.MenuItems.Add(1, new TrayItem("Dismount", volume.IndexId, true, _veraCrypt.OnDismount));

      return new List<MenuItem>() { menu };
    }
  }
}

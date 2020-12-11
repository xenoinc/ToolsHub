/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    SysTrayAddin.cs
 * Description:
 *  SystemTray add-in
 *
 *  TODO: Rename to, SysTrayAddinExtension
 */

namespace Xeno.ToolsHub.ExtensionModel.SystemTray
{
  using System.Collections.Generic;
  using System.Windows.Forms;

  ////[Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/SystemTray", NodeName = "SysTrayAddin")]
  public abstract class SysTrayAddin : AbstractAddin
  {
    public abstract bool IsInitialized { get; }

    public abstract List<MenuItem> MenuItems();
  }
}

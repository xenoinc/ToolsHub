/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    SysTrayAddin.cs
 * Description:
 *  SystemTray add-in
 *
 *  TODO: Rename to, SysTrayAddinExtension
 */

using System.Collections.Generic;
using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel.SystemTray
{
  //[Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/SystemTray", NodeName = "SysTrayAddin")]
  public abstract class SysTrayAddin : AbstractAddin
  {
    public abstract bool IsInitialized { get; }

    public abstract void Initialize();

    public abstract List<MenuItem> MenuItems();
  }
}

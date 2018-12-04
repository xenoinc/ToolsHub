/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    SysTrayAddin.cs
 * Description:
 *  SystemTray add-in
 */

using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  //[Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/SystemTray", NodeName = "SysTrayAddin")]
  public abstract class SysTrayAddin : AbstractAddin
  {
    public abstract bool IsInitialized { get; }

    public abstract void Initialize();

    public abstract List<MenuItem> MenuItems();
  }
}

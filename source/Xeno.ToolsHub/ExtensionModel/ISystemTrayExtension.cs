/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-1
 * Author:  <Unknown>
 * File:    ISystemTrayExtension.cs
 * Description:
 *
 */

using System.Collections.Generic;
using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel
{
  //[Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/SystemTray", NodeName = "SysTrayAddin")]
  public interface ISystemTrayExtension : IBaseExtension
  {
    List<MenuItem> MenuItems();
  }
}

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-1
 * Author:  Damian Suess
 * File:    ISystemTrayExtension.cs
 * Description:
 *  System Tray Add-in Extension Point.
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  using System.Collections.Generic;
  using System.Windows.Forms;

  /// <summary>SystemTray Add-in Extension Point.</summary>
  /// <remarks>
  ///   This Extension Points is defined in the file, "ToolsHub.addin.xml".
  ///   An alternative approach is to define the Extension Point via the Assembly Attribute:
  ///   [Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/SystemTray", NodeName = "SysTrayAddin")]
  ///
  ///   Note:
  ///   When we need garbage collection, switch from IBaseExtension to AbstractAddin
  /// </remarks>
  public interface ISystemTrayExtension : IBaseExtension
  {
    List<MenuItem> MenuItems();
  }
}

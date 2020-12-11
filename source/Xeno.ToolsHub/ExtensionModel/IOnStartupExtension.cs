/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    IOnStartupExtension.cs
 * Description:
 *  Add-ins executed immediately on startup
 *
 *  Note: When we need garbage collection, switch from IBaseExtension to AbstractAddin
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  /// <summary>
  ///   OnStartup Add-in Extension Point.
  ///   This type of add-in is executed immediately on application startup.
  /// </summary>
  /// <remarks>
  ///   This Extension Points is defined in the file, "ToolsHub.addin.xml".
  ///   An alternative approach is to define the Extension Point via the Assembly Attribute:
  ///   [Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/OnStartup", NodeName = "OnStartupAddin")]
  ///
  ///   Note:
  ///   When we need garbage collection, switch from IBaseExtension to AbstractAddin
  /// </remarks>
  public interface IOnStartupExtension : IBaseExtension
  {
  }
}

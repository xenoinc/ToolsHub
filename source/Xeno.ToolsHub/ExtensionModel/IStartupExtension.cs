/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    ISetartupExtension.cs
 * Description:
 *
 */

using Mono.Addins;

namespace Xeno.ToolsHub.ExtensionModel
{
  [TypeExtensionPoint(Path = Helpers.ExtensionPaths.AppInitializePath,
                      NodeName = "StartupAddin")]
  public interface IStartupExtension : IBaseExtension
  {
  }
}

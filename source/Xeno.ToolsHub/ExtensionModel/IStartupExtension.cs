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
  [TypeExtensionPoint(Path = Config.ExtensionPaths.OnStartupAddinsPath, NodeName = "OnStartupAddins")]
  public interface IStartupExtension : IBaseExtension
  {
  }
}

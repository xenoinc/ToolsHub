/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    IOnStartupExtension.cs
 * Description:
 *  Add-ins executed immediately on startup
 *
 *  Consider deriving it from AbstractAddin if we need garbage collection
 *  For now, we don't.
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  // We're using XML manifest, so we don't need this
  // [Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/OnStartup", NodeName = "OnStartup")]
  public interface IOnStartupExtension : IBaseExtension
  {
  }
}

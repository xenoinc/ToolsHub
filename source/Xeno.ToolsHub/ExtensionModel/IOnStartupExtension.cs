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
  // We're using XML manifest, so we don't need this
  // [Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/OnStartup", NodeName = "OnStartupAddin")]
  public interface IOnStartupExtension : IBaseExtension
  {
  }
}

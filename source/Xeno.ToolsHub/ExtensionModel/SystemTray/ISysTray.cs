/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-26
 * Author:  Damian Suess
 * File:    ISysTray.cs
 * Description:
 *
 */

//// [assembly: Mono.Addins.ExtensionPoint("/ToolsHub/SysTray")]

namespace Xeno.ToolsHub.ExtensionModel.SystemTray
{
  public interface ISysTray
  {
    string Id { get; }

    string Text { get; }
  }
}

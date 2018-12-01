/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-26
 * Author:  <Unknown>
 * File:    ISysTray.cs
 * Description:
 *
 */

// [assembly: Mono.Addins.ExtensionPoint("/ToolsHub/SysTray")]

namespace Xeno.ToolsHub.Managers.SystemTray
{
  public interface ISysTray
  {
    string Id { get; }

    string Text { get; }
  }
}

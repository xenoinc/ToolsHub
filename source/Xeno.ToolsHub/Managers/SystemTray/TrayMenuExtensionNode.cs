/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    TrayMenuExtensionNode.cs
 * Description:
 *
 */

using Mono.Addins;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  [ExtensionNode("TrayMenu")]
  internal class TrayMenuExtensionNode : ExtensionNode
  {
    [NodeAttribute("Text")]
    public string Text { get; set; }
  }
}

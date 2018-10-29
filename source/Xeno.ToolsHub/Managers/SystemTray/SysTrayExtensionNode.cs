/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    TrayMenuExtensionNode.cs
 * Description:
 *  System Tray extension node creator
 */

using System.Windows.Forms;
using Mono.Addins;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  //[ExtensionNode("SysTray")]  // or "TrayMenu"
  //[ExtensionNodeChild(typeof(SysTrayExtensionNode))]
  internal class SysTrayExtensionNode : TypeExtensionNode
  {
    [NodeAttribute("Text")]
    public string Text { get; set; }

    public MenuItem CreateMenuItem()
    {
      MenuItem menuItem = new MenuItem(Text);
      foreach (var child in ChildNodes)
      {
        ;
        //menuItem.MenuItems.Add(child.CreateMenuItem());
      }

      return menuItem;
    }
  }
}

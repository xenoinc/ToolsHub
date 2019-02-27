/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-27
 * Author:  Damian Suess
 * File:    TrayItemInfo.cs
 * Description:
 *  TrayItem information for passing details about the
 *  SysTray menu item
 */

namespace Xeno.ToolsHub.ExtensionModel.SystemTray
{
  public class TrayItemInfo
  {
    public TrayItemInfo(string addinParent, string itemId, string text, string target, bool enabled)
    {
      AddinParent = addinParent;
      ItemId = itemId;
      Text = text;
      Enabled = enabled;
      Target = target;
    }

    public string AddinParent { get; set; }

    public string ItemId { get; set; }

    public string Text { get; set; }

    public bool Enabled { get; set; }

    public string Target { get; set; }
  }
}

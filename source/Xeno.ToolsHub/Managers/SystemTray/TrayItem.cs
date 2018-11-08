/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    TrayItem.cs
 * Description:
 *  Custom MenuItem handler for extension point nodes
 *  Used by, SystemTrayManager
 */

using System;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  public class TrayItem : MenuItem
  {
    public TrayItem(string text, string tag) : this(text, tag, true)
    {
    }

    public TrayItem(string text, string tag, bool enabled)
    {
      Text = text;
      Tag = tag;
      Enabled = enabled;

      // Event handlers
      Click += OnClick;
      Select += OnSelect;
    }

    public void OnClick(object sender, EventArgs e)
    {
      int index = -1;
      string tag = "<unknown>", text = "";

      if (sender.GetType() == typeof(MenuItem))
      {
        MenuItem item = (MenuItem)sender;
        index = item.Index;
        tag = item.Tag.ToString();
        text = item.Text;
      }

      string dbg = $"TrayItem.OnClick: [ndx={index}] [text={text}] [tag={tag}]";
      Config.Log.Debug(dbg);

      //TODO: (for now) Execute tag as Shortcut's "target" to launch app & such

      //TODO: Send back to add-in so it can execute it's command
    }

    public void OnSelect(object sender, EventArgs e)
    {
      int index = -1;
      string tag = "<unknown>", text = "";

      if (sender.GetType() == typeof(MenuItem))
      {
        MenuItem item = (MenuItem)sender;
        index = item.Index;
        tag = item.Tag.ToString();
        text = item.Text;
      }

      string dbg = $"TrayItem.OnSelect: [ndx={index}] [text={text}] [tag={tag}]";
      Config.Log.Debug(dbg);

      //TODO: Send back to add-in
    }
  }
}

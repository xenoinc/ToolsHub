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
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  public class TrayItem : MenuItem
  {
    private Func<string, int> _routedMethod;

    public TrayItem(string text, string tag) : this(text, tag, true)
    {
    }

    /// <summary>
    ///   Create a SysTray item which will use, Process.Start(tag) by default
    /// </summary>
    /// <param name="text"></param>
    /// <param name="tag"></param>
    /// <param name="enabled"></param>
    public TrayItem(string text, string tag, bool enabled)
      : this(text, tag, enabled, null)
    {
    }

    /// <summary>
    ///   Create SysTray item whose Click action is handled by calling add-in
    /// </summary>
    /// <param name="text"></param>
    /// <param name="tagTarget"></param>
    /// <param name="enabled"></param>
    /// <param name="routedMethod"></param>
    public TrayItem(string text, string tagTarget, bool enabled, Func<string, int> routedMethod)
    {
      Text = text;
      Tag = tagTarget;
      Enabled = enabled;

      _routedMethod = routedMethod;

      // Event handlers
      Click += OnClick;
      Select += OnSelect;
    }

    public void OnClick(object sender, EventArgs e)
    {
      int index = -1;
      string target = "", text = "";

      if (sender.GetType() == typeof(TrayItem))
      {
        TrayItem item = (TrayItem)sender;
        index = item.Index;
        target = item.Tag.ToString(); // Get target path/url. If method, it's blank
        text = item.Text;

        Log.Debug($"TrayItem.OnClickDefault: Executing tag from, " +
                  $"[ndx={index}] [text={text}] [tag={target}]");

        if (_routedMethod != null)
        {
          _routedMethod.Invoke(target);
        }
        else if (target != "")
        {
          System.Diagnostics.Process.Start(target);
        }
        else
        {
          //TODO: Display notification icon bubble with error message
          Log.Error("Unspecified tag detected!");
        }
      }
      else
      {
        Log.Error("Invalid sender object. Must be of type, TrayItem.");
      }

      // Below are suggestions for using ICommand
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
      Log.Debug(dbg);

      //TODO: Send back to add-in
    }
  }
}

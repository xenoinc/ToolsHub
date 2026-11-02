/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-10
 * Author:  Damian Suess
 * File:    Shortcut.cs
 * Description:
 *  Shortcut link to launch
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class ShortcutItem
  {
    public ShortcutItem()
    {
    }

    public ShortcutItem(string title, string target, string args)
    {
      Title = title;
      Target = target;
      Params = args;
    }

    public string Title { get; set; }

    public string Target { get; set; }

    /// <summary>Gets or sets a value indicating the target's execution arguments.</summary>
    public string Params { get; set; }

    // public string StartPath { get; set; }

    // public int SortOrder { get; set; }

    // public string IconPath { get; set; }

    /// <summary>Gets or sets a value indicating whether to execute with administrator privileges.</summary>
    public bool RunAsAdmin { get; set; }

    // OR-----
    /////// <summary>Run as specified user.</summary>
    //// public string RunAs { get; set; }
  }
}

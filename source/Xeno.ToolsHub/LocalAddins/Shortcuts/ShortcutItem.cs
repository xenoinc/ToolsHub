/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-10
 * Author:  Damian Suess
 * File:    Shortcut.cs
 * Description:
 *  Shortcut link to launch
 */

using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class ShortcutItems : List<ShortcutItem>
  {
  }

  public class ShortcutItem
  {
    public string Title { get; set; }

    public string Target { get; set; }

    public string StartPath { get; set; }

    public string Params { get; set; }

    public int SortOrder { get; set; }

    public string IconPath { get; set; }

    public int RunAs { get; set; }
  }
}

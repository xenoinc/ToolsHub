/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-10
 * Author:  Damian Suess
 * File:    Shortcut.cs
 * Description:
 *  Shortcut link to launch
 */

using System.Collections.Generic;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class Shortcuts : List<Shortcut>
  {

  }

  public class Shortcut
  {
    public string Title { get; set; }

    public int SortOrder { get; set; }

    public string Uri { get; set; }

    public string Params { get; set; }

    public string Icon { get; set; }

  }
}

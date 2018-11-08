/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-6
 * Author:  Damian Suess
 * File:    Constants.cs
 * Description:
 *  Global constants
 */

using System.IO;

namespace Xeno.ToolsHub.Config
{
  public static class Constants
  {
    public static string SettingsFile => "config.json";
    public static string LocalSettingsPath => Path.Combine(LocalFolder, SettingsFile);

    public static string ShortcutsFile => "shortcuts.json";
    public static string LocalShortcutsPath => Path.Combine(LocalFolder, ShortcutsFile);

    public static string LocalFolder => Directory.GetCurrentDirectory();
  }
}

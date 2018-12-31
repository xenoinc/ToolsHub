/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-6
 * Author:  Damian Suess
 * File:    Constants.cs
 * Description:
 *  Global constants
 */

namespace Xeno.ToolsHub.Config
{
  using System.IO;

  public static class Constants
  {
    public static string SettingsFile => "ToolsHub.db";

    public static string LocalSettingsPath => Path.Combine(LocalFolder, SettingsFile);

    public static string LocalFolder => Directory.GetCurrentDirectory();
  }
}

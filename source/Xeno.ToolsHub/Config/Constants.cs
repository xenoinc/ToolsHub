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
    /// <summary>Remote URI</summary>
    public const string RemoteUri = @"https://xenoinc.com/dev";

    /// <summary>Remote folder</summary>
    /// <remarks>Must use, INTLAB\Administrator to access</remarks>
    public const string RemoteBase = RemoteUri + @"\toolshub\";

    /// <summary>Squirrel Updater (Beta) Path</summary>
    public static readonly string RemoteUpdateBetaPath = Path.Combine(RemoteBase, "beta");

    public static readonly string RemoteUpdateStablePath = Path.Combine(RemoteBase, "releases");

    public static string SettingsFile => "ToolsHub.db";

    public static string LocalSettingsPath => Path.Combine(LocalFolder, SettingsFile);

    public static string LocalFolder => Directory.GetCurrentDirectory();
  }
}

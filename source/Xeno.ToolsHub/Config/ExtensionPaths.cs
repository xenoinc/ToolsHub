/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Constants.cs
 * Description:
 *  Add-in Extension Paths
 */

namespace Xeno.ToolsHub.Config
{
  public class ExtensionPaths
  {
    // Application directives
    public const string OnStartupAddinsPath = "/ToolsHub/OnStartup";
    public const string SystemShutdownPath = "/ToolsHub/OnSystemShutdown";
    //public const string AppReadyPath = "/ToolsHub/OnAppReady";
    //public const string AppShutdownPath = "/ToolsHub/OnAppShutdown";

    public const string UtilityPath = "/ToolsHub/Utility";

    // Internal add-ins
    public const string SystemTrayPath = "/ToolsHub/SystemTray";
    public const string PreferencePath = "/ToolsHub/PreferenceAddins";
    //public const string SidebarPath = "/ToolsHub/Sidebar";
  }
}

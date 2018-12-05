/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Constants.cs
 * Description:
 *  Add-in Extension and Path names
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public class ExtensionName
  {
    public const string OnStartupAddin = "OnStartupAddin";

    public const string SysTrayAddin = "SysTrayAddin";

    public const string UtilityAddin = "UtilityAddin";
  }

  public class ExtensionPath
  {
    /// <summary>Application is starting up extension command</summary>
    public const string OnStartup = "/ToolsHub/OnStartup";

    /// <summary>Disposable utility extension</summary>
    public const string Utility = "/ToolsHub/Utility";

    /// <summary>System tray extension</summary>
    public const string SystemTray = "/ToolsHub/SystemTray";

    /// <summary>Preference page extension</summary>
    public const string PreferencePage = "/ToolsHub/PreferencePage";

    //public const string SystemShutdownPath = "/ToolsHub/OnSystemShutdown";
    //public const string AppReadyPath = "/ToolsHub/OnAppReady";
    //public const string AppShutdownPath = "/ToolsHub/OnAppShutdown";
    //public const string SidebarPath = "/ToolsHub/Sidebar";
  }
}

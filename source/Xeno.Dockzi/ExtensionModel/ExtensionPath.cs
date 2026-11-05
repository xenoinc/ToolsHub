/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    ExtensionPath.cs
 * Description:
 *  Add-in Extension Path names
 */

namespace Xeno.Dockzi.ExtensionModel
{
  /// <summary>Add-in Extension Path Constants.</summary>
  public class ExtensionPath
  {
    /// <summary>Application is starting up extension command</summary>
    public const string OnStartup = "/Dockzi/OnStartup";

    /// <summary>Disposable utility extension</summary>
    public const string Utility = "/Dockzi/Utility";

    /// <summary>System tray extension</summary>
    public const string SystemTray = "/Dockzi/SystemTray";

    /// <summary>Preference page extension</summary>
    public const string PreferencePage = "/Dockzi/PreferencePage";

    ////public const string SystemShutdownPath = "/Dockzi/OnSystemShutdown";
    ////public const string AppReadyPath = "/Dockzi/OnAppReady";
    ////public const string AppShutdownPath = "/Dockzi/OnAppShutdown";
    ////public const string SidebarPath = "/Dockzi/Sidebar";
  }
}

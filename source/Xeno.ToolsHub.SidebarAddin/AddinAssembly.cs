/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-6
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *
 */

[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "SidebarAddin",
  Category = "Tools",
  CompatVersion = "0.3",
  EnabledByDefault = true,
  Version = "0.3",
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]
[assembly: Mono.Addins.AddinDependency("XenoInnovations.ToolsHub", "0.3")]

namespace Xeno.ToolsHub.SidebarAddin
{
  public static class Constants
  {
    public static string AddinId => "Sidebar";

    public static string KeyShortcuts => "Shortcuts";

    public static string KeyBackground => "BackgroundImage";

    //public static string KeyTintColor => "TintColor";

    /// <summary>Left, top, bottom, right</summary>
    public static string KeyScreenArea => "ScreenSide";

    /// <summary>Vertical, horizontal</summary>
    public static string KeyScreenOrientation => "Orientation";

    /// <summary>Monitor "1" (main)</summary>
    public static string KeyMonitorNumber => "MonitorNumber";
  }
}

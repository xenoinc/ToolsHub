﻿/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-6
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our Sidebar add-in extension
 */

// Define our add-in
using Mono.Addins;

[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "SidebarAddin",
  Version = "0.3",
  Category = "Tools",
  CompatVersion = "0.3",
  EnabledByDefault = false,
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

[assembly:AddinProperty("Name", "Sidebar Add-in")]
//// [assembly:AddinName("Sidebar Add-in")]
[assembly:AddinDescription("Desktop dockable sidebar")]

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("XenoInnovations.ToolsHub", "0.3")]

namespace Xeno.ToolsHub.SidebarAddin
{
  public static class Constants
  {
    public static string AddinId => "Sidebar";

    public static string KeyShortcuts => "Shortcuts";

    public static string KeyBackground => "BackgroundImage";

    public static string KeyOpacity => "Opacity";

    /// <summary>Screen side: None, Left, top, bottom, right</summary>
    public static string KeyScreenArea => "ScreenArea";

    /// <summary>Vertical, horizontal</summary>
    public static string KeyScreenOrientation => "ScreenOrientation";

    /// <summary>Monitor "1" (main)</summary>
    public static string KeyScreenNumber => "ScreenNumber";

    /// <summary>Use show sidebar(s) when ToolsHub starts</summary>
    public static string KeyShowSidebars => "ShowSidebars";

    public static string KeyTintColor => "TintColor";
  }
}

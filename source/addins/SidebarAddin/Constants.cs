/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-6
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our Sidebar add-in extension
 */

namespace Xeno.Dockzi.SidebarAddin;

public static class Constants
{
  public static string AddinId => "Sidebar";

  public static string KeyShortcuts => "Shortcuts";

  public static string KeyLaunchers => "LauncherItems";

  public static string KeyBackground => "BackgroundImage";

  public static string KeyOpacity => "Opacity";

  /// <summary>Screen side: None, Left, top, bottom, right</summary>
  public static string KeyScreenArea => "ScreenArea";

  /// <summary>Vertical, horizontal</summary>
  public static string KeyScreenOrientation => "ScreenOrientation";

  /// <summary>Monitor "1" (main)</summary>
  public static string KeyScreenNumber => "ScreenNumber";

  /// <summary>Use show sidebar(s) when Dockzi starts</summary>
  public static string KeyShowSidebars => "ShowSidebars";

  public static string KeyTintColor => "TintColor";
}

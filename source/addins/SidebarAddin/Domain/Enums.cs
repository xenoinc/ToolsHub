/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    Enums.cs
 * Description:
 *  Enumerations
 *
 * References:
 *  - Animate Window
 *    - https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-animatewindow
 *    - http://www.pinvoke.net/default.aspx/user32/AnimateWindow.html
 *    - http://www.pinvoke.net/default.aspx/Enums/AnimateWindowFlags.html
 */

using System;

namespace Xeno.ToolsHub.SidebarAddin.Domain
{
  /// <summary>AnimateWindow USER32 API</summary>
  /// <example
  ///    AnimateWindow(this.Handle, 500, AnimateWindowFlags.AW_VER_POSITIVE | AnimateWindowFlags.AW_SLIDE);
  /// </example>
  [Flags]
  public enum AnimateWindowFlags
  {
    AW_HOR_POSITIVE = 0x00000001,
    AW_HOR_NEGATIVE = 0x00000002,
    AW_VER_POSITIVE = 0x00000004,
    AW_VER_NEGATIVE = 0x00000008,
    AW_CENTER = 0x00000010,
    AW_HIDE = 0x00010000,
    AW_ACTIVATE = 0x00020000,
    AW_SLIDE = 0x00040000,
    AW_BLEND = 0x00080000
  }

  /// <summary>Docking position</summary>
  public enum DockPosition
  {
    None,
    Left,
    Top,
    Right,
    Bottom
  }

  /// <summary>Toolbar layout orientation</summary>
  public enum ScreenOrientation
  {
    Vertical,
    Horizontal
  }

  /// <summary>On hiding effect via AnimateWindowFlags </summary>
  public enum OnHideEffect
  {
    Slide,
    Fade,
    //CenterShrink
  }

  /// <summary>Toolbar icon OnHover effect</summary>
  public enum IconEffect
  {
    None,
    Umber,
    Bounce,
    Rocking
  }

  /// <summary>Position of toolbar</summary>
  public enum ScreenArea
  {
    None,
    Left,
    Right,
    Top,
    Bottom
  }
}

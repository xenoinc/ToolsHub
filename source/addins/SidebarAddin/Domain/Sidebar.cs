/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-20
 * Author:  Damian Suess
 * File:    Sidebar.cs
 * Description:
 *
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Xeno.ToolsHub.SidebarAddin.Domain
{
  public class Sidebar
  {

    [DllImport("user32")]
    static extern bool AnimateWindow(IntPtr hwnd, int time, AnimateWindowFlags flags);


    public Sidebar()
    {
      AutoHide = true;
      AutoHideDelay = 250;
      AutoHideDuration = 350;

      FontName = "Thoma";
      FontSize = 10;
    }

    public bool AutoHide { get; set; }

    public double AutoHideDelay { get; set; }

    public double AutoHideDuration { get; set; }

    public int AutoHideStyle { get; set; }

    public bool DockPosition { get; set; }

    public string FontName { get; set; }

    public double FontSize { get; set; } = 10;

    public IconEffect IconEffect { get; set; }

    public bool LabelShadow { get; set; } = false;

    public string LabelTextColor { get; set; } = ColorToHex(Color.White);

    public double LeftMargine { get; set; }

    public bool OnMouseOverPopup { get; set; }

    public bool OnTop { get; set; }

    public int Opacity { get; set; } = 90;

    public bool ScreenPositionCenter { get; set; } = false;

    /// <summary>User is able to click and move toolbar body</summary>
    public bool ScreenPositionFixed { get; set; } = true;

    public string TintColor { get; set; } = string.Empty;

    private static string ColorToHex(Color c)
    {
      // return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
      return ColorTranslator.ToHtml(c);
    }
  }
}

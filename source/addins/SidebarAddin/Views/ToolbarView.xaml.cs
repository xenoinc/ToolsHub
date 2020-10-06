/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-6
 * Author:  Damian Suess
 * File:    ToolbarView.xaml.cs
 * Description:
 *
 */

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Xeno.ToolsHub.SidebarAddin.Domain;

namespace Xeno.ToolsHub.SidebarAddin.Views
{
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class ToolbarView : Window
  {
    public ToolbarView()
    {
      InitializeComponent();

      // Test this
      // https://social.msdn.microsoft.com/Forums/sqlserver/en-US/87d02c0a-5258-4bb6-9656-dddfe661c531/pinvoke-window-animation-with-windowstylenone?forum=wpf

      //var win = new Window();
      //IntPtr windowHandle = new WindowInteropHelper(win).Handle;
      //win.WindowStyle = WindowStyle.None;
      //AnimateWindow(windowHandle, 1000, (int)Domain.AnimateWindowFlags.AW_BLEND);
    }

    [DllImport("user32.dll")]
    public static extern bool AnimateWindow(IntPtr hWnd, int time, int flags);

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      //var win = new Window();
      //IntPtr windowHandle = new WindowInteropHelper(win).Handle;
      //win.WindowStyle = WindowStyle.None;
      //AnimateWindow(windowHandle, 500,
      //  (int)AnimateWindowFlags.AW_SLIDE | (int)AnimateWindowFlags.AW_HOR_NEGATIVE | (int)AnimateWindowFlags.AW_HIDE);
      //  // (int)AnimateWindowFlags.AW_ACTIVATE);
      // -----------------------
      IntPtr wnd = new WindowInteropHelper(this).Handle;
      AnimateWindow(wnd, 500, AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_SLIDE);
      // (int)AnimateWindowFlags.AW_SLIDE | (int)AnimateWindowFlags.AW_HOR_NEGATIVE | (int)AnimateWindowFlags.AW_HIDE);
      // (int)AnimateWindowFlags.AW_BLEND | (int)AnimateWindowFlags.AW_VER_NEGATIVE | (int)AnimateWindowFlags.AW_SLIDE );

      // -----------------------

      //System.Windows.Media.Animation.DoubleAnimation dblanim = new System.Windows.Media.Animation.DoubleAnimation();
      //dblanim.To = 200;
      //dblanim.Duration = TimeSpan.FromSeconds(3);
      //this.BeginAnimation(HeightProperty, dblanim);

      // (int)AnimateWindowFlags.AW_SLIDE | (int)AnimateWindowFlags.AW_HOR_NEGATIVE | (int)AnimateWindowFlags.AW_HIDE);
      // (int)AnimateWindowFlags.AW_BLEND | (int)AnimateWindowFlags.AW_VER_NEGATIVE | (int)AnimateWindowFlags.AW_SLIDE );
    }
  }
}

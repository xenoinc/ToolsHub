/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    AnimateForm.cs
 * Description:
 *
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Views
{
  public abstract class AnimateForm : Form
  {
    private readonly bool _useSlideAnimation;
    private readonly int _dwTime = 500;

    public AnimateForm()
      : this(false)
    {
    }

    public AnimateForm(bool useSlideAnimation, int duration = 500)
    {
      _useSlideAnimation = useSlideAnimation;
      _dwTime = duration;
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.</summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      NativeMethods.AnimateWindow(
        this.Handle,
        _dwTime,
        NativeMethods.AW_ACTIVATE | (_useSlideAnimation ? NativeMethods.AW_HOR_POSITIVE | NativeMethods.AW_SLIDE : NativeMethods.AW_BLEND));
    }

    /// <summary>Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.</summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (e.Cancel == false)
      {
        NativeMethods.AnimateWindow(
          this.Handle,
          _dwTime,
          NativeMethods.AW_HIDE | (_useSlideAnimation ? NativeMethods.AW_HOR_NEGATIVE | NativeMethods.AW_SLIDE : NativeMethods.AW_BLEND));
      }
    }

    /// <summary>
    ///   CA1060 compliant Platform Invocation method class.
    ///   See also: https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1060
    /// </summary>
    internal static class NativeMethods
    {
      public const int AW_HIDE = 0X10000;
      public const int AW_ACTIVATE = 0X20000;
      public const int AW_HOR_POSITIVE = 0X1;
      public const int AW_HOR_NEGATIVE = 0X2;
      public const int AW_SLIDE = 0X40000;
      public const int AW_BLEND = 0X80000;

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
    }
  }
}

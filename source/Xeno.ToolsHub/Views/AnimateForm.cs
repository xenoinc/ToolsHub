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
    private const int AW_HIDE = 0X10000;
    private const int AW_ACTIVATE = 0X20000;
    private const int AW_HOR_POSITIVE = 0X1;
    private const int AW_HOR_NEGATIVE = 0X2;
    private const int AW_SLIDE = 0X40000;
    private const int AW_BLEND = 0X80000;

    private bool _useSlideAnimation;
    private int _dwTime = 500;

    public AnimateForm()
      : this(false)
    {
    }

    public AnimateForm(bool useSlideAnimation, int duration = 500)
    {
      _useSlideAnimation = useSlideAnimation;
      _dwTime = duration;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      AnimateWindow(this.Handle, _dwTime, AW_ACTIVATE | (_useSlideAnimation ? AW_HOR_POSITIVE | AW_SLIDE : AW_BLEND));
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      if (e.Cancel == false)
      {
        AnimateWindow(this.Handle, _dwTime, AW_HIDE | (_useSlideAnimation ? AW_HOR_NEGATIVE | AW_SLIDE : AW_BLEND));
      }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int AnimateWindow(IntPtr hwand, int dwTime, int dwFlags);
  }
}

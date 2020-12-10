/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    MainForm.cs
 * Description:
 *  Main application handler. We don't need a GUI form, but do need WndProc
 *
 * DEPRECATED: See, WndProcManager
 */

using System;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Views
{
  /// <summary>
  ///   Dead-code ahead!
  ///   We're now using, MainHandler and WinProcManager.
  /// </summary>
  public partial class MainForm : Form
  {
    private static bool _systemShutdown = false;

    public MainForm()
    {
      InitializeComponent();

      this.ShowInTaskbar = false;
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == NativeMethods.WM_QUERYENDSESSION)
      {
        // System is about to log-off, shutdown, or reboot
        _systemShutdown = true;
      }

      // if this is WM_QUERYENDSESSION, then closing should be raised in WndProc
      base.WndProc(ref m);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_systemShutdown)
      {
        // Reset the variable because the user might cancel the shutdown
        _systemShutdown = false;

        if (MessageBox.Show("My application", "Do you want to save your work before logging off?", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          e.Cancel = true;
        }
        else
        {
          e.Cancel = false;
        }
      }
    }

    /// <summary>CA1060 compliant class for Native Methods.</summary>
    private class NativeMethods
    {
      public const int WM_QUERYENDSESSION = 0x11;
    }
  }
}

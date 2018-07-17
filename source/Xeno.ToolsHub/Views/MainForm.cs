/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    MainForm.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Views
{
  public partial class MainForm : Form
  {
    private const int WM_QUERYENDSESSION = 0x11;
    private static bool _systemShutdown = false;

    public MainForm()
    {
      InitializeComponent();
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_QUERYENDSESSION)
      {
        // System is about to logoff, shutdown, or reboot
        _systemShutdown = true;
      }

      // if this is WM_QUERYENDSESSION, then closing een should be raised in WndProc
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

        //TODO: 
        if (DialogResult.Yes == MessageBox.Show("My application", "Do you want to save your work before logging off?", MessageBoxButtons.YesNo))
        {
          e.Cancel = true;
        }
        else
        {
          e.Cancel = false;
        }
      }
    }
  }
}

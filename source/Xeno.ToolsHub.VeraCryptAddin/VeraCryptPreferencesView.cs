/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-8-13
 * Author:  Damian Suess
 * File:    VeraCryptPreferencesView.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public partial class VeraCryptPreferencesView : UserControl
  {
    public VeraCryptPreferencesView()
    {
      InitializeComponent();

      var shutdown = Xeno.ToolsHub.Config.AddinSettings.Load("VeraCrypt", "AutoDismountShutdown", "0");
      var logoff = Xeno.ToolsHub.Config.AddinSettings.Load("VeraCrypt", "AutoDismountSignout", "0");

      chkDismountShutdown.Checked = (shutdown == "1" ? true : false);
      chkDismountSignout.Checked = (logoff == "1" ? true : false);
    }

    private void chkDismountShutdown_CheckedChanged(object sender, EventArgs e)
    {
      string value = chkDismountShutdown.Checked ? "1" : "0";
      VeraCryptAddin.VeraCrypt.SettingSave("AutoDismountShutdown", value);
    }

    private void chkDismountSignout_CheckedChanged(object sender, EventArgs e)
    {
      string value = chkDismountShutdown.Checked ? "1" : "0";
      VeraCryptAddin.VeraCrypt.SettingSave("AutoDismountSignout", value);
    }
  }
}

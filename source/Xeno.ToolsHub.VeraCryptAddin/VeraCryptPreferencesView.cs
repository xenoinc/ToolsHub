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

      bool shutdown = Xeno.ToolsHub.Helpers.AddinSettings.Load("VeraCrypt", "AutoDismountShutdown", false);
      bool logoff = Xeno.ToolsHub.Helpers.AddinSettings.Load("VeraCrypt", "AutoDismountSignout", false);
    }

    private void chkDismountShutdown_CheckedChanged(object sender, EventArgs e)
    {
      // save
      Xeno.ToolsHub.Helpers.AddinSettings.Save("VeraCrypt")
    }

    private void chkDismountSignout_CheckedChanged(object sender, EventArgs e)
    {
      // save
    }
  }
}

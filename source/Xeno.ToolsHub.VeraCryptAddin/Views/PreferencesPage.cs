/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *  Sample XML Add-in Preference Page handler
 */

namespace Xeno.ToolsHub.VeraCryptAddin.views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;

  public partial class PreferencesPage : Form, IPreferencePageForm
  {
    private bool _isModified = false;

    public PreferencesPage()
    {
      InitializeComponent();
      var shutdown = Xeno.ToolsHub.Services.SettingsService.GetValue("VeraCrypt", "AutoDismountShutdown", "0");
      var logoff = Xeno.ToolsHub.Services.SettingsService.GetValue("VeraCrypt", "AutoDismountSignout", "0");

      chkDismountShutdown.Checked = shutdown == "1" ? true : false;
      chkDismountSignout.Checked = logoff == "1" ? true : false;
    }

    public bool IsModified => throw new NotImplementedException();

    public bool OnSave()
    {
      VeraCryptAddin.VeraCrypt.SettingSave("AutoDismountSignout", chkDismountSignout.Checked ? "1" : "0");
      VeraCryptAddin.VeraCrypt.SettingSave("AutoDismountShutdown", chkDismountShutdown.Checked ? "1" : "0");
      _isModified = false;
      return true;
    }

    private void chkDismountShutdown_CheckedChanged(object sender, EventArgs e)
    {
      _isModified = true;
    }

    private void chkDismountSignout_CheckedChanged(object sender, EventArgs e)
    {
      _isModified = true;
    }

    private void PreferencesPage_Load(object sender, EventArgs e)
    {
    }
  }
}

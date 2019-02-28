/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *  Sample XML Add-in Preference Page handler
 */

namespace Xeno.ToolsHub.VeraCryptAddin.Views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services;

  public partial class PreferencesPage : Form, IPreferencePageForm
  {
    private VeraCryptManager _manager;

    public PreferencesPage()
    {
      InitializeComponent();
      this.Text = "VeraCrypt";

      _manager = new VeraCryptManager();
      OnLoadSettings();

      IsModified = false;
    }

    public bool IsModified { get; set; }

    public bool OnSave()
    {
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcDriveLetter, TxtHcDrive.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcPath, TxtHcPath.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcPass, TxtHcPass.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnStartMount, ChkOnStartMount.Checked.ToString());
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnExitDismount.Checked.ToString());

      // Features not implemented yet
      //// SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnShutdownDismount.Checked.ToString());
      //// SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnSignoutDismount.Checked.ToString());

      return true;
    }

    private void OnLoadSettings()
    {
      TxtHcDrive.Text = _manager.SettingHcDrive.ToString();
      TxtHcPath.Text = _manager.SettingHcPath.ToString();
      TxtHcPass.Text = _manager.SettingHcPass.ToString();

      ChkOnStartMount.Checked = _manager.SettingOnStartMount;
      ChkOnExitDismount.Checked = _manager.SettingOnExitDismount;
    }

    private void PreferencesPage_Load(object sender, EventArgs e)
    {
    }

    private void ChkOnShutdownDismount_CheckedChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void ChkOnSignoutDismount_CheckedChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void ChkOnStartMount_CheckedChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void ChkOnExitDismount_CheckedChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      IsModified = true;
    }
  }
}

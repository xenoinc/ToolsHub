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
  using System.IO;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services;

  public partial class PreferencesPage : Form, IPreferencePageActions
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
      SettingsService.SetValue(Constants.AddinId, Constants.KeyInstallPath, TxtInstallPath.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyForceDismounts, ChkForceDismounts.Checked.ToString());

      // TODO: Save list of volume end-points
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcDriveLetter, CmboDrives.Text);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcPath, _manager.SettingEncrypt(TxtHcPath.Text));
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcPass, _manager.SettingEncrypt(TxtHcPass.Text));

      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnStartMount, ChkOnStartMount.Checked.ToString());
      SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnExitDismount.Checked.ToString());

      // Features not implemented yet
      //// SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnShutdownDismount.Checked.ToString());
      //// SettingsService.SetValue(Constants.AddinId, Constants.KeyHcOnExitDismount, ChkOnSignoutDismount.Checked.ToString());

      return true;
    }

    private void OnLoadSettings()
    {
      TxtInstallPath.Text = _manager.SettingInstallPath;
      ChkForceDismounts.Checked = _manager.SettingsForceDismounts;

      CmboDrives.Text = _manager.SettingHcDrive;
      TxtHcPath.Text = _manager.SettingHcPath;
      TxtHcPass.Text = _manager.SettingHcPass;

      ChkOnStartMount.Checked = _manager.SettingOnStartMount;
      ChkOnExitDismount.Checked = _manager.SettingOnExitDismount;
      ChkOnShutdownDismount.Checked = false;
      ChkOnSignoutDismount.Checked = false;

      var drives = _manager.GetAvailableDrives();
      CmboDrives.Items.AddRange(drives.ToArray());
    }

    private void PreferencesPage_Load(object sender, EventArgs e)
    {
    }

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
      IsModified = true;
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void Check_CheckedChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void BtnInstallManualFind_Click(object sender, EventArgs e)
    {
      IsModified = true;
      using (var dialog = new OpenFileDialog
      {
        Filter = "VeraCrypt.exe (*.exe)|*.exe|All files (*.*)|*.*",
        DefaultExt = "exe",
        CheckPathExists = true,
        InitialDirectory = @"C:\Program Files\VeraCrypt\",
        Multiselect = false
      })
      {
        DialogResult result = dialog.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
        {
          TxtInstallPath.Text = dialog.FileName;
        }
      }
    }

    private void BtnInstallAutoFind_Click(object sender, EventArgs e)
    {
      IsModified = true;
      string pth = _manager.GetInstallPath().Trim();
      if (!string.IsNullOrEmpty(pth))
        TxtInstallPath.Text = pth;
    }

    private void BtnHcPath_Click(object sender, EventArgs e)
    {
      using (var dialog = new OpenFileDialog
      {
        Filter = "VeraCrypt Volume (*.hc)|*.hc|All files (*.*)|*.*",
        DefaultExt = "hc",
        FilterIndex = 1,
        CheckPathExists = true,
        ValidateNames = false,  // Suppress file warning
        Multiselect = false
      })
      {
        if (Directory.Exists(_manager.GetInstallPath()))
          dialog.InitialDirectory = @"C:\Program Files\VeraCrypt\";

        DialogResult result = dialog.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.FileName))
        {
          TxtHcPath.Text = dialog.FileName;
        }
      }
    }

    private void BtnTestCipher_Click(object sender, EventArgs e)
    {
      string pth = @"C:\TEMP\some path";

      var c = new Domain.Crypto();

      var enc = c.Encrypt(pth);
      Console.WriteLine("Encoded: " + enc);

      var dec = c.Decrypt(enc);

      var status = dec != pth ? "Failed to decrypt" : "Passed!";
      lblCipherTest.Text = status;
      Console.WriteLine(status);
    }
  }
}

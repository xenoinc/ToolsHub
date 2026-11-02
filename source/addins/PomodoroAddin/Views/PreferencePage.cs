/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferencePage.cs
 * Description:
 *  🍅 Pomodoro preferences page
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using PomodoroAddin.Domain;
using PomodoroAddin.Managers;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Services;

namespace PomodoroAddin.Views
{
  public partial class PreferencePage : Form, IPreferencePageActions
  {
    private readonly PomodoroManager _manager;
    private readonly ISettingsService _settings;

    public PreferencePage(PomodoroManager manager, ISettingsService settings)
    {
      _manager = manager;
      _settings = settings;
      InitializeComponent();
      this.Text = "Pomodoro Timer 🍅";

      LoadSettings();

      TxtDuration.TextChanged += MarkModified;
      TxtBreakShort.TextChanged += MarkModified;
      TxtBreakLong.TextChanged += MarkModified;
      ChkPlaySound.CheckedChanged += MarkModified;
      ChkFlashScreenEvents.CheckedChanged += MarkModified;
      ChkSysTrayBubbles.CheckedChanged += MarkModified;
      ChkSysTrayDurations.CheckedChanged += MarkModified;
      IsModified = false;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsModified { get; set; }

    public bool OnSave()
    {
      _settings.SetValue(Constants.AddinId, Constants.KeyDuration, TxtDuration.Text);
      _settings.SetValue(Constants.AddinId, Constants.KeyShortBreak, TxtBreakShort.Text);
      _settings.SetValue(Constants.AddinId, Constants.KeyLongBreak, TxtBreakLong.Text);

      _settings.SetValue(Constants.AddinId, Constants.KeyAlertSound, ChkPlaySound.Checked.ToString());
      _settings.SetValue(Constants.AddinId, Constants.KeyAlertFlash, ChkFlashScreenEvents.Checked.ToString());
      _settings.SetValue(Constants.AddinId, Constants.KeyAlertSysTrayBubble, ChkSysTrayBubbles.Checked.ToString());
      _settings.SetValue(Constants.AddinId, Constants.KeyTrayShowDuration, ChkSysTrayDurations.Checked.ToString());

      _manager.RefreshMenu();
      IsModified = false;
      return true;
    }

    private void MarkModified(object sender, EventArgs args) => IsModified = true;

    private void PreferencePage_Load(object sender, EventArgs e)
    {
    }

    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      // Currently, NO decimals places

      if (!IsValidateNumber(e.KeyChar))
      {
        e.Handled = true;
        return;
      }

      IsModified = true;

      ////if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
      ////{
      ////  e.Handled = true;
      ////  return;
      ////}
      ////
      ////// Only allow one decimal point
      ////if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
      ////{
      ////  e.Handled = true;
      ////  return;
      ////}
    }

    private void BtnTestSound_Click(object sender, EventArgs e)
    {
      _manager.PlaySound(1, true);
    }

    private void BtnTestTrayIconUpdate_Click(object sender, EventArgs e)
    {
      // string tIcon = "🍅"; // Shows up as a square on Win10 due to a bug in .Net GDI
      string sigma = "Σ";
      IconGenerator icon = new IconGenerator(sigma);
      IconGenerator[] nIcon;
      nIcon = new IconGenerator[5];

      _manager.SendMessageTrayIcon(icon.ToIcon());
      Xeno.ToolsHub.Config.Helpers.Timeout(700);

      for (int i = 1; i <= 5; i++)
      {
        nIcon[i - 1] = new IconGenerator(i.ToString());

        _manager.SendMessageTrayIcon(nIcon[i - 1].ToIcon());
        Xeno.ToolsHub.Config.Helpers.Timeout(500);
      }

      // Go back to default
      _manager.SendMessageTrayIcon(null);
    }

    /// <summary>Validate if key is a valid number or not</summary>
    /// <param name="key"></param>
    /// <param name="allowDecimals"></param>
    /// <returns></returns>
    private bool IsValidateNumber(char key, bool allowDecimals = false)
    {
      bool isValid = true;

      if (!char.IsControl(key) && !char.IsDigit(key))
        isValid = false;

      if (allowDecimals && (key == '.'))
        isValid = false;

      return isValid;
    }

    /// <summary>Update GUI with settings</summary>
    private void LoadSettings()
    {
      TxtDuration.Text = _manager.SettingTimerDuration.ToString();
      TxtBreakShort.Text = _manager.SettingTimerShortBreak.ToString();
      TxtBreakLong.Text = _manager.SettingTimerLongBreak.ToString();

      ChkPlaySound.Checked = _manager.SettingAlertSound;
      ChkFlashScreenEvents.Checked = _manager.SettingAlertFlash;
      ChkSysTrayBubbles.Checked = _manager.SettingAlertTrayBubble;
      ChkSysTrayDurations.Checked = _manager.SettingTrayIconUpdates;
    }
  }
}

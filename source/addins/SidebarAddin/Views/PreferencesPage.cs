/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *  Sidebar preferences page
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.SidebarAddin.Managers;

namespace Xeno.ToolsHub.SidebarAddin.Views;

/// <summary>Preferences Page.</summary>
public partial class PreferencesPage : Form, IPreferencePageActions
{
  private readonly SidebarManager _manager;
  private readonly ISettingsService _settings;

  public PreferencesPage(SidebarManager manager, ISettingsService settings)
  {
    _manager = manager;
    _settings = settings;
    InitializeComponent();
    Text = "Sidebar";
    OnLoadSettings();
    ComboScreenArea.SelectedIndexChanged += MarkModified;
    ComboScreenOrientaiton.SelectedIndexChanged += MarkModified;
    UpDownScreenNumber.ValueChanged += MarkModified;
    Opacity.ValueChanged += MarkModified;
    ChkShowOnStart.CheckedChanged += MarkModified;
    IsModified = false;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool IsModified { get; set; }

  public bool OnSave()
  {
    _settings.SetValue(Constants.AddinId, Constants.KeyOpacity, Opacity.Value);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenArea, ComboScreenArea.SelectedIndex);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenOrientation, ComboScreenOrientaiton.SelectedIndex);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenNumber, (int)UpDownScreenNumber.Value);
    _settings.SetValue(Constants.AddinId, Constants.KeyShowSidebars, ChkShowOnStart.Checked);
    _manager.SendMessageRefreshShortcuts();
    IsModified = false;

    return true;
  }

  private void OnLoadSettings()
  {
    ComboScreenArea.SelectedIndex = Math.Clamp(_manager.SettingScreenArea, 0, ComboScreenArea.Items.Count - 1);
    ComboScreenOrientaiton.SelectedIndex = Math.Clamp(_manager.SettingScreenOrientation, 0, ComboScreenOrientaiton.Items.Count - 1);
    UpDownScreenNumber.Value = Math.Clamp(_manager.SettingScreenNumber, UpDownScreenNumber.Minimum, UpDownScreenNumber.Maximum);
    Opacity.Value = Math.Clamp(_manager.SettingOpacity, Opacity.Minimum, Opacity.Maximum);
    ChkShowOnStart.Checked = _manager.SettingsShowSidebar;
  }

  private void MarkModified(object sender, EventArgs args) => IsModified = true;

  private void PreferencesPage_Load(object sender, EventArgs e)
  {
  }
}

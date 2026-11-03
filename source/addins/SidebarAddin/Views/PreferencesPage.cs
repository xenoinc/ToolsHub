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
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Managers;

namespace Xeno.ToolsHub.SidebarAddin.Views;

/// <summary>Preferences Page.</summary>
public partial class PreferencesPage : Form, IPreferencePageActions
{
  private readonly SidebarManager _manager;
  private readonly ISettingsService _settings;
  private bool _previewed;

  public PreferencesPage(SidebarManager manager, ISettingsService settings)
  {
    _manager = manager;
    _settings = settings;
    InitializeComponent();
    Text = "Sidebar";
    ComboScreenArea.Enabled = true;
    ComboScreenArea.Items[0] = "Left (default)";
    UpDownScreenNumber.Enabled = true;
    UpDownScreenNumber.Maximum = Math.Max(1, Screen.AllScreens.Length);
    ChkShowOnStart.Enabled = true;
    ChkOnTop.Enabled = true;
    ChkAutoHide.Enabled = true;
    OnLoadSettings();
    ComboScreenArea.SelectedIndexChanged += (sender, args) =>
    {
      ComboScreenOrientaiton.SelectedIndex = ComboScreenArea.SelectedIndex >= 3 ? 1 : 0;
      MarkModified(sender, args);
    };
    ComboScreenOrientaiton.SelectedIndexChanged += MarkModified;
    UpDownScreenNumber.ValueChanged += MarkModified;
    Opacity.ValueChanged += MarkModified;
    ChkShowOnStart.CheckedChanged += MarkModified;
    ChkOnTop.CheckedChanged += MarkModified;
    ChkAutoHide.CheckedChanged += MarkModified;
    Disposed += (_, _) =>
    {
      if (_previewed)
        _manager.ApplySettings();
    };
    IsModified = false;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool IsModified { get; set; }

  public bool OnSave()
  {
    _settings.SetValue(Constants.AddinId, Constants.KeyOpacity, Opacity.Value);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenArea, ComboScreenArea.SelectedIndex);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenOrientation, ComboScreenArea.SelectedIndex >= 3 ? 1 : 0);
    _settings.SetValue(Constants.AddinId, Constants.KeyScreenNumber, (int)UpDownScreenNumber.Value);
    _settings.SetValue(Constants.AddinId, Constants.KeyShowSidebars, ChkShowOnStart.Checked);
    _settings.SetValue(Constants.AddinId, "OnTop", ChkOnTop.Checked);
    _settings.SetValue(Constants.AddinId, "AutoHide", ChkAutoHide.Checked);
    _manager.ApplySettings();
    IsModified = false;

    return true;
  }

  private void OnLoadSettings()
  {
    ComboScreenArea.SelectedIndex = Math.Clamp(_manager.SettingScreenArea, 0, ComboScreenArea.Items.Count - 1);
    ComboScreenOrientaiton.SelectedIndex = ComboScreenArea.SelectedIndex >= 3 ? 1 : 0;
    UpDownScreenNumber.Value = Math.Clamp(_manager.SettingScreenNumber, UpDownScreenNumber.Minimum, UpDownScreenNumber.Maximum);
    Opacity.Value = Math.Clamp(_manager.SettingOpacity, Opacity.Minimum, Opacity.Maximum);
    ChkAutoHide.Checked = _settings.GetBool(Constants.AddinId, "AutoHide", true);
    ChkOnTop.Checked = _settings.GetBool(Constants.AddinId, "OnTop", true);
    ChkShowOnStart.Checked = _manager.SettingsShowSidebar;
  }

  private void MarkModified(object sender, EventArgs args)
  {
    IsModified = true;
    _previewed = true;
    _manager.PreviewSettings(new SidebarAppearance(ComboScreenArea.SelectedIndex,
      (int)UpDownScreenNumber.Value, Opacity.Value, ChkOnTop.Checked, ChkAutoHide.Checked));
  }

  private void PreferencesPage_Load(object sender, EventArgs e)
  {
  }
}

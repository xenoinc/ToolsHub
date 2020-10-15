/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *  Sidebar preferences page
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.SidebarAddin.Managers;

namespace Xeno.ToolsHub.SidebarAddin.Views
{
  public partial class PreferencesPage : Form, IPreferencePageActions
  {
    private SidebarManager _manager;

    public PreferencesPage()
    {
      InitializeComponent();

      ComboScreenArea.SelectedIndex = 1;
      ComboScreenOrientaiton.SelectedIndex = 0;
      _manager = new SidebarManager();
    }

    public bool IsModified { get; set; }

    public bool OnSave()
    {
      SettingsService.SetValue(Constants.AddinId, Constants.KeyBackground, ComboScreenArea.SelectedIndex);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyOpacity, Opacity.Value);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyScreenArea, ComboScreenArea.SelectedIndex);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyScreenOrientation, ComboScreenArea.SelectedIndex);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyScreenNumber, ComboScreenArea.SelectedIndex);
      SettingsService.SetValue(Constants.AddinId, Constants.KeyShowSidebars, ChkShowOnStart.Checked);
      //  SettingsService.SetValue(Constants.AddinId, Constants.KeyTintColor, TxtTintColorHex);

      return true;
    }

    private void OnLoadSettings()
    {
      // Set checkboxes & gui stuff
      ////_manager.SettingBackground;
      ////_manager.SettingOpacity;
      ////_manager.SettingScreenArea;
      ////_manager.SettingScreenNumber;
      ////_manager.SettingScreenOrientation;
      ////_manager.SettingTintColor;
    }

    private void PreferencesPage_Load(object sender, EventArgs e)
    {
    }
  }
}

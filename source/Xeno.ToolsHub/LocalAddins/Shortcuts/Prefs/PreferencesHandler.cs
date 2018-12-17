﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-11
 * Author:  Damian Suess
 * File:    PreferencesHandler.cs
 * Description:
 *  Add-in handler for PreferencePage
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  public class PreferencesHandler : PreferencePageExtension
  {
    private PreferencePageCtrl _page;

    public override string Title => "Shortcuts";

    public override Control Page => _page;

    public override bool IsModified => _page.IsModified;

    public override void InitializePage()
    {
      _page = new PreferencePageCtrl();
    }

    public override void OnSave()
    {
      _page.OnSave(); // Save settings was initiated
    }
  }
}

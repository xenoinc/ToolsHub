/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.cs
 * Description:
 *  Shortcuts preference window
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  public partial class ShortcutsPreferences : Form, IPreferencePageForm
  {
    private bool _isModified = false;

    public ShortcutsPreferences()
    {
      InitializeComponent();
    }

    public bool IsModified => _isModified;

    public void OnSave()
    {
      _isModified = false;
    }

    private void ShortcutsPreferences_Load(object sender, EventArgs e)
    {
    }

    private void TxtRawFile_TextChanged(object sender, EventArgs e)
    {
      _isModified = true;
    }
  }
}

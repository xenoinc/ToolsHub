/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.cs
 * Description:
 *  Shortcuts preference window
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class ShortcutsPreferences : Form, IPreferencePageForm
  {
    private bool _isModified = false;

    private ShortcutsManager _shortcuts = new ShortcutsManager();

    public ShortcutsPreferences()
    {
      InitializeComponent();
    }

    public bool IsModified
    {
      get { return _isModified; }

      set
      {
        if (_isModified != value)
        {
          _isModified = value;
          LblModified.Visible = value;
        }
      }
    }

    public bool OnSave()
    {
      string json = TxtRawFile.Text;
      string errMsg = "";

      IsModified = false;

      if (_shortcuts.ValidateJson(json, out errMsg))
      {
        _shortcuts.Save(json);
        return true;
      }
      else
      {
        Log.Error("Your JSON formatting is bad, and you should feel bad too!" + Environment.NewLine + errMsg);
        return false;
      }
    }

    private void LoadShortcutsFile()
    {
      TxtRawFile.Text = _shortcuts.ToString();
    }

    private void ShortcutsPreferences_Load(object sender, EventArgs e)
    {
      LoadShortcutsFile();
    }

    private void TxtRawFile_TextChanged(object sender, EventArgs e)
    {
      IsModified = true;
      LblModified.Visible = true;
    }
  }
}

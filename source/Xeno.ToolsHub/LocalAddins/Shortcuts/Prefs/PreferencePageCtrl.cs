/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-11
 * Author:  Damian Suess
 * File:    PreferencePageCtrl.cs
 * Description:
 *  Shortcuts preference window
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  public partial class PreferencePageCtrl : UserControl
  {
    public PreferencePageCtrl()
    {
      InitializeComponent();
    }

    public void Save()
    {
      Log.Debug("Saving shortcuts.json... (not implemented)");
    }

    private void PreferencePageCtrl_Load(object sender, EventArgs e)
    {
      TxtRawFile.Text = LoadRawFile();
    }

    private string LoadRawFile()
    {
      Log.Debug("Loading shortcuts.json... (not implemented)");
      return "";
    }
  }
}

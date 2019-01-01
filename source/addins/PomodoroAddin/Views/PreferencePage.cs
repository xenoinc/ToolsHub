/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferencePage.cs
 * Description:
 *  Preferences page
 */

namespace PomodoroAddin.Views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class PreferencePage : Form, IPreferencePageForm
  {
    public PreferencePage()
    {
      InitializeComponent();
    }

    public bool IsModified { get; }

    public bool OnSave()
    {
      Log.Debug("Mock save of Pomodoro's settings. " +
        "This shouldn't ever get hit because we never modify anything");

      return true;
    }

    private void PreferencePage_Load(object sender, EventArgs e)
    {
    }
  }
}

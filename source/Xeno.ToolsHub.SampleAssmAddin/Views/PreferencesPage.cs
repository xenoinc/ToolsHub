/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.SampleAssmAddin.Views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class PreferencesPage : Form, IPreferencePageForm
  {
    public PreferencesPage()
    {
      InitializeComponent();
    }

    public bool IsModified { get; }

    public bool OnSave()
    {
      Log.Debug("Mock save, SampleAssm's settings. " +
        "This shouldn't ever get hit because we never modify anything");
      return true;
    }

    private void SampleAssmPreferencesPage_Load(object sender, EventArgs e)
    {
    }
  }
}

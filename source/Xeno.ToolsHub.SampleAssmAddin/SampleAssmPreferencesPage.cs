/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleAssmPreferencesPage.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleAssmAddin
{
  public partial class SampleAssmPreferencesPage : Form, IPreferencePageForm
  {
    public SampleAssmPreferencesPage()
    {
      InitializeComponent();
    }

    public bool IsModified { get; }

    public void OnSave()
    {
      Log.Debug("Mock save, SampleAssm's settings. " +
        "This shouldn't ever get hit because we never modify anything");
    }

    private void SampleAssmPreferencesPage_Load(object sender, EventArgs e)
    {
    }
  }
}

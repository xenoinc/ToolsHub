/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleAssmAddin.Views
{
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

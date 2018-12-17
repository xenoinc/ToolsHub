/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferencePagePanel.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleAssmAddin
{
  public partial class SampleAssmPreferencesCtrl : UserControl, IPreferencePageForm
  {
    public SampleAssmPreferencesCtrl()
    {
      InitializeComponent();
    }

    public bool IsModified { get; }

    public void OnSave()
    {
      throw new NotImplementedException();
    }

    private void button1_Click(object sender, EventArgs e)
    {
    }

    private void button2_Click(object sender, EventArgs e)
    {
    }

    private void PreferencePagePanel_Load(object sender, EventArgs e)
    {
    }
  }
}

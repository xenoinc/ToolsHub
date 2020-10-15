/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleXmlPerferencePage.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.SampleXmlAddin.Views
{
  using System;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class PerferencePage : Form, IPreferencePageActions
  {
    public PerferencePage()
    {
      InitializeComponent();
    }

    public bool IsModified { get; set; }

    public bool OnSave()
    {
      Log.Debug("Mock save, SampleXML's settings.");

      IsModified = false;
      lblIsModified.Text = "False";
      return true;
    }

    private void SampleXmlPerferencePage_Load(object sender, EventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (!IsModified)
        lblIsModified.Text = "True";

      IsModified = true;
    }
  }
}

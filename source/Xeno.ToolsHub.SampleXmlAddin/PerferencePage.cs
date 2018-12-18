﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    SampleXmlPerferencePage.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public partial class PerferencePage : Form, IPreferencePageForm
  {
    private bool _isModified = false;

    public PerferencePage()
    {
      InitializeComponent();
    }

    public bool IsModified => _isModified;

    public void OnSave()
    {
      Log.Debug("Mock save, SampleXML's settings.");

      _isModified = false;
      lblIsModified.Text = "False";
    }

    private void SampleXmlPerferencePage_Load(object sender, EventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (!IsModified)
        lblIsModified.Text = "True";

      _isModified = true;
    }
  }
}

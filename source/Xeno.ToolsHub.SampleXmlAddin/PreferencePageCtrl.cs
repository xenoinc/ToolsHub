/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-7
 * Author:  Damian Suess
 * File:    PreferencePageCtrl.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public partial class PreferencePageCtrl : UserControl, IPreferencePageForm
  {
    public PreferencePageCtrl()
    {
      InitializeComponent();
    }

    public bool IsModified { get; }

    public void OnSave()
    {
      throw new NotImplementedException();
    }
  }
}

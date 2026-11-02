/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    PreferencesPage.cs
 * Description:
 *  Sample preference page with MessagingCenter
 */

namespace Xeno.Dockzi.SampleAssmAddin.Views
{
  using System;
  using System.ComponentModel;
  using System.Windows.Forms;
  using Xeno.Dockzi.ExtensionModel;
  using Xeno.Dockzi.ExtensionModel.SystemTray;
  using Xeno.Dockzi.Services.Logging;
  using Xeno.Dockzi.Services.Messaging;

  public partial class PreferencesPage : Form, IPreferencePageActions
  {
    public PreferencesPage()
    {
      InitializeComponent();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool IsModified { get; set; }

    public bool OnSave()
    {
      Log.Debug("Mock save, SampleAssm's settings. " +
        "This shouldn't ever get hit because we never modify anything");
      return true;
    }

    private void SampleAssmPreferencesPage_Load(object sender, EventArgs e)
    {
    }

    private void BtnRefreshSysTray_Click(object sender, EventArgs e)
    {
      MessagingCenter.Send<SystemTrayMessages>(new SystemTrayMessages(), SystemTrayMessages.Refresh);
    }

    private void BtnSysTrayNotify_Click(object sender, EventArgs e)
    {
      MessagingCenter.Send<SystemTrayMessages, string>(
        new SystemTrayMessages(),
        SystemTrayMessages.Notify,
        TxtSysTrayMessage.Text);
    }
  }
}

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    OptionsForm.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Domain;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Helpers;

namespace Xeno.ToolsHub.Views
{
  public partial class PreferencesForm : Form
  {
    private readonly AddinManager _addinManager;

    private Panel[] _addinPanel;

    public PreferencesForm() : this(new ToolsManager())
    {
    }

    public PreferencesForm(ToolsManager toolsManager)
    {
      InitializeComponent();

      _addinManager = toolsManager.AddinManager;

      InitAddinManager();

      InitAddins();

      _addinManager.OnApplicationAddinListChanged += OnAppAddinListChanged;
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
      // loop through each add-in and save
      this.Close();
    }

    private void InitAddinManager()
    {
      Views.Preferences.AddinManagerCtrl ctrl = new Preferences.AddinManagerCtrl();
      ctrl.Dock = DockStyle.Fill;
      tabPage2.Controls.Add(ctrl);

      //Form1 myForm = new Form1();
      //myForm.TopLevel = false;
      //myForm.AutoScroll = true;
      //frmMain.Panel2.Controls.Add(myForm);
      //myForm.Show();
    }

    private void InitAddins()
    {
      foreach (PreferenceAddin addin in _addinManager.GetPreferenceAddins())
      {
        string name = addin.GetType().Name;
        Log.Debug($"Adding preference add-in: '{name}");
        try
        {
          string title = "";
          System.Windows.Forms.Panel panel;
          if (addin.GetPreferenceAddin(this, out title, out panel))
          {
            // insert into treeView
            // Add into _addinPanel
          }
        }
        catch (Exception ex)
        {
          Log.Warn($"There was an issue adding Preference panel from add-in {name}");
          Log.Error($"{ex.Message}:\n{ex.StackTrace}");
        }
      }
    }

    private void OnAppAddinListChanged(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }

    private void OptionsForm_Load(object sender, EventArgs e)
    {
    }
  }
}

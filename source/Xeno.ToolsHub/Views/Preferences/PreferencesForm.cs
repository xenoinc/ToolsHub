/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    OptionsForm.cs
 * Description:
 *
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Managers;

namespace Xeno.ToolsHub.Views
{
  public partial class PreferencesForm : Form
  {
    private readonly AddinsManager _addinManager;

    private Dictionary<string, PreferencePageExtension> _addinPanel;

    public PreferencesForm() : this(new AddinsManager())
    {
    }

    public PreferencesForm(AddinsManager addinsManager)
    {
      InitializeComponent();

      _addinManager = addinsManager;
      _addinPanel = new Dictionary<string, PreferencePageExtension>();

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
      Views.Preferences.AddinManagerCtrl ctrl = new Preferences.AddinManagerCtrl(_addinManager);
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
      foreach (PreferencePageExtension page in _addinManager.GetPreferenceAddins())
      {
        string name = string.Empty;
        try
        {
          name = page.GetType().Name;
          string title = page.Title;

          _addinPanel.Add(page.Title, page);

          TreeAddinList.Nodes.Add(title);

          Log.Debug($"Adding preference add-in: '{name}");

          //string title = "";
          //System.Windows.Forms.Panel panel;
          //if (addin.GetPreferenceAddin(this, out title, out panel))
          //{
          //  // insert into treeView
          //  // Add into _addinPanel
          //}
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

    private void PreferencesForm_Load(object sender, EventArgs e)
    {
    }
  }
}

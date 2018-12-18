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
using Xeno.ToolsHub.ExtensionModel.Preferences;
using Xeno.ToolsHub.Managers;

namespace Xeno.ToolsHub.Views
{
  public partial class PreferencesForm : Form
  {
    private readonly AddinsManager _addinManager;

    private Dictionary<string, PreferencePageExtension> _addinPages;

    public PreferencesForm() : this(new AddinsManager())
    {
    }

    public PreferencesForm(AddinsManager addinsManager)
    {
      InitializeComponent();

      AddinTree.Sort();

      _addinManager = addinsManager;
      _addinPages = new Dictionary<string, PreferencePageExtension>();

      InitAddinManager();

      RefreshTreeView();

      _addinManager.OnApplicationAddinListChanged += OnAppAddinListChanged;
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void BtnOk_Click(object sender, EventArgs e)
    {
      // loop through each add-in and save
      SavePreferences();
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

    private void SavePreferences()
    {
      foreach (var addinPage in _addinPages)
      {
        var page = addinPage.Value;
        if (page.IsModified)
        {
          page.OnSave();
        }
      }
    }

    private void RefreshTreeView()
    {
      foreach (PreferencePageExtension page in _addinManager.GetPreferenceAddins())
      {
        string id = string.Empty;
        string name = string.Empty;
        string title = string.Empty;
        try
        {
          id = page.Id;
          name = page.GetType().Name;
          title = page.Title;

          _addinPages.Add(page.Id, page);

          TreeNode node = new TreeNode(title)
          {
            Tag = id
          };

          AddinTree.Nodes.Add(node);

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

      // Auto-select first item
      if (AddinTree.Nodes.Count > 0)
      {
        var firstNode = AddinTree.Nodes[0];
        AddinTree.SelectedNode = firstNode;
      }
    }

    private void OnAppAddinListChanged(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }

    private void PreferencesForm_Load(object sender, EventArgs e)
    {
    }

    private void AddinTree_AfterSelect(object sender, TreeViewEventArgs e)
    {
      string id = e.Node.Tag.ToString();
      ShowPage(id);
    }

    private void ShowPage(string addinId)
    {
      foreach (var addin in _addinPages)
      {
        Log.Debug($"Parsing panel, {addin.Key}");

        if (addin.Value.Id == addinId)
        {
          var page = addin.Value.Page;
          if (page.GetType().BaseType == typeof(Form))
          {
            LblPageTitle.Text = page.Text;

            page.TopLevel = false;
            PanelAddinPrefsView.Controls.Clear();
            PanelAddinPrefsView.Controls.Add(page);
            page.FormBorderStyle = FormBorderStyle.None;
            page.Dock = DockStyle.Fill;
            page.Show();
          }
          else if (page.GetType().BaseType == typeof(UserControl))
          {
            LblPageTitle.Text = addin.Value.Title;

            page.Dock = DockStyle.Fill;
            // PanelAddinPrefsView.Controls.Cast<Control>().ForEach(i => i.Dispose());
            PanelAddinPrefsView.Controls.Clear();
            PanelAddinPrefsView.Controls.Add(page);
            PanelAddinPrefsView.Show();
          }
          else
          {
            Log.Error($"Preference page id:'{addinId}' is of an unsupported type.");
          }

          break;
        }
      }
    }
  }
}

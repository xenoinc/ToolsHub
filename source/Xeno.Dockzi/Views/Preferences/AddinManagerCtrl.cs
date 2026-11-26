/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-24
 * Author:  Damian Suess
 * File:    AddinManagerCtrl.cs
 * Description:
 *  Application Add-in Manager
 */

using System.Windows.Forms;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services.Logging;

namespace Xeno.Dockzi.Views.Preferences
{
  public partial class AddinManagerCtrl : UserControl
  {
    private readonly AddinsManager _addinManager;

    public AddinManagerCtrl(AddinsManager manager)
    {
      InitializeComponent();

      _addinManager = manager;

      ListAddins.View = View.Details;
      ListAddins.HideSelection = false;
      groupBox1.Visible = true;
      groupBox1.Height = 120;
      groupBox1.Top = Height - groupBox1.Height - 6;
      ListAddins.Height = groupBox1.Top - ListAddins.Top - 6;

      foreach (var label in new[] { label2, label3, label4, label5 })
      {
        label.Visible = true;
        label.UseMnemonic = false;
      }

      label4.AutoSize = false;
      label4.Size = new System.Drawing.Size(groupBox1.ClientSize.Width - 12, 47);
      label4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

      ListAddins.SelectedIndexChanged += (_, _) => ShowSelectedAddin();

      GetAddins();
    }

    private void AddinManagerCtrl_Load(object sender, System.EventArgs e)
    {
    }

    private void GetAddins()
    {
      ListAddins.Items.Clear();

      foreach (Mono.Addins.Addin addin in _addinManager.GetAllAddins())
      {
        //// Mono.Addins.Setup.SetupService.GetAddinHeader(addin)
        Log.Debug($"Add-in Discovered: name={addin.Name}; id={addin.Id}; localId={addin.LocalId};");

        ListViewItem item;
        item = new ListViewItem(new[] { addin.Id, addin.Name, addin.Enabled.ToString(), addin.IsUserAddin.ToString() }) { Tag = addin };
        ListAddins.Items.Add(item);
      }

      ShowSelectedAddin();
    }

    private void ShowSelectedAddin()
    {
      var addin = ListAddins.SelectedItems.Count == 1
        ? ListAddins.SelectedItems[0].Tag as Mono.Addins.Addin
        : null;

      label2.Text = "Title: " + (addin?.Name ?? string.Empty);
      label3.Text = "Version: " + (addin?.Version ?? string.Empty);
      label5.Text = "Author(s): " + (addin?.Description.Author ?? string.Empty);
      label4.Text = "Description: " + (addin?.Description.Description ?? string.Empty);
    }

    private void BtnDisable_Click(object sender, System.EventArgs e)
    {
      AddinEnabled(false);
      GetAddins();
    }

    private void BtnEnable_Click(object sender, System.EventArgs e)
    {
      AddinEnabled(true);
      GetAddins();
    }

    private void AddinEnabled(bool enable)
    {
      var id = GetSelectedAddinId();
      if (id == string.Empty)
      {
        Log.Debug("No add-in selected");
        return;
      }

      // AddinManager.Registry.DisableAddin("SimpleApp.HelloWorldExtension,0.1.0");
      if ((enable && Mono.Addins.AddinManager.Registry.IsAddinEnabled(id)) ||
        (!enable && !Mono.Addins.AddinManager.Registry.IsAddinEnabled(id)))
      {
        return;
      }

      if (enable)
        Mono.Addins.AddinManager.Registry.EnableAddin(id);
      else
        Mono.Addins.AddinManager.Registry.DisableAddin(id);

      Log.Debug($"Add-in '{id}' Enabled={enable}");
    }

    private string GetSelectedAddinId()
    {
      string id = string.Empty;

      var item = ListAddins.SelectedItems;
      if (item.Count > 0)
      {
        id = item[0].SubItems[0].Text;
      }

      return id;
    }
  }
}

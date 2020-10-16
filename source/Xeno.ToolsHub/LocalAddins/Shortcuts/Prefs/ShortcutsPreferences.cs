/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.cs
 * Description:
 *  Shortcuts preference window
 */

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  using System;
  using System.Windows.Forms;

  //// using Newtonsoft.Json.Linq;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  public partial class ShortcutsPreferences : Form, IPreferencePageActions
  {
    private bool _isModified = false;

    private ShortcutsManager _shortcuts = new ShortcutsManager();

    public ShortcutsPreferences()
    {
      InitializeComponent();

      LvShortcuts.MultiSelect = false;
    }

    public bool IsModified
    {
      get { return _isModified; }

      set
      {
        if (_isModified != value)
        {
          _isModified = value;
          LblModified.Visible = value;
        }
      }
    }

    public bool OnSave()
    {
      string errMsg = string.Empty;
      string json = TxtRawFile.Text;

      IsModified = false;

      if (_shortcuts.ValidateJson(json, out errMsg))
      {
        _shortcuts.ShortcutItems = Newtonsoft.Json.JsonConvert.DeserializeObject<ShortcutItems>(json);
        _shortcuts.SaveShortcuts();
        _shortcuts.Refresh();

        return true;
      }
      else
      {
        Log.Error("Your JSON formatting is bad, and you should feel bad too!" + Environment.NewLine + errMsg);
        return false;
      }
    }

    private void LoadShortcutsFile()
    {
      TxtRawFile.Text = _shortcuts.ToString();

      ToggleProperties(false);

      RefreshListView();
    }

    private void ShortcutsPreferences_Load(object sender, EventArgs e)
    {
      LoadShortcutsFile();
      IsModified = false;
    }

    private void TxtRawFile_TextChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void RefreshListView()
    {
      string json = _shortcuts.ToString();
      int ndx = 0;

      foreach (var item in _shortcuts.ShortcutItems)
      {
        var title = item.Title;
        var target = item.Target;
        LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), title, target }));

        ++ndx;
      }
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
      ListAdd();
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem item in LvShortcuts.SelectedItems)
        LvShortcuts.Items.Remove(item);
    }

    private void LvShortcuts_SelectedIndexChanged(object sender, EventArgs e)
    {

      foreach (ListViewItem item in LvShortcuts.SelectedItems)
      {
        TxtTitle.Text = item.SubItems[0].Text;
        TxtPath.Text = item.SubItems[1].Text;
        TxtPathArgs.Text = item.SubItems[2].Text;
      }
    }

    private void BtnHideShowProperties_Click(object sender, EventArgs e)
    {
      ToggleProperties(!GroupProperties.Visible);
    }

    private void ListAdd()
    {
      string title = TxtTitle.Text;
      string target = TxtPath.Text;
      int ndx = LvShortcuts.Items.Count;

      LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), title, target }));
    }

    /// <summary>Visibility of the Shortcut Properties group box.</summary>
    /// <param name="showProperties">Specify whether or not to display the Properties box.</param>
    private void ToggleProperties(bool showProperties)
    {
      // Hide/show Properties
      GroupProperties.Visible = showProperties;

      if (showProperties)
      {
        var padding = 6;
        BtnHideShowProperties.Text = "Hide Properties";
        GroupShortcuts.Height = GroupProperties.Top - GroupShortcuts.Location.Y - padding;
      }
      else
      {
        BtnHideShowProperties.Text = "Show Properties";
        GroupShortcuts.Height = GroupProperties.Location.Y + GroupProperties.Height;
      }
    }
  }
}

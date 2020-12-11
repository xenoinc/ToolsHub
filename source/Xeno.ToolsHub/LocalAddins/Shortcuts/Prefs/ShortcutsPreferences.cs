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

  /// <summary>Shortcuts Add-in Preference Page.</summary>
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
      IsModified = false;

      if (SaveShortcuts())
      {
        return true;
      }
      else
      {
        Log.Error("Unable to save shortcuts, errors were found in titles or paths.");
        return false;
      }

      ////string errMsg = string.Empty;
      ////string json = TxtRawFile.Text;
      ////
      ////IsModified = false;
      ////
      ////if (_shortcuts.ValidateJson(json, out errMsg))
      ////{
      ////  _shortcuts.ShortcutItems = Newtonsoft.Json.JsonConvert.DeserializeObject<ShortcutItems>(json);
      ////  _shortcuts.SaveShortcuts();
      ////  _shortcuts.Refresh();
      ////
      ////  return true;
      ////}
      ////else
      ////{
      ////  Log.Error("Your JSON formatting is bad, and you should feel bad too!" + Environment.NewLine + errMsg);
      ////  return false;
      ////}
    }

    /// <summary>Validate ListView for invalid user inputs.</summary>
    /// <returns>True if passing.</returns>
    private bool ValidateShortcuts(out ShortcutItems shortcuts)
    {
      bool passed = true;

      ShortcutItems items = new ShortcutItems();

      foreach (ListViewItem item in LvShortcuts.Items)
      {
        string title = item.SubItems[1].Text;
        string path = item.SubItems[2].Text;

        if (string.IsNullOrEmpty(title.Trim()) ||
            string.IsNullOrEmpty(path.Trim()))
        {
          Log.Error($"Shortcut Id '{item.SubItems[0]}' - Missing or invalid Title/Path");

          item.BackColor = System.Drawing.Color.OrangeRed;
          passed = false;
        }
        else
        {
          // Reset if it's okay
          item.BackColor = System.Drawing.Color.Empty;
        }
      }

      shortcuts = items;

      return passed;
    }

    private bool SaveShortcuts()
    {
      ShortcutItems shortcuts = new ShortcutItems();

      if (!ValidateShortcuts(out shortcuts))
        return false;

      _shortcuts.ShortcutItems.Clear();
      _shortcuts.ShortcutItems = shortcuts;

      ////_shortcuts.ShortcutItems = Newtonsoft.Json.JsonConvert.DeserializeObject<ShortcutItems>(json);
      _shortcuts.SaveShortcuts();
      _shortcuts.Refresh();

      return true;
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

    private void BtnAdd_Click(object sender, EventArgs e)
    {
      ItemAdd(TxtTitle.Text, TxtPath.Text, TxtPathArgs.Text);
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
        TxtTitle.Text = item.SubItems[1].Text;
        TxtPath.Text = item.SubItems[2].Text;
        TxtPathArgs.Text = item.SubItems[3].Text;
      }
    }

    private void BtnHideShowProperties_Click(object sender, EventArgs e)
    {
      ToggleProperties(!GroupProperties.Visible);
    }

    private void ItemAdd(string title, string target, string args)
    {
      int ndx = LvShortcuts.Items.Count;
      LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), title, target, args }));
    }

    private void RefreshListView()
    {
      LvShortcuts.Items.Clear();

      foreach (var item in _shortcuts.ShortcutItems)
      {
        var title = item.Title;
        var target = item.Target;
        var args = item.Params;

        ItemAdd(title, target, args);
      }
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

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
  using static System.Windows.Forms.ListView;

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

    /// <summary>
    ///   Gets or sets a value indicating whether or not the parent window
    ///   needs to invoke the OnSave method.
    /// </summary>
    public bool IsModified
    {
      get => _isModified;
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

      ShortcutItems shortcuts = new ShortcutItems();

      if (!ValidateListViewShortcuts(out shortcuts))
      {
        Log.Error("Unable to save shortcuts, errors were found in titles or paths.");
        return false;
      }

      _shortcuts.ShortcutItems.Clear();
      _shortcuts.ShortcutItems = shortcuts;

      _shortcuts.SaveShortcuts();
      _shortcuts.Refresh();

      return true;

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

    private void BtnAdd_Click(object sender, EventArgs e)
    {
      // https://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=6646
      LvShortcuts.SelectedItems.


      // Create new ShortcutItem for editing
      TxtPath.Text = string.Empty;
      TxtTitle.Text = string.Empty;
      TxtPathArgs.Text = string.Empty;

      int ndx = LvShortcuts.Items.Count;
      LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), string.Empty, string.Empty, string.Empty }));
      
      // Select it so we can edit it
      LvShortcuts.Items[LvShortcuts.Items.Count - 1].Selected = true;
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem item in LvShortcuts.SelectedItems)
        LvShortcuts.Items.Remove(item);
    }

    private void TxtTitle_TextChanged(object sender, EventArgs e)
    {
      RefreshListViewFromProperties();
    }

    private void TxtPath_TextChanged(object sender, EventArgs e)
    {
      RefreshListViewFromProperties();
    }

    private void TxtPathArgs_TextChanged(object sender, EventArgs e)
    {
      RefreshListViewFromProperties();
    }

    private void TxtRawFile_TextChanged(object sender, EventArgs e)
    {
      IsModified = true;
    }

    private void LoadShortcutsFile()
    {
      TxtRawFile.Text = _shortcuts.ToString();

      RefreshListViewFromCache();
    }

    private void ShortcutsPreferences_Load(object sender, EventArgs e)
    {
      LoadShortcutsFile();
      IsModified = false;
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

    /// <summary>Add new ShortcutItem into ListView.</summary>
    /// <param name="addToCache">Add new item to Cache. Use 'false' when loading from cache.</param>
    /// <param name="title">Title of shortcut.</param>
    /// <param name="target">Execution target of shortcut.</param>
    /// <param name="args">Optional execution arguments.</param>
    private void LvShortcutsAddItem(bool addToCache, string title, string target, string args)
    {
      int ndx = LvShortcuts.Items.Count;
      LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), title, target, args }));

      if (addToCache)
      {
        _shortcuts.ShortcutItems.Add(new ShortcutItem(title, target, args));
      }

      //// RefreshRawJsonFromCache();
      RefreshRawJsonFromListView();

      IsModified = true;
    }

    /// <summary>Refresh RawJson tab from ListView</summary>
    private void RefreshRawJsonFromCache()
    {
      var json = Xeno.ToolsHub.Config.Helpers.ToJson(_shortcuts.ShortcutItems, true);
      TxtRawFile.Text = json;
    }

    /// <summary>Refresh RawJson and Cache tab from ListView.</summary>
    private void RefreshRawJsonFromListView()
    {
      // 1. Validate ListView items for errors
      ;

      // 2. Convert to ShortcutItems object & Serialize
      var shortcutItems = ToShortcutItems(LvShortcuts.Items);
      var json = Xeno.ToolsHub.Config.Helpers.ToJson(shortcutItems);

      // 3. Refresh Cache & display Raw input
      _shortcuts.ShortcutItems.Clear();
      _shortcuts.ShortcutItems = shortcutItems;

      TxtRawFile.Text = json;
    }

    /// <summary>Refresh ListView from cached ShortcutItems.</summary>
    private void RefreshListViewFromCache()
    {
      LvShortcuts.Items.Clear();

      foreach (var item in _shortcuts.ShortcutItems)
      {
        var title = item.Title;
        var target = item.Target;
        var args = item.Params;

        LvShortcutsAddItem(false, title, target, args);
      }
    }

    private void RefreshListViewFromProperties()
    {
      if (LvShortcuts.SelectedItems.Count == 1)
      {
        var item = LvShortcuts.SelectedItems[0];
        item.SubItems[1].Text = TxtTitle.Text;
        item.SubItems[2].Text = TxtPath.Text;
        item.SubItems[3].Text = TxtPathArgs.Text;

        RefreshRawJsonFromListView();
      }
    }

    /// <summary>Convert ListView to ShortcutItems object.</summary>
    /// <param name="items">Collection of ListView items.</param>
    /// <returns>ShortcutItems object.</returns>
    private ShortcutItems ToShortcutItems(ListViewItemCollection items)
    {
      //// bool passed = true;
      var shortcuts = new ShortcutItems();

      foreach (ListViewItem item in items)
      {
        string title = item.SubItems[1].Text;
        string path = item.SubItems[2].Text;
        string args = string.Empty;

        if (!string.IsNullOrEmpty(title.Trim()) || !string.IsNullOrEmpty(path.Trim()))
        {
          //// passed = false;
          Log.Warn($"Shortcut Id '{item.SubItems[0]}' - Missing or invalid Title/Path");
        }
        else
        {
          shortcuts.Add(new ShortcutItem(title, path, args));
        }
      }

      return shortcuts;
    }

    /// <summary>Create ListView row from ShortcutItems object</summary>
    /// <param name="shortcutItems">ShortcutItems object.</param>
    /// <returns>Collection of ListView data.</returns>
    private ListViewItemCollection ToListViewItemArray(ShortcutItems shortcutItems)
    {
      throw new NotImplementedException();

      ListViewItemCollection items = new ListViewItemCollection(LvShortcuts);
      //// ListViewItem[] items = new ListViewItem[shortcutItems.Count];
      return items;
    }

    /// <summary>Validate ListView for invalid user inputs.</summary>
    /// <returns>True if passing.</returns>
    private bool ValidateListViewShortcuts(out ShortcutItems shortcuts)
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
          item.BackColor = System.Drawing.Color.Empty;  // Reset if it's okay
        }
      }

      shortcuts = items;

      return passed;
    }
  }
}

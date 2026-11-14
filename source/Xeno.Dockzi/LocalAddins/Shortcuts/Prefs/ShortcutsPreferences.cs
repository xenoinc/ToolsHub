/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    ShortcutsPreferences.cs
 * Description:
 *  Shortcuts preference window
 */

namespace Xeno.Dockzi.LocalAddins.Shortcuts.Prefs;

using System;
using System.ComponentModel;
using System.Windows.Forms;

using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.Services.Logging;
using static System.Windows.Forms.ListView;

/// <summary>Shortcuts Add-in Preference Page.</summary>
public partial class ShortcutsPreferences : Form, IPreferencePageActions
{
  private readonly ShortcutsManager _shortcuts;

  private bool _isModified = false;
  private bool _updatingControls;

  public ShortcutsPreferences()
    : this(new ShortcutsManager())
  {
  }

  public ShortcutsPreferences(ShortcutsManager shortcuts)
  {
    _shortcuts = shortcuts ?? throw new ArgumentNullException(nameof(shortcuts));
    InitializeComponent();

    LvShortcuts.MultiSelect = false;
  }

  /// <summary>
  ///   Gets or sets a value indicating whether or not the parent window
  ///   needs to invoke the OnSave method.
  /// </summary>
  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    ShortcutItems shortcuts = new ShortcutItems();

    if (!ValidateListViewShortcuts(out shortcuts))
    {
      Log.Error("Unable to save shortcuts, errors were found in titles or paths.");
      return false;
    }

    _shortcuts.ShortcutItems = shortcuts;
    _shortcuts.SaveShortcuts();
    IsModified = false;

    return true;

    ////string errMsg = string.Empty;
    ////string json = TxtRawFile.Text;
    ////
    ////IsModified = false;
    ////
    ////if (_shortcuts.ValidateJson(json, out errMsg))
    ////{
    ////  _shortcuts.ShortcutItems = JsonSerializer.Deserialize<ShortcutItems>(json);
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
    LvShortcuts.SelectedItems.Clear();

    // Create new ShortcutItem for editing
    TxtPath.Text = string.Empty;
    TxtTitle.Text = string.Empty;
    TxtPathArgs.Text = string.Empty;

    int ndx = LvShortcuts.Items.Count;
    LvShortcuts.Items.Add(new ListViewItem(new string[] { ndx.ToString(), string.Empty, string.Empty, string.Empty }));

    // Select it so we can edit it
    LvShortcuts.Items[LvShortcuts.Items.Count - 1].Selected = true;
    RefreshRawJsonFromListView();
  }

  private void BtnRemove_Click(object sender, EventArgs e)
  {
    while (LvShortcuts.SelectedItems.Count > 0)
      LvShortcuts.Items.Remove(LvShortcuts.SelectedItems[0]);

    RefreshRawJsonFromListView();
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
    if (_updatingControls)
      return;

    _updatingControls = true;
    try
    {
      if (LvShortcuts.SelectedItems.Count == 1)
      {
        var item = LvShortcuts.SelectedItems[0];
        TxtTitle.Text = item.SubItems[1].Text;
        TxtPath.Text = item.SubItems[2].Text;
        TxtPathArgs.Text = item.SubItems[3].Text;
      }
    }
    finally
    {
      _updatingControls = false;
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

    if (addToCache)
      RefreshRawJsonFromListView();
  }

  /// <summary>Refresh RawJson tab from ListView</summary>
  private void RefreshRawJsonFromCache()
  {
    var json = Xeno.Dockzi.Config.Helpers.ToJson(_shortcuts.ShortcutItems, true);
    TxtRawFile.Text = json;
  }

  /// <summary>Refresh RawJson and Cache tab from ListView.</summary>
  private void RefreshRawJsonFromListView()
  {
    if (_updatingControls)
      return;

    var shortcutItems = ToShortcutItems(LvShortcuts.Items);
    _shortcuts.ShortcutItems = shortcutItems;
    TxtRawFile.Text = Xeno.Dockzi.Config.Helpers.ToJson(shortcutItems);
    IsModified = true;
  }

  /// <summary>Refresh ListView from cached ShortcutItems without modifying the cache.</summary>
  private void RefreshListViewFromCache()
  {
    _updatingControls = true;
    LvShortcuts.BeginUpdate();
    try
    {
      LvShortcuts.Items.Clear();
      foreach (var item in _shortcuts.ShortcutItems)
      {
        LvShortcutsAddItem(false, item.Title, item.Target, item.Params);
        LvShortcuts.Items[LvShortcuts.Items.Count - 1].Tag = item;
      }

      RefreshRawJsonFromCache();
    }
    finally
    {
      LvShortcuts.EndUpdate();
      _updatingControls = false;
    }
  }

  private void RefreshListViewFromProperties()
  {
    if (!_updatingControls && LvShortcuts.SelectedItems.Count == 1)
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
    var shortcuts = new ShortcutItems();
    foreach (ListViewItem item in items)
    {
      shortcuts.Add(new ShortcutItem(item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text)
      {
        RunAsAdmin = (item.Tag as ShortcutItem)?.RunAsAdmin ?? false,
      });
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

    foreach (ListViewItem item in LvShortcuts.Items)
    {
      string title = item.SubItems[1].Text;
      string path = item.SubItems[2].Text;

      if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(path))
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

    shortcuts = ToShortcutItems(LvShortcuts.Items);

    return passed;
  }
}

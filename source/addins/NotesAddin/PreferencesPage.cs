namespace Xeno.Dockzi.NotesAddin;

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public sealed class PreferencesPage : Form
{
  private readonly NotesManager _manager;
  private readonly CheckBox _enabled = new() { Text = "Enable global shortcut for a new note", AutoSize = true };
  private readonly CheckBox _windows = new() { Text = "Include Windows key", AutoSize = true };
  private readonly TextBox _keys = new() { ReadOnly = true, Width = 180, AccessibleName = "New note shortcut" };
  private readonly Label _error = new() { AutoSize = true, ForeColor = Color.Firebrick, MaximumSize = new Size(420, 0), Visible = false };
  private Keys _selected;
  private readonly ListView _notes = new()
  {
    Name = "SavedNotes", View = View.Details, FullRowSelect = true, HideSelection = false,
    MultiSelect = false, Dock = DockStyle.Fill, AccessibleName = "Saved notes",
  };
  private readonly Font _trashFont = new("Segoe MDL2 Assets", 11);

  public PreferencesPage(NotesManager manager)
  {
    _manager = manager;
    Text = "Notes";
    _selected = manager.ShortcutKeys;
    _enabled.Checked = manager.ShortcutEnabled;
    _windows.Checked = manager.ShortcutWindows;

    _keys.Text = new KeysConverter().ConvertToString(_selected);

    var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true };
    layout.Controls.Add(_enabled);
    var shortcutRow = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0) };
    shortcutRow.Controls.Add(_keys);
    shortcutRow.Controls.Add(_windows);
    layout.Controls.Add(shortcutRow);
    layout.Controls.Add(new Label { Text = "Click the shortcut box and press a key combination (default: Ctrl+Alt+N).\nCtrl+S saves; Esc discards edits. Use # Title for the note title.", AutoSize = true, MaximumSize = new Size(420, 0) });

    var container = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1, RowCount = 4 };
    container.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
    container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    container.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
    container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    container.Controls.Add(layout, 0, 0);
    container.Controls.Add(new Label { Text = "Saved notes — double-click to open; click the trash icon to delete.", AutoSize = true, Margin = new Padding(3, 12, 3, 8) }, 0, 1);
    _notes.Columns.Add("Title", 210);
    _notes.Columns.Add("Saved Date", 145);
    _notes.Columns.Add("Delete", 52);
    container.Controls.Add(_notes, 0, 2);
    var footer = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
    footer.Controls.Add(new Label { Text = "Notes folder: " + manager.StorageDirectory, AutoSize = true, MaximumSize = new Size(420, 0), Margin = new Padding(3, 8, 3, 3) });
    footer.Controls.Add(_error);
    container.Controls.Add(footer, 0, 3);
    Controls.Add(container);
    _notes.Resize += (_, _) => _notes.Columns[0].Width = Math.Max(100, _notes.ClientSize.Width - 145 - 52 - SystemInformation.VerticalScrollBarWidth);
    _notes.MouseDoubleClick += (_, e) =>
    {
      var hit = _notes.HitTest(e.Location);
      if (hit.Item?.Tag is Note note && hit.Item.SubItems.IndexOf(hit.SubItem) != 2)
        _manager.Open(note);
    };
    _notes.MouseClick += (_, e) =>
    {
      if (e.Button != MouseButtons.Left) return;
      var hit = _notes.HitTest(e.Location);
      if (hit.Item?.Tag is Note note && hit.Item.SubItems.IndexOf(hit.SubItem) == 2)
        DeleteNote(note.Id);
    };
    _notes.KeyDown += (_, e) =>
    {
      if (e.KeyCode == Keys.Enter && _notes.SelectedItems.Count == 1 && _notes.SelectedItems[0].Tag is Note note)
      {
        _manager.Open(note);
        e.SuppressKeyPress = true;
      }
    };
    _manager.NotesChanged += OnNotesChanged;
    VisibleChanged += (_, _) => { if (Visible) RefreshNotes(); };
    RefreshNotes();
    _enabled.CheckedChanged += (_, _) => IsModified = true;
    _windows.CheckedChanged += (_, _) => IsModified = true;

    _keys.KeyDown += (_, e) =>
    {
      e.SuppressKeyPress = true;
      if (e.KeyCode is Keys.ControlKey or Keys.ShiftKey or Keys.Menu or Keys.LWin or Keys.RWin)
        return;

      _selected = e.KeyData;
      _keys.Text = new KeysConverter().ConvertToString(_selected);
      IsModified = true;
    };
  }

  public bool IsModified { get; private set; }

  public void RefreshNotes()
  {
    if (IsDisposed) return;
    if (InvokeRequired)
    {
      BeginInvoke(new Action(RefreshNotes));
      return;
    }

    _notes.BeginUpdate();
    try
    {
      _notes.Items.Clear();
      foreach (var note in _manager.All())
      {
        var item = new ListViewItem(note.Title) { Tag = note, UseItemStyleForSubItems = false };
        item.SubItems.Add(note.SavedUtc.ToLocalTime().ToString("g"));
        item.SubItems.Add(new ListViewItem.ListViewSubItem(item, "\uE74D") { Font = _trashFont });
        _notes.Items.Add(item);
      }
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      _error.Text = "Unable to list notes: " + ex.Message;
      _error.Visible = true;
    }
    finally
    {
      _notes.EndUpdate();
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      _manager.NotesChanged -= OnNotesChanged;
      _trashFont.Dispose();
    }

    base.Dispose(disposing);
  }

  private void OnNotesChanged(object sender, EventArgs e) => RefreshNotes();

  private void DeleteNote(Guid id)
  {
    try
    {
      _manager.Delete(id);
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      MessageBox.Show(this, ex.Message, "Unable to delete note", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  public void Save()
  {
    try
    {
      _manager.ConfigureShortcut(_enabled.Checked, _selected, _windows.Checked);
      IsModified = false;
      _error.Text = string.Empty;
      _error.Visible = false;
    }
    catch (Exception ex) when (ex is ArgumentException || ex is Win32Exception)
    {
      _error.Text = ex.Message;
      _error.Visible = true;
      // Keep the host dialog open when a shortcut cannot be applied.
      throw new InvalidOperationException(ex.Message, ex);
    }
  }
}

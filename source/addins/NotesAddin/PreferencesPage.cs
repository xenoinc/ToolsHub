namespace Xeno.Dockzi.NotesAddin;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public sealed class PreferencesPage : Form
{
  private readonly NotesManager _manager;
  private readonly CheckBox _enabled = new() { Text = "Enable global shortcut for a new note", AutoSize = true };
  private readonly CheckBox _windows = new() { Text = "Include Windows key", AutoSize = true };
  private readonly TextBox _keys = new() { ReadOnly = true, Width = 260, AccessibleName = "New note shortcut" };
  private readonly Label _error = new() { AutoSize = true, ForeColor = Color.Firebrick, MaximumSize = new Size(420, 0) };
  private Keys _selected;

  public PreferencesPage(NotesManager manager)
  {
    _manager = manager;
    Text = "Notes";
    _selected = manager.ShortcutKeys;
    _enabled.Checked = manager.ShortcutEnabled;
    _windows.Checked = manager.ShortcutWindows;

    _keys.Text = new KeysConverter().ConvertToString(_selected);

    var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(16), AutoScroll = true };
    layout.Controls.Add(new Label { Text = "New note keyboard shortcut", AutoSize = true, Font = new Font(Font, FontStyle.Bold) });
    layout.Controls.Add(_enabled);
    layout.Controls.Add(_keys);
    layout.Controls.Add(_windows);
    layout.Controls.Add(new Label { Text = "Click the shortcut box and press a key combination.\nDefault: Ctrl+Alt+N. Ctrl+S saves a note and shows its preview.\nUse # Title in your note to set its window title.", AutoSize = true });
    layout.Controls.Add(new Label { Text = "Notes folder: " + manager.StorageDirectory, AutoSize = true, MaximumSize = new Size(420, 0), Margin = new Padding(3, 20, 3, 3) });
    layout.Controls.Add(_error);

    Controls.Add(layout);
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

  public void Save()
  {
    try
    {
      _manager.ConfigureShortcut(_enabled.Checked, _selected, _windows.Checked);
      IsModified = false;
      _error.Text = string.Empty;
    }
    catch (Exception ex) when (ex is ArgumentException || ex is Win32Exception)
    {
      _error.Text = ex.Message;
      // Keep the host dialog open when a shortcut cannot be applied.
      throw new InvalidOperationException(ex.Message, ex);
    }
  }
}

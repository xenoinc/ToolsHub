namespace Xeno.Dockzi.NotesAddin;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Lite.EventIpc;
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.Services;

[SupportedOSPlatform("windows")]
public sealed partial class NotesManager : IDisposable
{
  public const string SettingsId = "NotesAddin";
  private readonly IEventAggregator _events;
  private readonly ISettingsService _settings;
  private readonly NoteStore _store;
  private readonly Dictionary<Guid, NoteWindow> _windows = new();
  private bool _running;
  private GlobalShortcut _shortcut;

  public NotesManager(NoteStore store, ISettingsService settings, IEventAggregator events)
  {
    _store = store;
    _settings = settings;
    _events = events;
  }

  public event EventHandler NotesChanged;

  public bool ShortcutEnabled => _settings.GetBool(SettingsId, "ShortcutEnabled", true);

  public Keys ShortcutKeys => (Keys)_settings.GetInt(SettingsId, "ShortcutKeys", (int)(Keys.Control | Keys.Alt | Keys.N));

  public bool ShortcutWindows => _settings.GetBool(SettingsId, "ShortcutWindows");

  public string StorageDirectory => _store.DirectoryPath;

  public IReadOnlyList<Note> All() => _store.All();

  public void ConfigureShortcut(bool enabled, Keys keys, bool windows)
  {
    if (enabled &&
      ((keys & Keys.KeyCode) == Keys.None || ((keys & Keys.Modifiers) == Keys.None && !windows)))
      throw new ArgumentException("Choose a key together with Ctrl, Alt, Shift, or Windows.");

    if (_running && (enabled != ShortcutEnabled || keys != ShortcutKeys || windows != ShortcutWindows || (enabled && _shortcut == null)))
    {
      _shortcut?.Dispose();
      _shortcut = null;

      try
      {
        RegisterShortcut(enabled, keys, windows);
      }
      catch
      {
        RegisterShortcut(ShortcutEnabled, ShortcutKeys, ShortcutWindows);
        throw;
      }
    }

    _settings.SetValue(SettingsId, "ShortcutEnabled", enabled);
    _settings.SetValue(SettingsId, "ShortcutKeys", (int)keys);
    _settings.SetValue(SettingsId, "ShortcutWindows", windows);
  }

  public void Delete(Guid id)
  {
    _store.Delete(id);
    if (_windows.TryGetValue(id, out var window))
      window.CloseWithoutSaving();

    NotifyChanged();
  }

  public void Dispose() => Stop();

  public void NewNote()
  {
    try
    {
      // Include unsaved windows and their saved titles, since Escape can restore those titles.
      var titles = _store
        .All()
        .Select(note => note.Title)
        .Concat(_windows.Values.SelectMany(window => new[] { window.Title, window.SavedTitle }));

      BigInteger highest = BigInteger.Zero;

      foreach (string title in titles)
      {
        var match = NoteNumberingRegex().Match(title);

        if (match.Success
          && BigInteger.TryParse(
            match.Groups[1].Value,
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out var number)
          && number > highest)
        {
          highest = number;
        }
      }

      string nextTitle = "Note " + (highest + BigInteger.One).ToString(CultureInfo.InvariantCulture);
      Show(new Note(Guid.NewGuid(), "# " + nextTitle + "\n\n", default), true);
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      MessageBox.Show(ex.Message, "Unable to create note", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  public void Open(Note note)
  {
    try
    {
      Show(_store.Load(note.Id), false);
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      MessageBox.Show(ex.Message, "Unable to open note", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  public IReadOnlyList<Note> Recent() => _store.Recent();

  public void Start()
  {
    if (_running) return;
    _running = true;
    try
    {
      RegisterShortcut(ShortcutEnabled, ShortcutKeys, ShortcutWindows);
    }
    catch (Win32Exception ex)
    {
      _events.Publish(new SystemTrayNotificationRequest(ex.Message + " Change it in Preferences > Notes."));
    }
  }

  public void Stop()
  {
    _shortcut?.Dispose();
    _shortcut = null;
    _running = false;

    // Preserve drafts when the add-in is disabled or the host shuts down.
    foreach (var window in _windows.Values.ToArray())
      window.Close();
  }

  [GeneratedRegex(@"^Note ([0-9]+)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
  private static partial Regex NoteNumberingRegex();

  private void NotifyChanged()
  {
    _events.Publish(new SystemTrayRefreshRequest());
    NotesChanged?.Invoke(this, EventArgs.Empty);
  }

  private void RegisterShortcut(bool enabled, Keys keys, bool windows)
  {
    if (enabled)
      _shortcut = new GlobalShortcut(keys, windows, NewNote);
  }

  private void Show(Note note, bool edit)
  {
    if (_windows.TryGetValue(note.Id, out var existing))
    {
      existing.Activate();
      return;
    }

    var window = new NoteWindow(note, text =>
    {
      var saved = _store.Save(note.Id, text);
      NotifyChanged();
      return saved;
    }, edit, () => Delete(note.Id));

    _windows.Add(note.Id, window);

    window.Closed += (_, _) => _windows.Remove(note.Id);

    System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(window);
    window.Show();
    window.Activate();
  }
}

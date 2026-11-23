namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.NotesAddin;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Services.Addins;

[TestClass]
[DoNotParallelize]
[OSCondition(OperatingSystems.Windows)]
public class NotesAddinTests
{
  [TestMethod]
  public void BundledDiscoveryRegistersNotesWithoutCreatingWindowsOrHotkeys()
  {
    var services = new ServiceCollection().AddDockzi();
    services.AddBundledAddinServices(Path.GetDirectoryName(typeof(NotesManager).Assembly.Location));

    using var provider = services.BuildServiceProvider(new ServiceProviderOptions
    {
      ValidateOnBuild = true,
      ValidateScopes = true,
    });

    Assert.IsNotNull(provider.GetRequiredService<NotesManager>());
  }

  [TestMethod]
  public void DeleteButtonRemovesDirtyNoteWithoutSavingItAgain()
  {
    RunSta(() => WithStore(store =>
    {
      var note = store.Save(Guid.NewGuid(), "# Delete me");
      int saves = 0;

      var window = new NoteWindow(
        note,
        saveText =>
        {
          saves++;
          return store.Save(note.Id, saveText);
        },
        edit: true,
        delete: () => store.Delete(note.Id));

      window.Show();

      try
      {
        window.NoteText = "# Unsaved changes";
        var header = (System.Windows.Controls.Grid)((System.Windows.Controls.Grid)window.Content).Children[0];

        var button = header.Children.OfType<System.Windows.Controls.Button>().Single(button =>
          System.Windows.Automation.AutomationProperties.GetName(button) == "Delete note");

        button.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

        Assert.IsFalse(window.IsVisible);
        Assert.AreEqual(0, saves);
        Assert.IsEmpty(store.All());
      }
      finally
      {
        window.CloseWithoutSaving();
      }
    }));
  }

  [TestMethod]
  public void EscapeDiscardsEditsRestoresLatestSavedPreviewAndThenCloses()
  {
    RunSta(() => WithStore(store =>
    {
      var id = Guid.NewGuid();
      int saves = 0;
      var window = new NoteWindow(store.Save(id, "# Original\n\nOriginal text"), text =>
      {
        saves++;
        return store.Save(id, text);
      });

      window.Show();

      try
      {
        window.BeginEdit();
        window.NoteText = "# Saved title\n\n**Saved text**";
        Assert.IsTrue(window.SaveNote());

        window.BeginEdit();
        window.NoteText = "# Discarded title\n\nDiscarded edits";

        PressEscape(window);

        Assert.IsTrue(window.IsVisible);
        Assert.IsFalse(window.IsEditing);
        Assert.AreEqual("Saved title", window.Title);
        Assert.AreEqual("# Saved title\n\n**Saved text**", window.NoteText);

        var layout = (System.Windows.Controls.Grid)window.Content;
        var preview = layout.Children.OfType<System.Windows.Controls.FlowDocumentScrollViewer>().Single();

        Assert.Contains("Saved text", new TextRange(preview.Document.ContentStart, preview.Document.ContentEnd).Text);
        Assert.AreEqual(1, saves);

        PressEscape(window);

        Assert.IsFalse(window.IsVisible);
        Assert.AreEqual(1, saves);
        Assert.AreEqual("# Saved title\n\n**Saved text**", store.Load(id).Text);
      }
      finally
      {
        window.Close();
      }
    }));
  }

  [TestMethod]
  public void EscapeDiscardsNewDraftWithoutSavingAndClosesFromPreview()
  {
    RunSta(() =>
    {
      int saves = 0;
      var window = new NoteWindow(
        new Note(Guid.NewGuid(), "# New note\n\n", default),
        text =>
        {
          saves++;
          return new Note(Guid.NewGuid(), text, DateTime.UtcNow);
        },
        edit: true);

      window.Show();

      try
      {
        window.NoteText = "# Unsaved draft\n\nDiscard me";
        PressEscape(window);

        Assert.IsFalse(window.IsEditing);
        Assert.IsTrue(window.IsVisible);
        Assert.AreEqual("# New note\n\n", window.NoteText);

        PressEscape(window);

        Assert.IsFalse(window.IsVisible);
        Assert.AreEqual(0, saves);
      }
      finally
      {
        window.Close();
      }
    });
  }

  [TestMethod]
  public void HeadersRoundTripSettingsAndKeepMetadataOutOfTheBody()
  {
    WithStore(store =>
    {
      var id = Guid.NewGuid();
      Directory.CreateDirectory(store.DirectoryPath);
      string path = Path.Combine(store.DirectoryPath, id.ToString("N") + ".md");
      const string Body = "# Header test\r\n\r\n!Reminder: This is body text.\r\n";

      File.WriteAllText(path, "!SaveDate: 2026-01-02_13:14:15\r\n!NoteColor: Blue\r\n!Reminder: 2026-12-01_09:30:00\r\n!ReminderSnooze: 10min\r\n!Custom: Keep me\r\n\r\n" + Body);
      var loaded = store.Load(id);

      Assert.AreEqual(Body, loaded.Text);
      Assert.AreEqual(new DateTime(2026, 1, 2, 13, 14, 15, DateTimeKind.Utc), loaded.SavedUtc);

      var saved = store.Save(id, Body + "More text");

      Assert.AreEqual("Blue", saved.Metadata["NoteColor"]);
      Assert.AreEqual("2026-12-01_09:30:00", saved.Metadata["Reminder"]);
      Assert.AreEqual("10min", saved.Metadata["ReminderSnooze"]);
      Assert.AreEqual("Keep me", saved.Metadata["Custom"]);
      Assert.AreEqual(Body + "More text", saved.Text);
      Assert.AreNotEqual(loaded.SavedUtc, saved.SavedUtc);

      string contents = File.ReadAllText(path);

      Assert.IsTrue(contents.StartsWith("!SaveDate: ", StringComparison.Ordinal));
      Assert.HasCount(1, System.Text.RegularExpressions.Regex.Matches(contents, "(?m)^!SaveDate:"));
    });
  }

  [TestMethod]
  public void LegacyNotesAcquireDefaultHeadersOnSaveAndDeleteRemovesOnlyTheirFile()
  {
    WithStore(store =>
    {
      Directory.CreateDirectory(store.DirectoryPath);
      var id = Guid.NewGuid();
      string path = Path.Combine(store.DirectoryPath, id.ToString("N") + ".md");
      File.WriteAllText(path, "# Legacy\n\nPlain text");
      var loaded = store.Load(id);

      Assert.AreEqual("# Legacy\n\nPlain text", loaded.Text);

      var saved = store.Save(id, loaded.Text);

      Assert.AreEqual("Yellow", saved.Metadata["NoteColor"]);
      Assert.AreEqual(string.Empty, saved.Metadata["Reminder"]);
      Assert.AreEqual("5min", saved.Metadata["ReminderSnooze"]);

      var other = store.Save(Guid.NewGuid(), "# Keep");
      store.Delete(id);
      store.Delete(id);

      Assert.IsFalse(File.Exists(path));
      Assert.AreEqual(other.Id, store.All().Single().Id);
    });
  }

  [TestMethod]
  public void MarkdownPreviewFormatsHeadingsListsEmphasisAndCodeWithoutLoadingHtml()
  {
    RunSta(() =>
    {
      var document = MarkdownPreview.Render("# Title\n\nHello **bold** and *italic*.\n\n- First\n- Second\n\n```cs\nvar x = 1;\n```\n\n<script>alert('no')</script>");
      var heading = (Paragraph)document.Blocks.FirstBlock;
      Assert.AreEqual(26d, heading.FontSize);

      var body = (Paragraph)heading.NextBlock;
      Assert.IsNotEmpty(body.Inlines.OfType<Bold>());
      Assert.IsNotEmpty(body.Inlines.OfType<Italic>());
      Assert.AreEqual(2, ((System.Windows.Documents.List)body.NextBlock).ListItems.Count);

      string text = new TextRange(document.ContentStart, document.ContentEnd).Text;
      Assert.Contains("var x = 1;", text);
      Assert.Contains("<script>", text);
    });
  }

  [TestMethod]
  public void PreferencesListShowsAllNotesOpensOnDoubleClickAndDeletesOpenDraft()
  {
    RunSta(() => WithStore(store =>
    {
      for (int i = 0; i < 12; i++)
        store.Save(Guid.NewGuid(), "# Managed " + i);

      using var provider = new ServiceCollection().AddDockzi().BuildServiceProvider();
      using var manager = new NotesManager(store, provider.GetRequiredService<ISettingsService>(), provider.GetRequiredService<Lite.EventIpc.IEventAggregator>());
      using var page = new Xeno.Dockzi.NotesAddin.PreferencesPage(manager) { ClientSize = new System.Drawing.Size(463, 380) };

      page.Show();
      var list = (System.Windows.Forms.ListView)page.Controls.Find("SavedNotes", true).Single();

      Assert.AreEqual(3, list.Columns.Count);
      Assert.IsGreaterThanOrEqualTo(100, list.Height, "Saved notes should remain usable at the host's preference-page size.");
      Assert.AreEqual(12, list.Items.Count);

      var note = (Note)list.Items[0].Tag;

      Assert.AreEqual(note.Title, list.Items[0].Text);
      Assert.AreEqual(note.SavedUtc.ToLocalTime().ToString("g"), list.Items[0].SubItems[1].Text);

      var titleBounds = list.Items[0].SubItems[0].Bounds;
      RaiseMouseEvent(list, "OnMouseDoubleClick", titleBounds);

      var window = PresentationSource.CurrentSources.OfType<System.Windows.Interop.HwndSource>()
        .Select(source => source.RootVisual).OfType<NoteWindow>().Single(window => window.Title == note.Title);

      try
      {
        window.BeginEdit();
        window.NoteText = "# Renamed from the note window";

        Assert.IsTrue(window.SaveNote());
        Assert.Contains(item => item.Text == "Renamed from the note window", list.Items.Cast<System.Windows.Forms.ListViewItem>());

        window.BeginEdit();
        window.NoteText = "# Dirty draft";
        var row = list.Items.Cast<System.Windows.Forms.ListViewItem>().Single(item => ((Note)item.Tag).Id == note.Id);
        RaiseMouseEvent(list, "OnMouseClick", row.SubItems[2].Bounds);

        Assert.IsFalse(window.IsVisible);
        Assert.AreEqual(11, list.Items.Count);
        Assert.HasCount(11, store.All());
        Assert.DoesNotContain(saved => saved.Id == note.Id, store.All());
      }
      finally
      {
        window.CloseWithoutSaving();
        page.Close();
      }
    }));
  }

  [TestMethod]
  public void SavePreservesMarkdownAndRecentNotesAreOrderedAndLimited()
  {
    WithStore(store =>
    {
      const string Text = "# Café 📝\n\n**Bold** and plain text\n";
      for (int i = 0; i < 12; i++)
      {
        var note = store.Save(Guid.NewGuid(), i == 0 ? Text : "# Note " + i);
        SetSavedDate(store, note, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMinutes(i));
      }

      File.WriteAllText(Path.Combine(store.DirectoryPath, "unrelated.md"), "leave me alone");
      var recent = store.Recent();

      Assert.HasCount(10, recent);
      Assert.AreEqual("Note 11", recent[0].Title);
      Assert.AreEqual("Note 2", recent[9].Title);

      var oldest = store.Recent(12).Last();
      Assert.AreEqual(Text, oldest.Text);
      Assert.AreEqual("Café 📝", oldest.Title);
      store.Save(oldest.Id, "# Updated\n\nNew text");

      Assert.AreEqual(oldest.Id, store.Recent()[0].Id);
      Assert.AreEqual(12, Directory.GetFiles(store.DirectoryPath, "*.md").Length - 1);
      Assert.IsEmpty(Directory.GetFiles(store.DirectoryPath, "*.tmp"));
    });
  }

  [TestMethod]
  public void ShortcutConflictsAreReportedAndStoppingReleasesTheRegistration()
  {
    RunSta(() => WithStore(store =>
    {
      using var provider = new ServiceCollection().AddDockzi().BuildServiceProvider();
      var events = provider.GetRequiredService<Lite.EventIpc.IEventAggregator>();
      var firstSettings = new ApplicationSettings(new SettingsManager(Config.StorageMethod.UnitTest));
      var secondSettings = new ApplicationSettings(new SettingsManager(Config.StorageMethod.UnitTest));

      firstSettings.SetValue(NotesManager.SettingsId, "ShortcutEnabled", false);
      secondSettings.SetValue(NotesManager.SettingsId, "ShortcutEnabled", false);

      using var first = new NotesManager(store, firstSettings, events);
      using var second = new NotesManager(store, secondSettings, events);

      first.Start();
      second.Start();

      var keys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F24;
      first.ConfigureShortcut(true, keys, false);
      first.Start();

      Assert.ThrowsExactly<Win32Exception>(() => second.ConfigureShortcut(true, keys, false));
      Assert.IsFalse(second.ShortcutEnabled);
      Assert.ThrowsExactly<ArgumentException>(() => first.ConfigureShortcut(true, System.Windows.Forms.Keys.N, false));
      Assert.AreEqual(keys, first.ShortcutKeys);

      first.Stop();
      second.ConfigureShortcut(true, keys, false);
      Assert.IsTrue(second.ShortcutEnabled);
    }));
  }

  [TestMethod]
  public void TitleUsesLevelOneHeadingOutsideCodeAndFallsBackForPlainText()
  {
    Assert.AreEqual("Meeting", Note.GetTitle("```md\n# Not title\n```\n## Subheading\n# Meeting ###\nBody"));
    Assert.AreEqual("Untitled note", Note.GetTitle("A plain text note\n## Subheading"));
    Assert.AreEqual("Real", Note.GetTitle("    # Code\n# Real"));
  }

  [TestMethod]
  public void TraySubmenuShowsOnlyTenMostRecentlySavedTitles()
  {
    RunSta(() => WithStore(store =>
    {
      for (int i = 0; i < 12; i++)
      {
        var note = store.Save(Guid.NewGuid(), "# Note " + i);
        SetSavedDate(store, note, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMinutes(i));
      }

      using var provider = new ServiceCollection().AddDockzi().BuildServiceProvider();
      using var manager = new NotesManager(store, provider.GetRequiredService<ISettingsService>(), provider.GetRequiredService<Lite.EventIpc.IEventAggregator>());
      using var menu = (System.Windows.Forms.ToolStripMenuItem)new SysTrayHandler(manager).MenuItems()[0];

      Assert.AreEqual("New note", menu.DropDownItems[0].Text);
      Assert.AreEqual(12, menu.DropDownItems.Count); // New, separator, ten notes.
      Assert.AreEqual("Note 11", menu.DropDownItems[2].Text);
      Assert.AreEqual("Note 2", menu.DropDownItems[11].Text);
    }));
  }

  [TestMethod]
  public void WindowSavesToPreviewReentersEditorAndPreservesDraftOnClose()
  {
    RunSta(() => WithStore(store =>
    {
      var id = Guid.NewGuid();
      var window = new NoteWindow(new Note(id, "# Draft\n\nHello **world**", default), text => store.Save(id, text), true);
      window.Show();

      try
      {
        Assert.IsTrue(window.IsEditing);
        Assert.AreEqual("Draft", window.Title);
        Assert.IsTrue(window.SaveNote());
        Assert.IsFalse(window.IsEditing);
        Assert.AreEqual("# Draft\n\nHello **world**", store.Load(id).Text);

        window.BeginEdit();

        Assert.IsTrue(window.IsEditing);

        window.NoteText = "# Renamed\n\nUnsaved changes";
        Assert.AreEqual("Renamed", window.Title);
      }
      finally
      {
        window.Close();
      }

      Assert.AreEqual("# Renamed\n\nUnsaved changes", store.Load(id).Text);
      var reopened = new NoteWindow(store.Load(id), text => store.Save(id, text));
      Assert.IsFalse(reopened.IsEditing);
      reopened.Close();
    }));
  }

  private static void PressEscape(NoteWindow window)
  {
    var key = new KeyEventArgs(Keyboard.PrimaryDevice, PresentationSource.FromVisual(window), Environment.TickCount, Key.Escape)
    {
      RoutedEvent = Keyboard.PreviewKeyDownEvent,
    };

    window.RaiseEvent(key);
    Assert.IsTrue(key.Handled);
  }

  private static void RaiseMouseEvent(System.Windows.Forms.Control control, string name, System.Drawing.Rectangle bounds) =>
    typeof(System.Windows.Forms.Control).GetMethod(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
    .Invoke(control, new object[] { new System.Windows.Forms.MouseEventArgs(System.Windows.Forms.MouseButtons.Left, name == "OnMouseDoubleClick" ? 2 : 1, bounds.Left + 8, bounds.Top + bounds.Height / 2, 0) });

  private static void RunSta(Action action)
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        failure = ex;
      }
    });

    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }

  private static void SetSavedDate(NoteStore store, Note note, DateTime savedUtc) =>
    File.WriteAllText(Path.Combine(store.DirectoryPath, note.Id.ToString("N") + ".md"), NoteFile.Serialize(note with { SavedUtc = savedUtc }));

  private static void WithStore(Action<NoteStore> action)
  {
    string path = Path.Combine(Path.GetTempPath(), "DockziNotesTests-" + Guid.NewGuid().ToString("N"));

    try
    {
      action(new NoteStore(path));
    }
    finally
    {
      if (Directory.Exists(path))
        Directory.Delete(path, true);
    }
  }
}

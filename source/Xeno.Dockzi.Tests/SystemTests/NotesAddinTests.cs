namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.NotesAddin;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Services.Addins;

[TestClass]
[DoNotParallelize]
public class NotesAddinTests
{
  [TestMethod]
  public void SavePreservesMarkdownAndRecentNotesAreOrderedAndLimited()
  {
    WithStore(store =>
    {
      const string Text = "# Café 📝\n\n**Bold** and plain text\n";
      for (int i = 0; i < 12; i++)
      {
        var note = store.Save(Guid.NewGuid(), i == 0 ? Text : "# Note " + i);
        File.SetLastWriteTimeUtc(Path.Combine(store.DirectoryPath, note.Id.ToString("N") + ".md"), new DateTime(2026, 1, 1).AddMinutes(i));
      }

      File.WriteAllText(Path.Combine(store.DirectoryPath, "unrelated.md"), "leave me alone");
      var recent = store.Recent();
      Assert.AreEqual(10, recent.Count);
      Assert.AreEqual("Note 11", recent[0].Title);
      Assert.AreEqual("Note 2", recent[9].Title);
      var oldest = store.Recent(12).Last();
      Assert.AreEqual(Text, oldest.Text);
      Assert.AreEqual("Café 📝", oldest.Title);
      store.Save(oldest.Id, "# Updated\n\nNew text");
      Assert.AreEqual(oldest.Id, store.Recent()[0].Id);
      Assert.AreEqual(12, Directory.GetFiles(store.DirectoryPath, "*.md").Length - 1);
      Assert.AreEqual(0, Directory.GetFiles(store.DirectoryPath, "*.tmp").Length);
    });
  }

  [TestMethod]
  public void TitleUsesLevelOneHeadingOutsideCodeAndFallsBackForPlainText()
  {
    Assert.AreEqual("Meeting", Note.GetTitle("```md\n# Not title\n```\n## Subheading\n# Meeting ###\nBody"));
    Assert.AreEqual("Untitled note", Note.GetTitle("A plain text note\n## Subheading"));
    Assert.AreEqual("Real", Note.GetTitle("    # Code\n# Real"));
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

  [TestMethod]
  public void MarkdownPreviewFormatsHeadingsListsEmphasisAndCodeWithoutLoadingHtml()
  {
    RunSta(() =>
    {
      var document = MarkdownPreview.Render("# Title\n\nHello **bold** and *italic*.\n\n- First\n- Second\n\n```cs\nvar x = 1;\n```\n\n<script>alert('no')</script>");
      var heading = (Paragraph)document.Blocks.FirstBlock;
      Assert.AreEqual(26d, heading.FontSize);
      var body = (Paragraph)heading.NextBlock;
      Assert.IsTrue(body.Inlines.OfType<Bold>().Any());
      Assert.IsTrue(body.Inlines.OfType<Italic>().Any());
      Assert.AreEqual(2, ((System.Windows.Documents.List)body.NextBlock).ListItems.Count);
      string text = new TextRange(document.ContentStart, document.ContentEnd).Text;
      StringAssert.Contains(text, "var x = 1;");
      StringAssert.Contains(text, "<script>");
    });
  }

  [TestMethod]
  public void BundledDiscoveryRegistersNotesWithoutCreatingWindowsOrHotkeys()
  {
    var services = new ServiceCollection().AddDockzi();
    services.AddBundledAddinServices(Path.GetDirectoryName(typeof(NotesManager).Assembly.Location));
    using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    Assert.IsNotNull(provider.GetRequiredService<NotesManager>());
  }

  [TestMethod]
  public void TraySubmenuShowsOnlyTenMostRecentlySavedTitles()
  {
    RunSta(() => WithStore(store =>
    {
      for (int i = 0; i < 12; i++)
      {
        var note = store.Save(Guid.NewGuid(), "# Note " + i);
        File.SetLastWriteTimeUtc(Path.Combine(store.DirectoryPath, note.Id.ToString("N") + ".md"), new DateTime(2026, 1, 1).AddMinutes(i));
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

  private static void RunSta(Action action)
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      try { action(); }
      catch (Exception ex) { failure = ex; }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();
    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }
}

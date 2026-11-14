namespace Xeno.Dockzi.NotesAddin;

using System.Collections.Generic;
using System.IO;
using System;
using System.Windows.Forms;
using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.ExtensionModel.SystemTray;

[Mono.Addins.Extension(NodeName = ExtensionName.SysTrayAddin, Path = ExtensionPath.SystemTray)]
public sealed class SysTrayHandler : SysTrayAddin
{
  private readonly NotesManager _manager;

  public SysTrayHandler(NotesManager manager) => _manager = manager;

  public override bool IsInitialized => true;

  public override List<ToolStripItem> MenuItems()
  {
    var menu = new ToolStripMenuItem("Notes");
    menu.DropDownItems.Add("New note", null, (_, _) => _manager.NewNote());
    menu.DropDownItems.Add(new ToolStripSeparator());

    try
    {
      var recent = _manager.Recent();
      foreach (var note in recent)
      {
        string title = note.Title.Length > 60 ? note.Title.Substring(0, 57) + "…" : note.Title;
        var item = new ToolStripMenuItem(title.Replace("&", "&&"), null, (_, _) => _manager.Open(note));
        item.ToolTipText = note.SavedUtc.ToLocalTime().ToString("g");
        menu.DropDownItems.Add(item);
      }

      if (recent.Count == 0)
        menu.DropDownItems.Add(new ToolStripMenuItem("No saved notes yet") { Enabled = false });
    }
    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
    {
      menu.DropDownItems.Add(new ToolStripMenuItem("Notes folder unavailable") { Enabled = false });
    }

    return new List<ToolStripItem> { menu };
  }
}

[Mono.Addins.Extension(NodeName = ExtensionName.UtilityAddin, Path = ExtensionPath.Utility)]
public sealed class UtilityHandler : UtilityAddin
{
  private readonly NotesManager _manager;

  public UtilityHandler(NotesManager manager) => _manager = manager;

  public override bool IsInitialized => true;

  public override void Execute() => _manager.Start();

  public override void Shutdown() => _manager.Stop();
}

[Mono.Addins.Extension(NodeName = ExtensionName.PreferencePageAddin, Path = ExtensionPath.PreferencePage)]
public sealed class PreferenceHandler : IPreferencePageExtension
{
  private readonly PreferencesPage _page;

  public PreferenceHandler(PreferencesPage page) => _page = page;

  public string Id { get; set; }

  public bool IsModified => _page.IsModified;

  public Form Page => _page;

  public string Title => "Notes";

  public void InitializePage()
  {
  }

  public void OnSave() => _page.Save();
}

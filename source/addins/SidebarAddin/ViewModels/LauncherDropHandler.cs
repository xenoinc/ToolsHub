namespace Xeno.ToolsHub.SidebarAddin.ViewModels;

using System;
using System.IO;
using System.Windows;

/// <summary>Interprets Explorer and browser drag data without launching items during a drop.</summary>
public sealed class LauncherDropHandler
{
  public const string ItemDragFormat = "ToolsHub.Sidebar.Launcher";
  private readonly ToolbarViewModel _model;

  public LauncherDropHandler(ToolbarViewModel model) => _model = model;

  public DragDropEffects GetEffect(IDataObject data, DragDropEffects allowedEffects)
  {
    if (data.GetDataPresent(ItemDragFormat))
      return data.GetData(ItemDragFormat) is LauncherIcon item && _model.Items.Contains(item)
        ? allowedEffects & DragDropEffects.Move : DragDropEffects.None;
    if (GetPaths(data).Length == 0)
      return DragDropEffects.None;
    if ((allowedEffects & DragDropEffects.Copy) != 0)
      return DragDropEffects.Copy;
    return allowedEffects & DragDropEffects.Link;
  }

  public bool Drop(IDataObject data, int insertionIndex)
  {
    if (data.GetDataPresent(ItemDragFormat))
      return data.GetData(ItemDragFormat) is LauncherIcon item && _model.Move(item, insertionIndex);
    string[] paths = GetPaths(data);
    return paths.Length > 0 && _model.AddPaths(paths, insertionIndex);
  }

  private static string[] GetPaths(IDataObject data)
  {
    if (data.GetData(DataFormats.FileDrop) is string[] paths)
      return paths;
    string text = data.GetData(DataFormats.UnicodeText) as string ?? data.GetData(DataFormats.Text) as string;
    if (string.IsNullOrWhiteSpace(text))
      return Array.Empty<string>();
    text = text.Trim().Trim('"');
    if (Uri.TryCreate(text, UriKind.Absolute, out var uri))
    {
      if (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
        return new[] { uri.AbsoluteUri };
      if (uri.IsFile && (File.Exists(uri.LocalPath) || Directory.Exists(uri.LocalPath)))
        return new[] { uri.LocalPath };
    }

    return File.Exists(text) || Directory.Exists(text) ? new[] { text } : Array.Empty<string>();
  }
}

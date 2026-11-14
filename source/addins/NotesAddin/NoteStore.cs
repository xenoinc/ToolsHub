namespace Xeno.Dockzi.NotesAddin;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public sealed record Note(Guid Id, string Text, DateTime SavedUtc)
{
  public string Title => GetTitle(Text);

  public static string GetTitle(string text)
  {
    // A title must be an ATX level-one heading, outside a fenced code block.
    bool fenced = false;
    char fence = '\0';
    int fenceLength = 0;
    foreach (string line in (text ?? string.Empty).Split('\n'))
    {
      string trimmed = line.TrimStart(' ');

      if (line.Length - trimmed.Length > 3)
        continue;

      var match = Regex.Match(trimmed, @"^(`{3,}|~{3,})");

      if (match.Success)
      {
        if (!fenced)
        {
          fenced = true;
          fence = trimmed[0];
          fenceLength = match.Length;
        }
        else if (trimmed[0] == fence && match.Length >= fenceLength && trimmed.Substring(match.Length).Trim().Length == 0)
          fenced = false;
        continue;
      }

      if (!fenced && (trimmed.StartsWith("# ", StringComparison.Ordinal) || trimmed.StartsWith("#\t", StringComparison.Ordinal)))
      {
        string title = Regex.Replace(trimmed.Substring(2).Trim(), @"\s+#+\s*$", string.Empty);
        if (title.Length > 0)
          return title;
      }
    }

    return "Untitled note";
  }
}

/// <summary>One UTF-8 Markdown file per note; saves replace files atomically.</summary>
public sealed class NoteStore
{
  public NoteStore(string directory) => DirectoryPath = Path.GetFullPath(directory);

  public string DirectoryPath { get; }

  public Note Load(Guid id)
  {
    string path = GetPath(id);
    return new Note(id, File.ReadAllText(path), File.GetLastWriteTimeUtc(path));
  }

  public IReadOnlyList<Note> Recent(int count = 10)
  {
    if (count <= 0 || !Directory.Exists(DirectoryPath))
      return Array.Empty<Note>();

    var notes = new List<Note>();

    foreach (var file in new DirectoryInfo(DirectoryPath).EnumerateFiles("*.md").OrderByDescending(file => file.LastWriteTimeUtc))
    {
      if (!Guid.TryParseExact(Path.GetFileNameWithoutExtension(file.Name), "N", out var id))
        continue;

      try
      {
        notes.Add(Load(id));
      }
      catch (IOException)
      {
        continue;
      }
      catch (UnauthorizedAccessException)
      {
        continue;
      }

      if (notes.Count >= count)
        break;
    }

    return notes;
  }

  public Note Save(Guid id, string text)
  {
    Directory.CreateDirectory(DirectoryPath);
    string path = GetPath(id);
    string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
    try
    {
      File.WriteAllText(temporary, text, new UTF8Encoding(false));
      File.Move(temporary, path, true);
    }
    finally
    {
      if (File.Exists(temporary))
        File.Delete(temporary);
    }

    return Load(id);
  }

  private string GetPath(Guid id) => Path.Combine(DirectoryPath, id.ToString("N") + ".md");
}

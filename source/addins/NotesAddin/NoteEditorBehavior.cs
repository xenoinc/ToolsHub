namespace Xeno.Dockzi.NotesAddin;

using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Markdig;
using Markdig.Syntax;

/// <summary>Keyboard editing conveniences for Markdown source.</summary>
internal static class NoteEditorBehavior
{
  private const string TabReplacement = "  ";

  public static void Attach(TextBox editor) => editor.PreviewKeyDown += (_, e) =>
  {
    if (Keyboard.Modifiers != ModifierKeys.None)
      return;

    if (e.Key == Key.Tab)
    {
      ReplaceSelection(editor, TabReplacement);
      e.Handled = true;
    }
    else if (e.Key == Key.Return && TryContinueList(editor))
    {
      e.Handled = true;
    }
  };

  private static void ReplaceSelection(TextBox editor, string replacement)
  {
    int start = editor.SelectionStart;
    editor.BeginChange();
    try
    {
      editor.SelectedText = replacement;
      editor.Select(start + replacement.Length, 0);
    }
    finally
    {
      editor.EndChange();
    }
  }

  private static bool TryContinueList(TextBox editor)
  {
    if (editor.SelectionLength != 0)
      return false;

    string text = editor.Text;
    int caret = editor.CaretIndex;
    int end = text.IndexOfAny(new[] { '\r', '\n' }, caret);
    if (end >= 0 && caret != end)
      return false;

    int start = caret == 0 ? 0 : text.LastIndexOfAny(new[] { '\r', '\n' }, caret - 1) + 1;
    var match = Regex.Match(text.Substring(start, caret - start), @"^([ \t]*)([-+*]|[0-9]+[.)])([ \t]+)(.*)$");
    if (!match.Success)
      return false;

    // Bullet-like source inside a fenced or indented code block stays ordinary text.
    int markerPosition = start + match.Groups[1].Length;
    if (Markdown.Parse(text).Descendants<CodeBlock>().Any(block => block.Lines.Count > 0 && block.Lines.Lines.Take(block.Lines.Count)
      .Any(line => line.Position <= markerPosition && line.Position + line.Slice.Length >= markerPosition)))
      return false;

    if (string.IsNullOrWhiteSpace(match.Groups[4].Value))
    {
      editor.Select(start, caret - start);
      ReplaceSelection(editor, string.Empty);
      return true;
    }

    string marker = match.Groups[2].Value;
    if (marker.Length > 1)
    {
      var number = BigInteger.Parse(marker.Substring(0, marker.Length - 1), CultureInfo.InvariantCulture);
      marker = (number + BigInteger.One).ToString(CultureInfo.InvariantCulture) + marker[marker.Length - 1];
    }

    string newline = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : text.Contains('\n') ? "\n" : Environment.NewLine;
    ReplaceSelection(editor, newline + match.Groups[1].Value + marker + match.Groups[3].Value);
    return true;
  }
}

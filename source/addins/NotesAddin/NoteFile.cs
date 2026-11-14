namespace Xeno.Dockzi.NotesAddin;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>Separates the leading ! settings from the Markdown body.</summary>
public static class NoteFile
{
  public const string DateFormat = "yyyy-MM-dd_HH:mm:ss";

  public static Note Parse(Guid id, string contents, DateTime fallbackSavedUtc)
  {
    var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    int offset = 0;

    while (offset < contents.Length)
    {
      int end = contents.IndexOf('\n', offset);
      if (end < 0)
        end = contents.Length;

      var match = Regex.Match(contents.Substring(offset, end - offset).TrimEnd('\r'), @"^!([A-Za-z][A-Za-z0-9]*):[ \t]*(.*)$");

      if (!match.Success)
        break;
      metadata[match.Groups[1].Value] = match.Groups[2].Value;
      offset = end < contents.Length ? end + 1 : end;
    }

    // Consume only the header separator, preserving whitespace in the note itself.
    if (metadata.Count > 0)
    {
      if (contents.Substring(offset).StartsWith("\r\n", StringComparison.Ordinal))
        offset += 2;
      else if (offset < contents.Length && contents[offset] == '\n')
        offset++;
    }

    DateTime savedUtc = fallbackSavedUtc;
    if (metadata.TryGetValue("SaveDate", out string date)
      && DateTime.TryParseExact(
        date,
        DateFormat,
        CultureInfo.InvariantCulture,
        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
        out var parsed))
    {
      savedUtc = parsed;
    }

    return new Note(id, contents.Substring(offset), savedUtc) { Metadata = metadata };
  }

  public static string Serialize(Note note)
  {
    var header = new StringBuilder();
    header.Append("!SaveDate: ").AppendLine(note.SavedUtc.ToUniversalTime().ToString(DateFormat, CultureInfo.InvariantCulture));
    AppendSetting(header, note, "NoteColor", "Yellow");
    AppendSetting(header, note, "Reminder", string.Empty);
    AppendSetting(header, note, "ReminderSnooze", "5min");

    foreach (var entry in note.Metadata)
    {
      if (entry.Key.Equals("SaveDate", StringComparison.OrdinalIgnoreCase) ||
          entry.Key.Equals("NoteColor", StringComparison.OrdinalIgnoreCase) ||
          entry.Key.Equals("Reminder", StringComparison.OrdinalIgnoreCase) ||
          entry.Key.Equals("ReminderSnooze", StringComparison.OrdinalIgnoreCase))
      {
        continue;
      }

      header.Append('!').Append(entry.Key).Append(": ").AppendLine(entry.Value);
    }

    return header.AppendLine().Append(note.Text).ToString();
  }

  private static void AppendSetting(StringBuilder header, Note note, string key, string fallback) =>
    header
    .Append('!')
    .Append(key)
    .Append(": ")
    .AppendLine(note.Metadata.TryGetValue(key, out var value) ? value : fallback);
}

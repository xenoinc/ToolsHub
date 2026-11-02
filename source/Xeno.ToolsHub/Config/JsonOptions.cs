namespace Xeno.ToolsHub.Config;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>Shared settings for application JSON and legacy settings-file compatibility.</summary>
public static class JsonOptions
{
  public static JsonSerializerOptions Compact { get; } = new()
  {
    PropertyNameCaseInsensitive = true,
    AllowTrailingCommas = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
  };

  public static JsonSerializerOptions Indented { get; } = new(Compact) { WriteIndented = true };
}

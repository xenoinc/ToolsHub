namespace Xeno.Dockzi.Services;

using System;
using System.IO;
using System.Text.Json;

public sealed class InstallationConfiguration
{
  public string UpdateUrl { get; set; } = string.Empty;

  public string Channel { get; set; } = "win";

  public static InstallationConfiguration Load(string applicationDirectory)
  {
    string path = Path.Combine(applicationDirectory, "Dockzi.install.json");
    if (!File.Exists(path))
      return new InstallationConfiguration();
    var config = JsonSerializer.Deserialize<InstallationConfiguration>(File.ReadAllText(path)) ?? new InstallationConfiguration();
    if (!string.IsNullOrEmpty(config.UpdateUrl) && (!Uri.TryCreate(config.UpdateUrl, UriKind.Absolute, out var uri) || uri.Scheme != Uri.UriSchemeHttps))
      throw new InvalidDataException("The Dockzi update feed must use HTTPS.");
    return config;
  }
}

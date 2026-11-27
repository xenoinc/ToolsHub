namespace Xeno.Dockzi.Services;

using System;
using System.IO;
using System.Text.Json;
using Velopack.Sources;

public sealed class InstallationConfiguration
{
  public string Channel { get; set; } = "win";

  public string UpdateUrl { get; set; } = string.Empty;

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

  public IUpdateSource CreateUpdateSource(bool receivePreviewUpdates = false)
  {
    var uri = new Uri(UpdateUrl, UriKind.Absolute);
    if (uri.Scheme != Uri.UriSchemeHttps)
      throw new InvalidDataException("The Dockzi update feed must use HTTPS.");

    if (string.Equals(uri.Host, "github.com", StringComparison.OrdinalIgnoreCase))
      return new DockziGithubSource(UpdateUrl, receivePreviewUpdates);

    return new SimpleWebSource(UpdateUrl);
  }
}

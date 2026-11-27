namespace Xeno.Dockzi.Services;

using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Velopack.Sources;

/// <summary>Keeps stable releases discoverable when frequent previews fill GitHub's first release page.</summary>
public sealed class DockziGithubSource : GithubSource
{
  public DockziGithubSource(string repositoryUrl, bool receivePreviewUpdates, IFileDownloader downloader = null)
    : base(repositoryUrl, accessToken: null, prerelease: receivePreviewUpdates, downloader: downloader)
  {
  }

  protected override async Task<GithubRelease[]> GetReleases(bool includePrereleases)
  {
    if (includePrereleases)
      return await base.GetReleases(true).ConfigureAwait(false);

    var endpoint = new Uri(GetApiBaseUrl(RepoUri), $"repos{RepoUri.AbsolutePath}/releases/latest");
    try
    {
      string json = await Downloader.DownloadString(endpoint.AbsoluteUri, GetRequestHeaders("application/vnd.github+json"), 0.25).ConfigureAwait(false);
      var release = JsonSerializer.Deserialize<GithubRelease>(json);
      if (release == null || release.Prerelease)
        return Array.Empty<GithubRelease>();

      return new[] { release };
    }
    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    {
      return Array.Empty<GithubRelease>();
    }
  }
}

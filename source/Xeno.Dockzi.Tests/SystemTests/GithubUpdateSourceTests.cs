namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Velopack.Sources;
using Xeno.Dockzi.Services;

[TestClass]
[OSCondition(OperatingSystems.Windows)]
public class GithubUpdateSourceTests
{
  [TestMethod]
  public async Task PreviewChecksRetainBothStableAndPrereleaseCandidatesAsync()
  {
    var downloader = new FakeDownloader
    {
      Response = "[{\"name\":\"Preview\",\"prerelease\":true,\"assets\":[]},{\"name\":\"Stable\",\"prerelease\":false,\"assets\":[]}]",
    };

    var source = new DockziGithubSource("https://github.com/xenoinc/ToolsHub", true, downloader);
    var releases = await GetReleases(source, true);

    Assert.HasCount(2, releases);
    Assert.Contains("/releases?", downloader.Url);
  }

  [TestMethod]
  public async Task RepositoryWithOnlyPreviewsHasNoStableUpdateAsync()
  {
    var downloader = new FakeDownloader { MissingStable = true };
    var releases = await GetReleases(new DockziGithubSource("https://github.com/xenoinc/ToolsHub", false, downloader), false);

    Assert.IsEmpty(releases);
  }

  [TestMethod]
  public async Task StableChecksUseTheLatestStableEndpointRegardlessOfPreviewVolumeAsync()
  {
    var downloader = new FakeDownloader { Response = "{\"name\":\"Stable build\",\"prerelease\":false,\"assets\":[]}" };
    var source = new DockziGithubSource("https://github.com/xenoinc/ToolsHub", false, downloader);
    var releases = await GetReleases(source, false);

    Assert.HasCount(1, releases);
    Assert.AreEqual("Stable build", releases[0].Name);
    Assert.AreEqual("https://api.github.com/repos/xenoinc/ToolsHub/releases/latest", downloader.Url);
  }

  private static Task<GithubRelease[]> GetReleases(DockziGithubSource source, bool previews) =>
    typeof(DockziGithubSource).GetMethod("GetReleases", BindingFlags.Instance | BindingFlags.NonPublic)
      .CreateDelegate<Func<bool, Task<GithubRelease[]>>>(source)(previews);

  private sealed class FakeDownloader : IFileDownloader
  {
    public bool MissingStable { get; set; }

    public string Response { get; set; }

    public string Url { get; private set; }

    public Task<byte[]> DownloadBytes(string url, IDictionary<string, string> headers = null, double timeout = 3) => throw new NotSupportedException();

    public Task DownloadFile(string url, string targetFile, Action<int> progress, IDictionary<string, string> headers = null, double timeout = 30, CancellationToken cancelToken = default) =>
      throw new NotSupportedException();

    public Task<string> DownloadString(string url, IDictionary<string, string> headers = null, double timeout = 3)
    {
      Url = url;
      return MissingStable
        ? Task.FromException<string>(new HttpRequestException("No stable release.", null, HttpStatusCode.NotFound))
        : Task.FromResult(Response);
    }
  }
}

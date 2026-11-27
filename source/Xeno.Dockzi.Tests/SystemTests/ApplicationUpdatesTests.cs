namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Velopack;
using Velopack.Sources;
using Xeno.Dockzi.Services;

[TestClass]
[OSCondition(OperatingSystems.Windows)]
public class ApplicationUpdatesTests
{
  [TestMethod]
  public void CoreBuildIncrementsAreNewerVelopackPackageVersions()
  {
    var previous = SemanticVersion.Parse("0.9.1-build.57");
    var next = SemanticVersion.Parse("0.9.1-build.58");
    Assert.IsTrue(next > previous);
    Assert.IsTrue(previous > SemanticVersion.Parse("0.6.0"));
    Assert.IsTrue(SemanticVersion.Parse("0.9.2-build.0") > next);
    Assert.IsTrue(SemanticVersion.Parse("0.9.1-build.100") > next);

    var stable = SemanticVersion.Parse("0.9.1-build.58.stable");
    Assert.IsTrue(stable > next);
    Assert.IsTrue(SemanticVersion.Parse("0.9.1-build.59") > stable);
  }

  [TestMethod]
  public async Task CurrentVersionContinuesNormalStartupWithoutDownloadingAsync()
  {
    var client = new FakeClient();
    var updates = CreateUpdates(client);
    Assert.IsFalse(await updates.PrepareStartupUpdateAsync());

    updates.ApplyPendingUpdate();
    Assert.AreEqual(1, client.CheckCount);
    Assert.AreEqual(0, client.DownloadCount);
    Assert.AreEqual(0, client.ApplyCount);
  }

  [TestMethod]
  [DataRow(false, false)]
  [DataRow(true, true)]
  public async Task DevelopmentAndPortableCopiesDoNotCheckAsync(bool installed, bool portable)
  {
    var client = new FakeClient { IsInstalled = installed, IsPortable = portable };
    var updates = CreateUpdates(client);
    Assert.IsFalse(await updates.PrepareStartupUpdateAsync());
    Assert.AreEqual(0, client.CheckCount);
  }

  [TestMethod]
  public async Task EmptyFeedDoesNotCreateAClientAsync()
  {
    var updates = new ApplicationUpdates(new InstallationConfiguration(), _ => throw new InvalidOperationException("Unexpected update client."));
    Assert.IsFalse(await updates.PrepareStartupUpdateAsync());
  }

  [TestMethod]
  public void GithubRepositoriesUseTheGithubReleaseSource()
  {
    var configuration = new InstallationConfiguration { UpdateUrl = "https://github.com/xenoinc/ToolsHub" };
    Assert.IsInstanceOfType<GithubSource>(configuration.CreateUpdateSource());

    configuration.UpdateUrl = "https://updates.example.com/dockzi/";
    Assert.IsInstanceOfType<SimpleWebSource>(configuration.CreateUpdateSource());

    configuration.UpdateUrl = "http://updates.example.com/dockzi/";
    Assert.ThrowsExactly<InvalidDataException>(() => configuration.CreateUpdateSource());
  }

  [TestMethod]
  [DataRow(true)]
  [DataRow(false)]
  public async Task NetworkOrDownloadFailureDoesNotBlockStartupOrApplyAPartialUpdateAsync(bool failDuringCheck)
  {
    var client = new FakeClient { HasUpdate = true, FailCheck = failDuringCheck, FailDownload = !failDuringCheck };
    var updates = CreateUpdates(client);
    Assert.IsFalse(await updates.PrepareStartupUpdateAsync());

    updates.ApplyPendingUpdate();
    Assert.AreEqual(0, client.ApplyCount);

    client.FailCheck = false;
    client.FailDownload = false;
    Assert.IsTrue(await updates.PrepareStartupUpdateAsync());
  }

  [TestMethod]
  public async Task SimultaneousChecksDoNotDownloadTwiceAsync()
  {
    var client = new FakeClient { CheckCompletion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously) };
    var updates = CreateUpdates(client);
    var first = updates.PrepareStartupUpdateAsync();
    Assert.IsFalse(await updates.PrepareStartupUpdateAsync());

    client.CheckCompletion.SetResult(true);
    Assert.IsTrue(await first);
    Assert.AreEqual(1, client.CheckCount);
    Assert.AreEqual(1, client.DownloadCount);
  }

  [TestMethod]
  public async Task StartupDownloadsThenDefersRestartUntilServicesHaveShutDownAsync()
  {
    var client = new FakeClient { HasUpdate = true };
    var updates = CreateUpdates(client);
    Assert.IsTrue(await updates.PrepareStartupUpdateAsync());
    Assert.IsTrue(await updates.PrepareStartupUpdateAsync());
    Assert.AreEqual(1, client.CheckCount);
    Assert.AreEqual(1, client.DownloadCount);
    Assert.IsTrue(client.CancellableDownload);
    Assert.AreEqual(0, client.ApplyCount);

    updates.ApplyPendingUpdate();
    updates.ApplyPendingUpdate();
    Assert.AreEqual(1, client.ApplyCount);
  }

  private static ApplicationUpdates CreateUpdates(FakeClient client) =>
    new(new InstallationConfiguration { UpdateUrl = "https://github.com/xenoinc/ToolsHub" }, _ => client);

  private sealed class FakeClient : IApplicationUpdateClient
  {
    public int ApplyCount { get; private set; }

    public bool CancellableDownload { get; private set; }

    public TaskCompletionSource<bool> CheckCompletion { get; set; }

    public int CheckCount { get; private set; }

    public int DownloadCount { get; private set; }

    public bool FailCheck { get; set; }

    public bool FailDownload { get; set; }

    public bool HasUpdate { get; set; }

    public bool IsInstalled { get; set; } = true;

    public bool IsPortable { get; set; }

    public void ApplyUpdatesAndRestart() => ApplyCount++;

    public Task<bool> CheckForUpdatesAsync()
    {
      CheckCount++;
      if (FailCheck)
        throw new IOException("Test feed unavailable.");

      return CheckCompletion?.Task ?? Task.FromResult(HasUpdate);
    }

    public Task DownloadUpdatesAsync(CancellationToken cancellationToken)
    {
      DownloadCount++;
      CancellableDownload = cancellationToken.CanBeCanceled;
      if (FailDownload)
        throw new IOException("Test download failed.");

      return Task.CompletedTask;
    }
  }
}

namespace Xeno.Dockzi.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using Velopack;

internal sealed class VelopackUpdateClient : IApplicationUpdateClient
{
  private readonly UpdateManager _manager;
  private UpdateInfo _available;

  public VelopackUpdateClient(InstallationConfiguration configuration)
  {
    _manager = new UpdateManager(configuration.CreateUpdateSource(), new UpdateOptions { ExplicitChannel = configuration.Channel });
  }

  public bool IsInstalled => _manager.IsInstalled;

  public bool IsPortable => _manager.IsPortable;

  public void ApplyUpdatesAndRestart() => _manager.ApplyUpdatesAndRestart(_available.TargetFullRelease);

  public async Task<bool> CheckForUpdatesAsync()
  {
    _available = await _manager.CheckForUpdatesAsync().ConfigureAwait(false);
    return _available != null;
  }

  public Task DownloadUpdatesAsync(CancellationToken cancellationToken)
  {
    if (_available == null)
      throw new InvalidOperationException("Check for an available update before downloading.");

    return _manager.DownloadUpdatesAsync(_available, cancelToken: cancellationToken);
  }
}

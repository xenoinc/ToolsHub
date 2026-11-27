namespace Xeno.Dockzi.Services;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xeno.Dockzi.Services.Logging;

/// <summary>Checks at launch or on request, then applies updates after host services have shut down.</summary>
public sealed class ApplicationUpdates
{
  private readonly InstallationConfiguration _configuration;
  private readonly Func<InstallationConfiguration, IApplicationUpdateClient> _createClient;
  private bool _busy;
  private IApplicationUpdateClient _client;
  private bool _pending;

  public ApplicationUpdates()
    : this(LoadConfiguration(), configuration => new VelopackUpdateClient(configuration))
  {
  }

  public ApplicationUpdates(InstallationConfiguration configuration, Func<InstallationConfiguration, IApplicationUpdateClient> createClient)
  {
    _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    _createClient = createClient ?? throw new ArgumentNullException(nameof(createClient));
  }

  public bool IsConfigured => !string.IsNullOrWhiteSpace(_configuration.UpdateUrl);

  public void ApplyPendingUpdate()
  {
    if (_pending)
    {
      _pending = false;
      _client.ApplyUpdatesAndRestart();
    }
  }

  public async Task CheckAndInstallAsync()
  {
    if (_busy || !IsConfigured)
      return;
    _busy = true;
    try
    {
      _client ??= _createClient(_configuration);
      if (!_client.IsInstalled || _client.IsPortable)
      {
        MessageBox.Show("Install Dockzi using Setup.exe to receive application updates.", "Dockzi updates");
        return;
      }

      if (!await _client.CheckForUpdatesAsync())
      {
        MessageBox.Show("Dockzi is up to date.", "Dockzi updates");
        return;
      }

      if (MessageBox.Show("A Dockzi update is available. Download it and restart Dockzi?", "Dockzi updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
        return;
      await _client.DownloadUpdatesAsync(CancellationToken.None);
      _pending = true;
      Application.Exit();
    }
    catch (Exception ex)
    {
      Log.Warn("Unable to update Dockzi: " + ex.Message);
      MessageBox.Show("Unable to update Dockzi. " + ex.Message, "Dockzi updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
      _busy = false;
    }
  }

  /// <summary>Prepares an automatic update before add-ins and editable note windows are opened.</summary>
  /// <returns>True when normal startup should be skipped so the prepared update can be applied.</returns>
  public async Task<bool> PrepareStartupUpdateAsync()
  {
    if (_pending)
      return true;

    if (_busy || !IsConfigured)
      return false;

    _busy = true;
    try
    {
      _client = _createClient(_configuration);
      if (!_client.IsInstalled || _client.IsPortable)
        return false;

      Log.Info("Checking for Dockzi updates at startup.");
      if (!await _client.CheckForUpdatesAsync().WaitAsync(TimeSpan.FromSeconds(15)).ConfigureAwait(false))
        return false;

      using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));
      await _client.DownloadUpdatesAsync(timeout.Token).ConfigureAwait(false);
      _pending = true;
      Log.Info("Dockzi startup update downloaded; applying before opening add-ins.");
      return true;
    }
    catch (Exception ex)
    {
      Log.Warn("Startup update unavailable; continuing to launch Dockzi. " + ex.Message);
      _client = null;
      return false;
    }
    finally
    {
      _busy = false;
    }
  }

  private static InstallationConfiguration LoadConfiguration()
  {
    try
    {
      return InstallationConfiguration.Load(AppContext.BaseDirectory);
    }
    catch (Exception ex)
    {
      Log.Warn("Application updates are unavailable: " + ex.Message);
      return new InstallationConfiguration();
    }
  }
}

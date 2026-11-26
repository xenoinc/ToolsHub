namespace Xeno.Dockzi.Services;

using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Velopack;
using Xeno.Dockzi.Services.Logging;

/// <summary>Downloads an update on request, then applies it after host services have shut down.</summary>
public sealed class ApplicationUpdates
{
  private readonly InstallationConfiguration _configuration;
  private UpdateManager _manager;
  private UpdateInfo _pending;
  private bool _busy;

  public ApplicationUpdates()
  {
    try
    {
      _configuration = InstallationConfiguration.Load(AppContext.BaseDirectory);
    }
    catch (Exception ex)
    {
      Log.Warn("Application updates are unavailable: " + ex.Message);
      _configuration = new InstallationConfiguration();
    }
  }

  public bool IsConfigured => !string.IsNullOrWhiteSpace(_configuration.UpdateUrl);

  public async Task CheckAndInstallAsync()
  {
    if (_busy || !IsConfigured)
      return;
    _busy = true;
    try
    {
      _manager = new UpdateManager(_configuration.UpdateUrl, new UpdateOptions { ExplicitChannel = _configuration.Channel });
      if (!_manager.IsInstalled)
      {
        MessageBox.Show("Install Dockzi using Setup.exe to receive application updates.", "Dockzi updates");
        return;
      }

      var update = await _manager.CheckForUpdatesAsync();
      if (update == null)
      {
        MessageBox.Show("Dockzi is up to date.", "Dockzi updates");
        return;
      }

      if (MessageBox.Show("A Dockzi update is available. Download it and restart Dockzi?", "Dockzi updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
        return;
      await _manager.DownloadUpdatesAsync(update);
      _pending = update;
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

  public void ApplyPendingUpdate()
  {
    if (_pending != null)
      _manager.ApplyUpdatesAndRestart(_pending.TargetFullRelease);
  }
}

namespace Xeno.Dockzi.Services;

using System.Threading;
using System.Threading.Tasks;

/// <summary>Installation-aware update operations, separate from startup and manual-check UI.</summary>
public interface IApplicationUpdateClient
{
  bool IsInstalled { get; }

  bool IsPortable { get; }

  Task<bool> CheckForUpdatesAsync();

  Task DownloadUpdatesAsync(CancellationToken cancellationToken);

  void ApplyUpdatesAndRestart();
}

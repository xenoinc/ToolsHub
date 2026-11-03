namespace Xeno.ToolsHub.SidebarAddin.Managers;

using System;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Services;

public sealed class SidebarManager : IDisposable
{
  private readonly ISettingsService _settings;
  private readonly ISidebarWindowFactory _windows;
  private ISidebarWindow _window;
  private bool _disposed;

  public SidebarManager(ISettingsService settings, ISidebarWindowFactory windows)
  {
    _settings = settings;
    _windows = windows;
  }

  public string SettingBackground => _settings.GetString(Constants.AddinId, Constants.KeyBackground, string.Empty);

  public int SettingOpacity => _settings.GetInt(Constants.AddinId, Constants.KeyOpacity, 90);

  public int SettingScreenArea => _settings.GetInt(Constants.AddinId, Constants.KeyScreenArea, 1);

  public int SettingScreenOrientation => _settings.GetInt(Constants.AddinId, Constants.KeyScreenOrientation, 0);

  public int SettingScreenNumber => _settings.GetInt(Constants.AddinId, Constants.KeyScreenNumber, 1);

  public bool SettingsShowSidebar => _settings.GetBool(Constants.AddinId, Constants.KeyShowSidebars, true);

  public string SettingTintColor => _settings.GetString(Constants.AddinId, Constants.KeyTintColor, string.Empty);

  public void ShowSidebars()
  {
    ObjectDisposedException.ThrowIf(_disposed, this);
    if (_window != null)
      return;
    _window = _windows.Create();
    _window.Closed += OnClosed;
    _window.Show();
  }

  public void CloseSidebars()
  {
    var window = _window;
    _window = null;
    if (window == null)
      return;
    window.Closed -= OnClosed;
    window.Close();
  }

  public void ApplySettings()
  {
    if (_window == null)
      return;
    _window.ApplySettings(SidebarAppearance.Load(_settings));
  }

  public void PreviewSettings(SidebarAppearance appearance) => _window?.ApplySettings(appearance);

  public void Dispose()
  {
    if (_disposed)
      return;
    _disposed = true;
    CloseSidebars();
  }

  private void OnClosed(object sender, EventArgs args)
  {
    if (sender is ISidebarWindow window)
      window.Closed -= OnClosed;
    if (ReferenceEquals(sender, _window))
      _window = null;
  }
}

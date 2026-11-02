namespace Xeno.ToolsHub.SidebarAddin.Managers;

using System;
using Lite.EventIpc;
using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.SidebarAddin.Services;

public sealed class SidebarManager : IDisposable
{
  private readonly ISettingsService _settings;
  private readonly IEventAggregator _events;
  private readonly ISidebarWindowFactory _windows;
  private ISidebarWindow _window;
  private bool _disposed;

  public SidebarManager(ISettingsService settings, IEventAggregator events, ISidebarWindowFactory windows)
  {
    _settings = settings;
    _events = events;
    _windows = windows;
  }

  public string SettingBackground => _settings.GetString(Constants.AddinId, Constants.KeyBackground, string.Empty);

  public int SettingOpacity => _settings.GetInt(Constants.AddinId, Constants.KeyOpacity, 0);

  public int SettingScreenArea => _settings.GetInt(Constants.AddinId, Constants.KeyScreenArea, 1);

  public int SettingScreenOrientation => _settings.GetInt(Constants.AddinId, Constants.KeyScreenOrientation, 0);

  public int SettingScreenNumber => _settings.GetInt(Constants.AddinId, Constants.KeyScreenNumber, 1);

  public bool SettingsShowSidebar => _settings.GetBool(Constants.AddinId, Constants.KeyShowSidebars, true);

  public string SettingTintColor => _settings.GetString(Constants.AddinId, Constants.KeyTintColor, string.Empty);

  public ShortcutItems GetShortcuts() =>
    _settings.GetObject<ShortcutItems>(ShortcutsManager.ShortcutsAddinId, ShortcutsManager.ShortcutItemsKey) ?? new ShortcutItems();

  public void ShowSidebars()
  {
    ObjectDisposedException.ThrowIf(_disposed, this);
    if (!SettingsShowSidebar || _window != null)
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

  public void SendMessageRefreshShortcuts() => _events.Publish(new ShortcutsRefreshRequested());

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

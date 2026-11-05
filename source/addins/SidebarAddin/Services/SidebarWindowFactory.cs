namespace Xeno.Dockzi.SidebarAddin.Services;

using System;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.SidebarAddin.Domain;
using Xeno.Dockzi.SidebarAddin.ViewModels;
using Xeno.Dockzi.SidebarAddin.Views;

public sealed class SidebarWindowFactory : ISidebarWindowFactory
{
  private readonly IRunningApplications _running;
  private readonly ISettingsService _settings;
  private readonly ILauncherShell _shell;
  private readonly ILauncherStore _store;

  public SidebarWindowFactory(ILauncherStore store, ILauncherShell shell, ISettingsService settings, IRunningApplications running)
  {
    _store = store;
    _shell = shell;
    _settings = settings;
    _running = running;
  }

  public ISidebarWindow Create() => new SidebarWindow(new ToolbarView(new ToolbarViewModel(_store, _shell, _settings, _running)));

  private sealed class SidebarWindow : ISidebarWindow
  {
    private readonly ToolbarView _view;

    public SidebarWindow(ToolbarView view)
    {
      _view = view;
      _view.Closed += OnClosed;
    }

    public event EventHandler Closed;

    public void ApplySettings(SidebarAppearance appearance) => _view.ApplySettings(appearance);

    public void Close() => _view.Close();

    public void Show() => _view.Show();

    private void OnClosed(object sender, EventArgs args)
    {
      _view.Closed -= OnClosed;
      Closed?.Invoke(this, args);
    }
  }
}

namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;
using Xeno.ToolsHub.SidebarAddin.ViewModels;
using Xeno.ToolsHub.SidebarAddin.Views;

public sealed class SidebarWindowFactory : ISidebarWindowFactory
{
  public ISidebarWindow Create() => new SidebarWindow();

  private sealed class SidebarWindow : ISidebarWindow
  {
    private readonly ToolbarView _view = new() { ShowInTaskbar = true };

    public SidebarWindow()
    {
      _view.DataContext = new ToolbarViewModel(_view.StackDynamic);
      _view.Closed += OnClosed;
    }

    public event EventHandler Closed;

    public void Show() => _view.Show();

    public void Close() => _view.Close();

    private void OnClosed(object sender, EventArgs args)
    {
      _view.Closed -= OnClosed;
      Closed?.Invoke(this, args);
    }
  }
}

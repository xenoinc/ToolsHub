namespace Xeno.ToolsHub.SidebarAddin.ViewModels;

using System.ComponentModel;
using System.Windows.Media;
using Xeno.ToolsHub.SidebarAddin.Models;

public sealed class LauncherIcon : INotifyPropertyChanged
{
  private bool _isRunning;

  public LauncherIcon(DockLauncher item, ImageSource image)
  {
    Item = item;
    Image = image;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public bool IsRunning
  {
    get => _isRunning;
    set
    {
      if (_isRunning == value)
        return;
      _isRunning = value;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
    }
  }

  public DockLauncher Item { get; }

  public ImageSource Image { get; }

  public string Title => string.IsNullOrWhiteSpace(Item.Title) ? Item.Target : Item.Title;
}

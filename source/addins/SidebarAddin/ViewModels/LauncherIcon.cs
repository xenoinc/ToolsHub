namespace Xeno.ToolsHub.SidebarAddin.ViewModels;

using System.Windows.Media;
using Xeno.ToolsHub.SidebarAddin.Models;

public sealed class LauncherIcon
{
  public LauncherIcon(DockLauncher item, ImageSource image)
  {
    Item = item;
    Image = image;
  }

  public DockLauncher Item { get; }

  public ImageSource Image { get; }

  public string Title => string.IsNullOrWhiteSpace(Item.Title) ? Item.Target : Item.Title;
}

namespace Xeno.Dockzi.SidebarAddin.Services;

using System.Windows.Media;
using Xeno.Dockzi.SidebarAddin.Models;

public interface ILauncherShell
{
  ImageSource GetIcon(string target);

  DockLauncher Import(string path);

  void Launch(DockLauncher item);
}

namespace Xeno.ToolsHub.SidebarAddin.Services;

using System.Windows.Media;
using Xeno.ToolsHub.SidebarAddin.Models;

public interface ILauncherShell
{
  DockLauncher Import(string path);

  ImageSource GetIcon(string target);

  void Launch(DockLauncher item);
}

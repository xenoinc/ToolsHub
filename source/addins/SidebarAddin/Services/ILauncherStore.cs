namespace Xeno.Dockzi.SidebarAddin.Services;

using System.Collections.Generic;
using Xeno.Dockzi.SidebarAddin.Models;

public interface ILauncherStore
{
  IReadOnlyList<DockLauncher> Load();

  void Save(IReadOnlyList<DockLauncher> items);
}

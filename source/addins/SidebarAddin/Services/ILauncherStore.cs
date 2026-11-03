namespace Xeno.ToolsHub.SidebarAddin.Services;

using System.Collections.Generic;
using Xeno.ToolsHub.SidebarAddin.Models;

public interface ILauncherStore
{
  IReadOnlyList<DockLauncher> Load();

  void Save(IReadOnlyList<DockLauncher> items);
}

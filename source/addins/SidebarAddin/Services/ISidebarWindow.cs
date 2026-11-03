namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;
using Xeno.ToolsHub.SidebarAddin.Domain;

public interface ISidebarWindow
{
  event EventHandler Closed;

  void ApplySettings(SidebarAppearance appearance);

  void Close();

  void Show();
}

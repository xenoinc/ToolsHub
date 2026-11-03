namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;
using Xeno.ToolsHub.SidebarAddin.Domain;

public interface ISidebarWindow
{
  event EventHandler Closed;

  void Show();

  void Close();

  void ApplySettings(SidebarAppearance appearance);
}

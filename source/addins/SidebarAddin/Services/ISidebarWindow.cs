namespace Xeno.ToolsHub.SidebarAddin.Services;

using System;

public interface ISidebarWindow
{
  event EventHandler Closed;

  void Show();

  void Close();
}

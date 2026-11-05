namespace Xeno.Dockzi.SidebarAddin.Services;

using System;
using Xeno.Dockzi.SidebarAddin.Domain;

public interface ISidebarWindow
{
  event EventHandler Closed;

  void ApplySettings(SidebarAppearance appearance);

  void Close();

  void Show();
}

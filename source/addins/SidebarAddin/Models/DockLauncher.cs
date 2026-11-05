namespace Xeno.Dockzi.SidebarAddin.Models;

using System;

/// <summary>A sidebar-owned shell target. Windows links retain their own launch options.</summary>
public sealed class DockLauncher
{
  public string Id { get; set; } = Guid.NewGuid().ToString("N");

  public string Target { get; set; }

  public string Title { get; set; }
}

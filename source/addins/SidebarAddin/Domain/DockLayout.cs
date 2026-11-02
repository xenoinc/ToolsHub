namespace Xeno.ToolsHub.SidebarAddin.Domain;

using System;
using System.Windows;

public static class DockLayout
{
  /// <summary>Centers a dock on a monitor edge, bounded by its working area (in pixels).</summary>
  public static Rect Calculate(Rect workArea, int screenArea, double width, double height)
  {
    width = Math.Min(Math.Max(1, width), workArea.Width);
    height = Math.Min(Math.Max(1, height), workArea.Height);
    double left = workArea.Left + ((workArea.Width - width) / 2);
    double top = workArea.Top + ((workArea.Height - height) / 2);
    switch (screenArea)
    {
      case 2:
        left = workArea.Right - width;
        break;
      case 3:
        top = workArea.Top;
        break;
      case 4:
        top = workArea.Bottom - height;
        break;
      default:
        left = workArea.Left;
        break;
    }

    return new Rect(left, top, width, height);
  }
}

namespace Xeno.ToolsHub.SidebarAddin.Domain;

using System;
using System.Windows;

/// <summary>Animation state and clipping geometry; the native window never leaves its monitor.</summary>
public sealed class DockAutoHide
{
  private double _idleSeconds;

  public double Progress { get; private set; }

  public static Vector Offset(Size size, int edge, double progress)
  {
    progress = Math.Clamp(progress, 0, 1);
    return edge switch
    {
      2 => new Vector(Math.Max(0, size.Width - 3) * progress, 0),
      3 => new Vector(0, -Math.Max(0, size.Height - 3) * progress),
      4 => new Vector(0, Math.Max(0, size.Height - 3) * progress),
      _ => new Vector(-Math.Max(0, size.Width - 3) * progress, 0),
    };
  }

  public static Rect VisibleArea(Size size, int edge, double progress)
  {
    Vector offset = Offset(size, edge, progress);
    var visible = new Rect(offset.X, offset.Y, size.Width, size.Height);
    visible.Intersect(new Rect(size));
    return visible;
  }

  public void Reveal()
  {
    Progress = 0;
    _idleSeconds = 0;
  }

  public void Tick(double elapsedSeconds, bool enabled, bool interacting)
  {
    elapsedSeconds = Math.Clamp(elapsedSeconds, 0, 0.1);
    _idleSeconds = interacting || !enabled ? 0 : _idleSeconds + elapsedSeconds;
    double target = enabled && !interacting && _idleSeconds >= 0.65 ? 1 : 0;
    double step = elapsedSeconds / 0.22;
    Progress = target > Progress ? Math.Min(target, Progress + step) : Math.Max(target, Progress - step);
  }
}

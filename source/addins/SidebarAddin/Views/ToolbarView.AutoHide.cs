namespace Xeno.ToolsHub.SidebarAddin.Views;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Xeno.ToolsHub.SidebarAddin.Domain;

public partial class ToolbarView
{
  private readonly DockAutoHide _autoHide = new();
  private readonly Stopwatch _clock = Stopwatch.StartNew();
  private readonly DispatcherTimer _hideTimer = new(DispatcherPriority.Input) { Interval = TimeSpan.FromMilliseconds(16) };
  private readonly DispatcherTimer _runningTimer = new(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(1) };
  private readonly TranslateTransform _slide = new();
  private Rect _nativeBounds = Rect.Empty;
  private Rect _lastRegion = Rect.Empty;
  private double _lastTick;
  private double _lastDropAt = -10;
  private bool _contextMenuOpen;
  private bool _modalOpen;

  public double HiddenProgress => _autoHide.Progress;

  public void ApplySettings(SidebarAppearance appearance)
  {
    _model.ApplyAppearance(appearance);
    ConfigureOrientation();
    PositionDock();
  }

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetCursorPos(out NativePoint point);

  [DllImport("gdi32.dll")]
  private static extern IntPtr CreateRectRgn(int left, int top, int right, int bottom);

  [DllImport("user32.dll")]
  private static extern int SetWindowRgn(IntPtr window, IntPtr region, [MarshalAs(UnmanagedType.Bool)] bool redraw);

  [DllImport("gdi32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool DeleteObject(IntPtr value);

  private void InitializeDockBehavior()
  {
    DockSurface.RenderTransform = _slide;
    _hideTimer.Tick += OnHideTick;
    _runningTimer.Tick += OnRunningTick;
    AddHandler(ContextMenuOpeningEvent, new System.Windows.Controls.ContextMenuEventHandler((_, _) => _contextMenuOpen = true));
    AddHandler(ContextMenuClosingEvent, new System.Windows.Controls.ContextMenuEventHandler((_, _) => _contextMenuOpen = false));
    Loaded += (_, _) =>
    {
      _lastTick = _clock.Elapsed.TotalSeconds;
      _model.RefreshRunningState();
      _hideTimer.Start();
      _runningTimer.Start();
    };
  }

  private void OnRunningTick(object sender, EventArgs args) => _model.RefreshRunningState();

  private void OnHideTick(object sender, EventArgs args)
  {
    double now = _clock.Elapsed.TotalSeconds;
    double elapsed = now - _lastTick;
    _lastTick = now;
    if (_closed || _nativeBounds.IsEmpty || ActualWidth <= 0 || ActualHeight <= 0)
      return;
    var size = new Size(ActualWidth, ActualHeight);
    var visible = DockAutoHide.VisibleArea(size, _model.ScreenArea, _autoHide.Progress);
    bool hovered = false;
    if (GetCursorPos(out var cursor))
    {
      var point = new Point((cursor.X - _nativeBounds.X) * ActualWidth / _nativeBounds.Width,
        (cursor.Y - _nativeBounds.Y) * ActualHeight / _nativeBounds.Height);
      hovered = visible.Contains(point);
    }

    bool interacting = hovered || _activeDragItem != null || _contextMenuOpen || _modalOpen || now - _lastDropAt < 0.2;
    _autoHide.Tick(elapsed, _model.AutoHide, interacting);
    UpdateSlide();
  }

  private void UpdateSlide()
  {
    if (ActualWidth <= 0 || ActualHeight <= 0 || _nativeBounds.IsEmpty)
      return;
    var size = new Size(ActualWidth, ActualHeight);
    Vector offset = DockAutoHide.Offset(size, _model.ScreenArea, _autoHide.Progress);
    _slide.X = offset.X;
    _slide.Y = offset.Y;
    EdgeGrip.Visibility = _model.AutoHide && _autoHide.Progress > 0.98 ? Visibility.Visible : Visibility.Collapsed;
    bool horizontal = _model.Orientation == System.Windows.Controls.Orientation.Horizontal;
    EdgeGrip.Width = horizontal ? double.NaN : 3;
    EdgeGrip.Height = horizontal ? 3 : double.NaN;
    EdgeGrip.HorizontalAlignment = horizontal ? HorizontalAlignment.Stretch
      : _model.ScreenArea == 2 ? HorizontalAlignment.Right : HorizontalAlignment.Left;
    EdgeGrip.VerticalAlignment = !horizontal ? VerticalAlignment.Stretch
      : _model.ScreenArea == 4 ? VerticalAlignment.Bottom : VerticalAlignment.Top;
    var visible = DockAutoHide.VisibleArea(size, _model.ScreenArea, _autoHide.Progress);
    double scaleX = _nativeBounds.Width / ActualWidth;
    double scaleY = _nativeBounds.Height / ActualHeight;
    var region = new Rect(Math.Floor(visible.X * scaleX), Math.Floor(visible.Y * scaleY),
      Math.Ceiling(visible.Right * scaleX) - Math.Floor(visible.X * scaleX),
      Math.Ceiling(visible.Bottom * scaleY) - Math.Floor(visible.Y * scaleY));
    if (region == _lastRegion)
      return;
    IntPtr nativeRegion = CreateRectRgn((int)region.Left, (int)region.Top, (int)region.Right, (int)region.Bottom);
    if (nativeRegion == IntPtr.Zero)
      return;
    if (SetWindowRgn(new System.Windows.Interop.WindowInteropHelper(this).Handle, nativeRegion, true) == 0)
      DeleteObject(nativeRegion);
    else
      _lastRegion = region;
  }

  private void StopDockBehavior()
  {
    _hideTimer.Stop();
    _runningTimer.Stop();
    _hideTimer.Tick -= OnHideTick;
    _runningTimer.Tick -= OnRunningTick;
  }

  [StructLayout(LayoutKind.Sequential)]
  private struct NativePoint
  {
    public int X;

    public int Y;
  }
}

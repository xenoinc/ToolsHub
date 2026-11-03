namespace Xeno.ToolsHub.SidebarAddin.Views;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Win32;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.ViewModels;
using Screen = System.Windows.Forms.Screen;

public partial class ToolbarView : Window
{
  private readonly ToolbarViewModel _model;
  private readonly LauncherDropHandler _drops;
  private Point _dragStart;
  private LauncherIcon _dragItem;
  private LauncherIcon _activeDragItem;
  private bool _positioning;
  private bool _closed;

  public ToolbarView(ToolbarViewModel model)
  {
    _model = model;
    _drops = new LauncherDropHandler(model);
    InitializeComponent();
    DataContext = model;
    ConfigureOrientation();
    InitializeDockBehavior();

    SourceInitialized += (_, _) => PositionDock();
    Loaded += (_, _) => PositionDock();
    DragLeave += (_, args) =>
    {
      if (!new Rect(RenderSize).Contains(args.GetPosition(this)))
        ClearDragFeedback();
    };
    DpiChanged += (_, _) => QueuePosition();
    Closed += OnClosed;
    ((INotifyCollectionChanged)_model.Items).CollectionChanged += OnItemsChanged;
    _model.PropertyChanged += OnModelChanged;
    SystemEvents.DisplaySettingsChanged += OnDisplayChanged;
  }

  public void PositionDock()
  {
    if (_closed || _positioning)
      return;
    IntPtr handle = new WindowInteropHelper(this).Handle;
    if (handle == IntPtr.Zero)
      return;
    _positioning = true;
    try
    {
      var screens = Screen.AllScreens.OrderByDescending(screen => screen.Primary).ToArray();
      var screen = screens[Math.Clamp(_model.ScreenNumber - 1, 0, screens.Length - 1)];
      var area = screen.WorkingArea;
      var dpi = VisualTreeHelper.GetDpi(this);
      bool horizontal = _model.Orientation == Orientation.Horizontal;
      double length = Math.Max(180, (_model.Items.Count * 64) + 16);
      double width = Math.Min(area.Width / dpi.DpiScaleX, horizontal ? length : 80);
      double height = Math.Min(area.Height / dpi.DpiScaleY, horizontal ? 80 : length);
      if (!string.IsNullOrEmpty(_model.Error))
      {
        width = Math.Min(area.Width / dpi.DpiScaleX, Math.Max(width, 200));
        height = Math.Min(area.Height / dpi.DpiScaleY, height + 70);
      }

      Width = width;
      Height = height;
      var bounds = DockLayout.Calculate(new Rect(area.X, area.Y, area.Width, area.Height),
        _model.ScreenArea, width * dpi.DpiScaleX, height * dpi.DpiScaleY);

      // Native placement keeps negative coordinates and per-monitor DPI in physical pixels.
      SetWindowPos(handle, IntPtr.Zero, (int)Math.Round(bounds.X), (int)Math.Round(bounds.Y),
        (int)Math.Round(bounds.Width), (int)Math.Round(bounds.Height), 0x14);
      _nativeBounds = bounds;
      _autoHide.Reveal();
      _lastRegion = Rect.Empty;
      UpdateSlide();
    }
    finally
    {
      _positioning = false;
    }
  }

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool SetWindowPos(IntPtr window, IntPtr insertAfter, int x, int y, int width, int height, uint flags);

  private void ConfigureOrientation()
  {
    bool horizontal = _model.Orientation == Orientation.Horizontal;
    LauncherScroll.HorizontalScrollBarVisibility = horizontal ? ScrollBarVisibility.Hidden : ScrollBarVisibility.Disabled;
    LauncherScroll.VerticalScrollBarVisibility = horizontal ? ScrollBarVisibility.Disabled : ScrollBarVisibility.Hidden;
  }

  private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs args) => QueuePosition();

  private void OnModelChanged(object sender, PropertyChangedEventArgs args) => QueuePosition();

  private void OnDisplayChanged(object sender, EventArgs args) => QueuePosition();

  private void QueuePosition()
  {
    if (!_closed && !Dispatcher.HasShutdownStarted)
      Dispatcher.BeginInvoke(new Action(PositionDock));
  }

  private void OnClosed(object sender, EventArgs args)
  {
    _closed = true;
    StopDockBehavior();
    ClearDragFeedback();
    SystemEvents.DisplaySettingsChanged -= OnDisplayChanged;
    ((INotifyCollectionChanged)_model.Items).CollectionChanged -= OnItemsChanged;
    _model.PropertyChanged -= OnModelChanged;
  }

  private void OnMouseWheel(object sender, MouseWheelEventArgs args)
  {
    if (_model.Orientation == Orientation.Horizontal)
      LauncherScroll.ScrollToHorizontalOffset(LauncherScroll.HorizontalOffset - args.Delta);
    else
      LauncherScroll.ScrollToVerticalOffset(LauncherScroll.VerticalOffset - args.Delta);
    args.Handled = true;
  }

  private void OnLaunch(object sender, RoutedEventArgs args)
  {
    if (((FrameworkElement)sender).DataContext is LauncherIcon item)
      _model.Launch(item);
  }

  private void OnAddLauncher(object sender, RoutedEventArgs args)
  {
    var dialog = new Microsoft.Win32.OpenFileDialog
    {
      Title = "Add sidebar launchers",
      Filter = "Applications and shortcuts|*.exe;*.lnk;*.url;*.appref-ms|All files|*.*",
      Multiselect = true,
      DereferenceLinks = false,
    };
    _modalOpen = true;
    try
    {
      if (dialog.ShowDialog(this) == true)
        _model.AddPaths(dialog.FileNames, _model.Items.Count);
    }
    finally
    {
      _modalOpen = false;
      _autoHide.Reveal();
      UpdateSlide();
    }
  }

  private void OnRemoveLauncher(object sender, RoutedEventArgs args)
  {
    if (((FrameworkElement)sender).DataContext is LauncherIcon item)
      _model.Remove(item);
  }

  private void OnItemMouseDown(object sender, MouseButtonEventArgs args)
  {
    _dragStart = args.GetPosition(this);
    _dragItem = ((FrameworkElement)sender).DataContext as LauncherIcon;
  }

  private void OnItemMouseMove(object sender, MouseEventArgs args)
  {
    if (args.LeftButton != MouseButtonState.Pressed || _dragItem == null)
      return;
    Point current = args.GetPosition(this);
    if (Math.Abs(current.X - _dragStart.X) < SystemParameters.MinimumHorizontalDragDistance
      && Math.Abs(current.Y - _dragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
      return;
    var item = _dragItem;
    _dragItem = null;

    // End Button's click capture before beginning a drag so dropping never launches the app.
    ((Button)sender).ReleaseMouseCapture();
    var button = (Button)sender;
    _activeDragItem = item;
    button.Opacity = 0.3;
    button.GiveFeedback += OnGiveDragFeedback;
    try
    {
      DragDrop.DoDragDrop(button, new DataObject(LauncherDropHandler.ItemDragFormat, item), DragDropEffects.Move);
    }
    finally
    {
      button.GiveFeedback -= OnGiveDragFeedback;
      button.Opacity = 1;
      _activeDragItem = null;
      ClearDragFeedback();
    }

    args.Handled = true;
  }

  private void OnDragOver(object sender, DragEventArgs args)
  {
    _lastDropAt = _clock.Elapsed.TotalSeconds;
    args.Effects = _drops.GetEffect(args.Data, args.AllowedEffects);
    DockSurface.BorderBrush = args.Effects == DragDropEffects.None ? new SolidColorBrush(Color.FromRgb(126, 139, 160)) : Brushes.LightSkyBlue;
    AutoScroll(args.GetPosition(LauncherScroll));
    if (args.Effects != DragDropEffects.None)
      UpdateDragFeedback(args.GetPosition(DragOverlay), FindInsertion(args.GetPosition(Launchers)));
    else
      ClearDragFeedback();
    args.Handled = true;
  }

  private void OnDrop(object sender, DragEventArgs args)
  {
    int insertion = FindInsertion(args.GetPosition(Launchers));
    args.Effects = _drops.GetEffect(args.Data, args.AllowedEffects);
    if (args.Effects == DragDropEffects.None || !_drops.Drop(args.Data, insertion))
      args.Effects = DragDropEffects.None;
    ClearDragFeedback();
    args.Handled = true;
  }

  private void OnGiveDragFeedback(object sender, GiveFeedbackEventArgs args)
  {
    args.UseDefaultCursors = args.Effects == DragDropEffects.None;
    if (!args.UseDefaultCursors)
      Mouse.SetCursor(Cursors.Hand);
    args.Handled = true;
  }

  private void UpdateDragFeedback(Point pointer, int insertionIndex)
  {
    if (_activeDragItem != null)
    {
      DraggedIcon.Source = _activeDragItem.Image;
      DragPreview.ToolTip = _activeDragItem.Title;
      DragPreview.Visibility = Visibility.Visible;
      Canvas.SetLeft(DragPreview, Math.Clamp(pointer.X - 28, 0, Math.Max(0, DragOverlay.ActualWidth - 56)));
      Canvas.SetTop(DragPreview, Math.Clamp(pointer.Y - 28, 0, Math.Max(0, DragOverlay.ActualHeight - 56)));
    }

    if (_model.Items.Count == 0)
    {
      InsertionMarker.Visibility = Visibility.Collapsed;
      return;
    }

    bool horizontal = _model.Orientation == Orientation.Horizontal;
    int index = Math.Min(insertionIndex, _model.Items.Count - 1);
    if (Launchers.ItemContainerGenerator.ContainerFromIndex(index) is not FrameworkElement container)
      return;
    Point origin = container.TranslatePoint(new Point(), DragOverlay);
    bool afterLast = insertionIndex == _model.Items.Count;
    InsertionMarker.Width = horizontal ? 3 : 48;
    InsertionMarker.Height = horizontal ? 48 : 3;
    Canvas.SetLeft(InsertionMarker, horizontal
      ? origin.X + (afterLast ? container.ActualWidth : 0) - 1.5
      : origin.X + ((container.ActualWidth - 48) / 2));
    Canvas.SetTop(InsertionMarker, horizontal
      ? origin.Y + ((container.ActualHeight - 48) / 2)
      : origin.Y + (afterLast ? container.ActualHeight : 0) - 1.5);
    InsertionMarker.Visibility = Visibility.Visible;
  }

  private void ClearDragFeedback()
  {
    DragPreview.Visibility = Visibility.Collapsed;
    DraggedIcon.Source = null;
    InsertionMarker.Visibility = Visibility.Collapsed;
    DockSurface.BorderBrush = new SolidColorBrush(Color.FromRgb(126, 139, 160));
  }

  private int FindInsertion(Point position)
  {
    bool horizontal = _model.Orientation == Orientation.Horizontal;
    for (int index = 0; index < _model.Items.Count; index++)
    {
      if (Launchers.ItemContainerGenerator.ContainerFromIndex(index) is not FrameworkElement container)
        continue;
      Point origin = container.TranslatePoint(new Point(), Launchers);
      if (horizontal ? position.X < origin.X + (container.ActualWidth / 2) : position.Y < origin.Y + (container.ActualHeight / 2))
        return index;
    }

    return _model.Items.Count;
  }

  private void AutoScroll(Point position)
  {
    if (_model.Orientation == Orientation.Horizontal)
    {
      if (position.X < 24)
        LauncherScroll.LineLeft();
      else if (position.X > LauncherScroll.ActualWidth - 24)
        LauncherScroll.LineRight();
    }
    else
    {
      if (position.Y < 24)
        LauncherScroll.LineUp();
      else if (position.Y > LauncherScroll.ActualHeight - 24)
        LauncherScroll.LineDown();
    }
  }
}

namespace Xeno.ToolsHub.SidebarAddin.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Models;
using Xeno.ToolsHub.SidebarAddin.Services;

public sealed class ToolbarViewModel : INotifyPropertyChanged
{
  private readonly ILauncherStore _store;
  private readonly ILauncherShell _shell;
  private readonly IRunningApplications _running;
  private readonly ObservableCollection<LauncherIcon> _items = new();
  private readonly bool _loadFailed;
  private SidebarAppearance _appearance;
  private string _error;

  public ToolbarViewModel(ILauncherStore store, ILauncherShell shell, ISettingsService settings, IRunningApplications running = null)
  {
    _store = store;
    _shell = shell;
    _appearance = SidebarAppearance.Load(settings);
    _running = running ?? new WindowsRunningApplications();
    Items = new ReadOnlyObservableCollection<LauncherIcon>(_items);
    try
    {
      foreach (var item in _store.Load())
        _items.Add(CreateIcon(item));
    }
    catch (Exception ex)
    {
      _loadFailed = true;
      ReportError("Unable to load sidebar launchers. Repair SidebarLaunchers.json before adding items.", ex);
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public ReadOnlyObservableCollection<LauncherIcon> Items { get; }

  public int ScreenArea => _appearance.ScreenArea;

  public int ScreenNumber => _appearance.ScreenNumber;

  public Orientation Orientation => ScreenArea == 3 || ScreenArea == 4 ? Orientation.Horizontal : Orientation.Vertical;

  public double Opacity => Math.Clamp(_appearance.Opacity, 20, 100) / 100.0;

  public bool OnTop => _appearance.OnTop;

  public bool AutoHide => _appearance.AutoHide;

  public string Error
  {
    get => _error;
    private set
    {
      _error = value;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
    }
  }

  public void ApplyAppearance(SidebarAppearance appearance)
  {
    _appearance = appearance;
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
  }

  public void RefreshRunningState()
  {
    try
    {
      var open = _running.GetOpenExecutables();
      foreach (var icon in _items)
      {
        string target = _running.ResolveExecutable(icon.Item.Target);
        icon.IsRunning = target != null && open.Contains(target);
      }
    }
    catch (Exception ex)
    {
      Log.Debug($"Unable to refresh sidebar running indicators: {ex.Message}");
    }
  }

  public bool AddPaths(IEnumerable<string> paths, int insertionIndex)
  {
    if (_loadFailed)
      return false;
    try
    {
      var next = _items.ToList();
      int index = Math.Clamp(insertionIndex, 0, next.Count);
      foreach (string path in paths)
      {
        var item = _shell.Import(path);
        if (next.Any(existing => string.Equals(existing.Item.Target, item.Target, StringComparison.OrdinalIgnoreCase)))
          continue;
        next.Insert(index++, CreateIcon(item));
      }

      return Commit(next);
    }
    catch (Exception ex)
    {
      ReportError("Unable to add the dropped items.", ex);
      return false;
    }
  }

  /// <summary>Moves an item to an insertion boundary in the current list.</summary>
  public bool Move(LauncherIcon item, int insertionIndex)
  {
    int oldIndex = _items.IndexOf(item);
    if (oldIndex < 0 || _loadFailed)
      return false;
    int index = Math.Clamp(insertionIndex, 0, _items.Count);
    if (oldIndex < index)
      index--;
    var next = _items.ToList();
    next.RemoveAt(oldIndex);
    next.Insert(index, item);
    return Commit(next);
  }

  public bool Remove(LauncherIcon item)
  {
    if (!_items.Contains(item) || _loadFailed)
      return false;
    var next = _items.ToList();
    next.Remove(item);
    return Commit(next);
  }

  public void Launch(LauncherIcon item)
  {
    if (!_items.Contains(item))
      return;
    try
    {
      _shell.Launch(item.Item);
      Error = null;
    }
    catch (Exception ex)
    {
      ReportError($"Unable to launch {item.Title}. Check that its target still exists.", ex);
    }
  }

  private LauncherIcon CreateIcon(DockLauncher item) => new(item, _shell.GetIcon(item.Target));

  private bool Commit(List<LauncherIcon> next)
  {
    try
    {
      _store.Save(next.Select(icon => icon.Item).ToList());
      _items.Clear();
      foreach (var icon in next)
        _items.Add(icon);
      Error = null;
      return true;
    }
    catch (Exception ex)
    {
      ReportError("Unable to save sidebar launchers. Your previous list has been kept.", ex);
      return false;
    }
  }

  private void ReportError(string message, Exception exception)
  {
    Error = message;
    Log.Error($"{message} {exception.Message}");
  }
}

/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-21
 * Author:  Damian Suess
 * File:    ToolbarViewModel.cs
 * Description:
 *  Sidebar Toolbar View Model
 */

using System.Windows.Controls;
using System.Windows.Input;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Models;

namespace Xeno.ToolsHub.SidebarAddin.ViewModels
{
  public class ToolbarViewModel
  {
    // Lets start with one
    private readonly DockItem _linkIcon;

    private StackPanel _stackPanel;

    // private readonly ObservableCollection<LinkIcon> _linkIcons;
    private ICommand _cmdClickTest;

    private ICommand _cmdAddItem;

    public ToolbarViewModel(StackPanel stackPanel)
    {
      _stackPanel = stackPanel;

      _linkIcon = new DockItem("Test Shortcut", "image.png", () =>
      {
        Log.Debug("Toolbar shortcut clicked");
      });

      InitListItems();
    }

    public ToolbarViewModel()
    {
      _linkIcon = new DockItem("Test Shortcut", "image.png", () =>
      {
        Log.Debug("Toolbar shortcut clicked");
      });

      InitListItems();
    }

    public bool IsTransparent { get; set; } = true;

    public double Transparency { get; set; } = 0.9;

    public ICommand CmdAddItem
    {
      get => _cmdAddItem ?? (_cmdAddItem = new DelegateCommand(OnAddItem));
    }

    public ICommand CmdClickTest
    {
      get => (_cmdClickTest ??
        (_cmdClickTest = new DelegateCommand((_) => _linkIcon.OnClick())));
    }

    private void OnAddItem(object obj)
    {
      Label lblDynamic = new Label
      {
        Content = "Hello",
      };

      _stackPanel.Children.Add(lblDynamic);
    }

    /// <summary>Load the dock items to display</summary>
    private void InitListItems()
    {
    }
  }
}

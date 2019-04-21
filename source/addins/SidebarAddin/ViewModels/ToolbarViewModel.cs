/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-21
 * Author:  Damian Suess
 * File:    ToolbarViewModel.cs
 * Description:
 *  Sidebar Toolbar View Model
 */

using System.Windows.Input;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.SidebarAddin.Domain;
using Xeno.ToolsHub.SidebarAddin.Models;

namespace Xeno.ToolsHub.SidebarAddin.ViewModels
{
  public class ToolbarViewModel
  {
    public ToolbarViewModel()
    {
      _linkIcon = new LinkIcon("Test Shortcut", "image.png", () => {
        Log.Debug("Toolbar shortcut clicked");
      });

    }

    // private readonly ObservableCollection<LinkIcon> _linkIcons;

    // Lets start with one
    private readonly LinkIcon _linkIcon;

    private ICommand _clickCommand;

    public ICommand ClickCommand
    {
      get => (_clickCommand ??
        (_clickCommand = new DelegateCommand((_) => _linkIcon.OnClick())));
    }
  }
}

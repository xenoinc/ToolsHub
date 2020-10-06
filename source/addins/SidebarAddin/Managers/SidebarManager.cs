/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    SidebarManager.cs
 * Description:
 *  Sidebar manager
 */

using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.Services.Messaging;
using Xeno.ToolsHub.SidebarAddin.ViewModels;

namespace Xeno.ToolsHub.SidebarAddin.Managers
{
  public class SidebarManager
  {
    public string SettingBackground => SettingsService.GetString(Constants.AddinId, Constants.KeyBackground, string.Empty);

    public int SettingOpacity => SettingsService.GetInt(Constants.AddinId, Constants.KeyOpacity, 0);

    public int SettingScreenArea => SettingsService.GetInt(Constants.AddinId, Constants.KeyScreenArea, 1);

    public int SettingScreenOrientation => SettingsService.GetInt(Constants.AddinId, Constants.KeyScreenOrientation, 0);

    public int SettingScreenNumber => SettingsService.GetInt(Constants.AddinId, Constants.KeyScreenNumber, 1);

    public bool SettingsShowSidebar => SettingsService.GetBool(Constants.AddinId, Constants.KeyShowSidebars, true);

    public string SettingTintColor => SettingsService.GetString(Constants.AddinId, Constants.KeyTintColor, string.Empty);

    public ShortcutItems GetShortcuts()
    {
      // Use Shortcut object, not string
      Log.Warn("Feature not implemented yet.");

      ShortcutsManager mngr = new ShortcutsManager();
      var items = mngr.ShortcutItems;

      return items;
    }

    public void ShowSidebars()
    {
      // TODO: In the future, iterate through sidebars we've created and display them

      if (SettingsShowSidebar)
      {
        //// var viewModel = new ToolbarViewModel();
        var form = new Views.ToolbarView()
        {
          //// form.DataContext = new ToolbarViewModel(),
          //// Owner = System.Windows.Application.Current.MainWindow,
          ShowInTaskbar = true
        };

        form.DataContext = new ToolbarViewModel(form.StackDynamic);

        form.Show();
      }
    }

    /// <summary>If we modified shortcuts, tell the rest of the system to update</summary>
    public void SendMessageRefreshShortcuts()
    {
      MessagingCenter.Send<ShortcutsMessage>(new ShortcutsMessage(), ShortcutsMessage.Refresh);
    }
  }
}

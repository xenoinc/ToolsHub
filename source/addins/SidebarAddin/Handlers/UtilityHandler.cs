/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    UtilityHandler.cs
 * Description:
 *  Sidebar utility handler
 */

using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.SidebarAddin.Managers;

namespace Xeno.ToolsHub.SidebarAddin.Handlers
{
  [Mono.Addins.Extension(
    NodeName = ExtensionName.UtilityAddin,
    Path = ExtensionPath.Utility)]
  public class UtilityHandler : UtilityAddin
  {
    private bool _initialized = false;
    private SidebarManager _manager = new SidebarManager();

    public UtilityHandler()
    {
      Log.Debug("Loading sidebar utility");

      // TODO: Validate SidebarManager settings
      // [ ] Check screen assignment & if it exists
      // [ ] Check if background image exists; else, use default
      // [ ] Disable icons with dead shortcuts
      // [ ] Check if ShortcutItem icons are dead; else, use default

      _initialized = true;
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("Sidebar utility add-in executed!");

      if (_manager.SettingsShowSidebar)
        _manager.ShowSidebars();
    }

    public override void Shutdown()
    {
      Log.Debug("Sidebar utility add-in shutting down");
    }
  }
}

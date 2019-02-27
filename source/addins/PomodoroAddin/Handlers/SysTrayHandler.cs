/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    SysTrayHandler.cs
 * Description:
 *  System Tray extension point for Pomodoro add-in
 */

namespace PomodoroAddin.Handlers
{
  using System.Collections.Generic;
  using System.Windows.Forms;
  using PomodoroAddin.Managers;
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.ExtensionModel.SystemTray;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.SysTrayAddin,
    Path = ExtensionPath.SystemTray)]
  public class SysTrayHandler : Xeno.ToolsHub.ExtensionModel.SystemTray.SysTrayAddin
  {
    private PomodoroManager _pomodoro;

    public SysTrayHandler()
    {
      _pomodoro = new PomodoroManager();
      this.IsInitialized = true;
    }

    public override bool IsInitialized { get; }

    public override List<MenuItem> MenuItems()
    {
      MenuItem menu = new MenuItem("🍅 Pomodoro (alpha)");
      menu.MenuItems.Add(0, new TrayItem($"Start ({_pomodoro.SettingTimerDuration} min)", string.Empty, true, _pomodoro.OnStart));
      menu.MenuItems.Add(1, new TrayItem($"Take short break ({_pomodoro.SettingTimerShortBreak} min)", string.Empty, true, _pomodoro.OnBreakShort));
      menu.MenuItems.Add(2, new TrayItem($"Take long break ({_pomodoro.SettingTimerLongBreak} min)", string.Empty, true, _pomodoro.OnBreakLong));
      menu.MenuItems.Add(3, new TrayItem($"-", string.Empty));
      menu.MenuItems.Add(4, new TrayItem($"Pause", string.Empty, true, _pomodoro.OnPause));
      menu.MenuItems.Add(5, new TrayItem($"Stop", string.Empty, true, _pomodoro.OnStop));

      // Proposed method for creating menu items
      ////menu.MenuItems.Add(0, new TrayItem(new TrayItemInfo("P1", "Start", $"Start ({_pomodoro.SettingTimerDuration} min)", string.Empty, true), _pomodoro.OnStart));
      ////menu.MenuItems.Add(1, new TrayItem(new TrayItemInfo("P1", "Start", $"Take short break ({_pomodoro.SettingTimerShortBreak} min)", string.Empty, true), _pomodoro.OnBreakShort));
      ////menu.MenuItems.Add(2, new TrayItem(new TrayItemInfo("P1", "Start", $"Take long break ({_pomodoro.SettingTimerLongBreak} min)", string.Empty, true), _pomodoro.OnBreakLong));
      ////menu.MenuItems.Add(3, new TrayItem($"-", string.Empty));
      ////menu.MenuItems.Add(4, new TrayItem(new TrayItemInfo("P1", "Start", $"Pause", string.Empty, true), _pomodoro.OnPause));
      ////menu.MenuItems.Add(5, new TrayItem(new TrayItemInfo("P1", "Start", $"Stop", string.Empty, true), _pomodoro.OnStop));

      return new List<MenuItem>() { menu };
    }
  }
}

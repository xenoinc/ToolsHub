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
      MenuItem menu = new MenuItem("Pomodoro");
      menu.MenuItems.Add(0, new TrayItem($"Start ({_pomodoro.TimerDuration} min)", string.Empty, true, _pomodoro.OnStart));
      menu.MenuItems.Add(1, new TrayItem($"Take short break ({_pomodoro.TimerShortBreak} min)", string.Empty, true, _pomodoro.OnBreakShort));
      menu.MenuItems.Add(2, new TrayItem($"Take long break ({_pomodoro.TimerLongBreak} min)", string.Empty, true, _pomodoro.OnBreakLong));
      menu.MenuItems.Add(2, new TrayItem($"-", string.Empty));
      menu.MenuItems.Add(2, new TrayItem($"Pause", string.Empty, true, _pomodoro.OnPause));
      menu.MenuItems.Add(2, new TrayItem($"Stop", string.Empty, true, _pomodoro.OnStop));

      return new List<MenuItem>() { menu };
    }
  }
}

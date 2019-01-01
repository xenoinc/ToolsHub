/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PomodoroManager.cs
 * Description:
 *  Pomodoro timer manager
 */

namespace PomodoroAddin
{
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel.SystemTray;
  using Xeno.ToolsHub.Services.Logging;

  public class PomodoroManager
  {
    public PomodoroManager()
    {
      TimerDuration = 25;
      TimerShortBreak = 5;
      TimerLongBreak = 10;
    }

    public int TimerDuration { get; set; }

    public int TimerShortBreak { get; set; }

    public int TimerLongBreak { get; set; }

    public List<MenuItem> MenuItems
    {
      get
      {
        MenuItem menu = new MenuItem("Pomodoro");

        menu.MenuItems.Add(0, new TrayItem($"Start ({TimerDuration} min)", string.Empty, true, OnStart));
        menu.MenuItems.Add(1, new TrayItem($"Take short break ({TimerShortBreak} min)", string.Empty, true, OnBreakShort));
        menu.MenuItems.Add(2, new TrayItem($"Take long break ({TimerLongBreak} min)", string.Empty, true, OnBreakLong));
        menu.MenuItems.Add(2, new TrayItem($"-", string.Empty));
        menu.MenuItems.Add(2, new TrayItem($"Pause", string.Empty, true, OnPause));
        menu.MenuItems.Add(2, new TrayItem($"Stop", string.Empty, true, OnStop));

        return new List<MenuItem>() { menu };
      }
    }

    public int OnStart(string target)
    {
      Log.Debug($"Start timer for {TimerDuration} minutes");
      return 0;
    }

    public int OnBreakShort(string target)
    {
      Log.Debug($"Break timer for {TimerShortBreak} minutes");
      return 0;
    }

    public int OnBreakLong(string target)
    {
      Log.Debug($"Break timer for {TimerLongBreak} minutes");
      return 0;
    }

    public int OnPause(string target)
    {
      Log.Debug($"Stop timer");
      return 0;
    }

    public int OnStop(string target)
    {
      Log.Debug($"Stop timer");
      return 0;
    }
  }
}

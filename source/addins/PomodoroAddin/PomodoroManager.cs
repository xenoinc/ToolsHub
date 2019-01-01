/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PomodoroManager.cs
 * Description:
 *  Pomodoro timer manager
 */

namespace PomodoroAddin
{
  using Xeno.ToolsHub.Services;
  using Xeno.ToolsHub.Services.Logging;

  public enum TimerState
  {
    /// <summary>Timer started</summary>
    Start,

    /// <summary>Timer paused</summary>
    Pause,

    /// <summary>Timer stopped</summary>
    Stop,

    /// <summary>Timer finished</summary>
    Done
  };

  public class PomodoroManager
  {
    private System.Timers.Timer _timer;
    private bool _running;

    public PomodoroManager()
    {
      _timer = new System.Timers.Timer();

      TimerDuration = SettingsService.GetInt(Constants.AddinId, Constants.KeyDuration, 25);
      TimerShortBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyShortBreak, 5);
      TimerLongBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyLongBreak, 10);
    }

    public int TimerDuration { get; set; }

    public int TimerShortBreak { get; set; }

    public int TimerLongBreak { get; set; }

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

    private void StartTimer(int minutes)
    {
      _timer.Enabled = false;
      _timer.Stop();
      _timer.Interval = 1000;
      _timer.Elapsed += Timer_Elapsed;
      _timer.Start();
      _timer.Enabled = true;
      
      _running = true;

    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      throw new System.NotImplementedException();
    }
  }
}

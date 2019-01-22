/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PomodoroManager.cs
 * Description:
 *  Pomodoro timer manager
 */

namespace PomodoroAddin.Managers
{
  using Xeno.ToolsHub.ExtensionModel.SystemTray;
  using Xeno.ToolsHub.Services;
  using Xeno.ToolsHub.Services.Logging;
  using Xeno.ToolsHub.Services.Messaging;

  public class PomodoroManager
  {
    private System.Timers.Timer _timer;
    private bool _isRunning;

    public PomodoroManager()
    {
      _timer = new System.Timers.Timer();

      TimerDuration = SettingsService.GetInt(Constants.AddinId, Constants.KeyDuration, 25);
      TimerShortBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyShortBreak, 5);
      TimerLongBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyLongBreak, 10);
    }

    public bool DoesFlashScreenEvents { get; set; }

    public bool DoesSysTrayBubbles { get; set; }

    public bool DoesSysTrayIconUpdates { get; set; }

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

    /// <summary>Send message to SysTray icon</summary>
    /// <param name="icon">icon to display. NULL for default</param>
    public void SendMessage(System.Drawing.Icon icon)
    {
      // We MUST specify the <..>, otherwise it will fail if you pass a NULL icon
      MessagingCenter.Send<SystemTrayMessages, System.Drawing.Icon>(
        new SystemTrayMessages(), SystemTrayMessages.CustomIcon, icon);
    }

    private void StartTimer(int minutes)
    {
      _timer.Enabled = false;
      _timer.Stop();
      _timer.Interval = 1000;
      _timer.Elapsed += Timer_Elapsed;
      _timer.Start();
      _timer.Enabled = true;

      _isRunning = true;
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      throw new System.NotImplementedException();
    }
  }
}

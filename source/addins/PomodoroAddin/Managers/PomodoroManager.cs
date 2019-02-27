/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PomodoroManager.cs
 * Description:
 *  Pomodoro timer manager
 */

using System;
using PomodoroAddin.Domain;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.Services.Messaging;

namespace PomodoroAddin.Managers
{
  public class PomodoroManager
  {
    private TimerState _currentState = TimerState.Done;
    private System.Timers.Timer _timer;
    private int _duration = 0;

    public PomodoroManager()
    {
      _timer = new System.Timers.Timer();
      _timer.Elapsed += Timer_Elapsed;
      _timer.Interval = 60000;  // Testing only: 1000;

      SettingTimerDuration = SettingsService.GetInt(Constants.AddinId, Constants.KeyDuration, 25);
      SettingTimerShortBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyShortBreak, 5);
      SettingTimerLongBreak = SettingsService.GetInt(Constants.AddinId, Constants.KeyLongBreak, 15);
      SettingAlertSound = SettingsService.GetBool(Constants.AddinId, Constants.KeyAlertSound, true);
      SettingAlertFlash = SettingsService.GetBool(Constants.AddinId, Constants.KeyAlertFlash, true);
      SettingAlertTrayBubble = SettingsService.GetBool(Constants.AddinId, Constants.KeyAlertSysTrayBubble, true);
    }

    public int SettingTimerDuration { get; set; }

    public int SettingTimerShortBreak { get; set; }

    public int SettingTimerLongBreak { get; set; }

    public bool SettingAlertSound { get; set; }

    public bool SettingAlertFlash { get; set; }

    public bool SettingAlertTrayBubble { get; set; }

    public bool SettingTrayIconUpdates { get; set; }

    public int OnStart(string target)
    {
      Log.Debug($"Start timer for {SettingTimerDuration} minutes");
      ChangeTimerState(TimerState.Started, SettingTimerDuration);
      return 0;
    }

    public int OnBreakShort(string target)
    {
      Log.Debug($"Break timer for {SettingTimerShortBreak} minutes");
      ChangeTimerState(TimerState.Started, SettingTimerShortBreak);
      return 0;
    }

    public int OnBreakLong(string target)
    {
      Log.Debug($"Break timer for {SettingTimerLongBreak} minutes");
      ChangeTimerState(TimerState.Started, SettingTimerLongBreak);
      return 0;
    }

    public int OnPause(string target)
    {
      var state = _currentState != TimerState.Paused ?
        TimerState.Paused : TimerState.Started;

      ChangeTimerState(state);
      return 0;
    }

    public int OnStop(string target)
    {
      ChangeTimerState(TimerState.Done);
      return 0;
    }

    /// <summary>Send message to SysTray icon</summary>
    /// <param name="icon">icon to display. NULL for default</param>
    public void SendMessageTrayIcon(System.Drawing.Icon icon)
    {
      // We MUST specify the <..>, otherwise it will fail if you pass a NULL icon
      MessagingCenter.Send<SystemTrayMessages, System.Drawing.Icon>(
        new SystemTrayMessages(), SystemTrayMessages.CustomIcon, icon);
    }

    public void SendMessageTrayData(TrayItemInfo itemInfo)
    {
      Log.Warn("Method currently not in use.");

      // TODO: Create infrastructure to inform MenuItems which ones to enable/disable/change-text
      //
      MessagingCenter.Send<SystemTrayMessages, TrayItemInfo>(
        new SystemTrayMessages(), SystemTrayMessages.Update, itemInfo);
    }

    private void ChangeTimerState(TimerState state, int durationMinutes = 0)
    {
      if (_currentState == state)
        return;

      switch (state)
      {
        case TimerState.Started:

          if (_currentState == TimerState.Paused)
          {
            // resume timer
            Log.Debug($"Resuming timer for {SettingTimerDuration} minutes");
          }
          else
          {
            // new timer
            Log.Debug($"Starting timer for {SettingTimerDuration} minutes");
            StartTimer(durationMinutes);
            UpdateMenuItems(state);
          }
          break;

        case TimerState.Paused:
          Log.Debug($"Timer paused");
          break;

        case TimerState.Done:
          Log.Debug($"Timer stopped");
          _timer.Stop();
          // _running = false;
          UpdateTrayIcon();
          break;
      }

      _currentState = state;
      NotifyStateChanged(state);
    }

    /// <summary>Alert to user of state change</summary>
    /// <param name="state">State transition</param>
    private void NotifyStateChanged(TimerState state)
    {
      // TODO:
      //  [ ] Flash on screen
      //  [ ] Make sound
      //  [ ] System Tray Bubble

      Log.Debug($"Alert user we're at state: {state}!!");
      Log.Warn("Notify state change feature is currently not available!");
      // throw new NotImplementedException();
    }

    private void PlaySound(uint repeat = 1)
    {
      if (!SettingAlertSound)
        return;

      try
      {
        for (uint cnt = 1; cnt <= repeat; ++cnt)
        {
          System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
          System.IO.Stream s = a.GetManifestResourceStream("PomodoroAddin.chime.wav");
          using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(s))
          {
            player.PlaySync();
          }
        }
      }
      catch (Exception ex)
      {
        Log.Error("Something's missing! " + ex.Message);
      }
    }

    /// <summary>Override TrayIcon with duration remaining</summary>
    /// <param name="number"></param>
    private void UpdateTrayIcon(int number = 0)
    {
      if (number == 0)
      {
        this.SendMessageTrayIcon(null);
      }
      else
      {
        IconGenerator icon = new IconGenerator(number.ToString());
        this.SendMessageTrayIcon(icon.ToIcon());
      }
    }

    /// <summary>Disable menu times</summary>
    /// <param name="state">State transition</param>
    private void UpdateMenuItems(TimerState state)
    {
      Log.Debug("Not implemented yet!");

      bool start = false;
      bool breakLong = false;
      bool breakShort = false;
      bool pause = false;
      bool stop = false;

      switch (state)
      {
        case TimerState.Started:
          // Make sure pause item says, "Pause" not "Resume"
          start = false;
          breakLong = false;
          breakShort = false;
          pause = true;
          stop = true;

          SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "Pause", "Pause", string.Empty, pause));
          break;

        case TimerState.Paused:
          // Change text to "Resume"
          start = true;
          breakLong = false;
          breakShort = false;
          pause = false;
          stop = true;

          SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "Pause", $"Resume", string.Empty, pause));
          break;

        case TimerState.Done:
          start = true;
          breakLong = true;
          breakShort = true;
          pause = false;
          stop = false;

          SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "Pause", $"Pause", string.Empty, pause));
          break;
      }

      SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "Start", $"Start ({SettingTimerDuration}", string.Empty, start));
      SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "BreakShort", $"Take short break ({SettingTimerShortBreak} min)", string.Empty, breakShort));
      SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "BreakLong", $"Take long break ({SettingTimerLongBreak} min)", string.Empty, breakLong));
      // SendmessageTrayUpdate(new TrayItemInfo(Constants.AddinId, "Pause", $"Pause", string.Empty, pause));
      SendMessageTrayData(new TrayItemInfo(Constants.AddinId, "Stop", $"Stop", string.Empty, stop));
    }

    private void StartTimer(int minutes)
    {
      _timer.Enabled = false;
      _timer.Stop();
      _timer.Start();
      _timer.Enabled = true;

      _duration = minutes;
      UpdateTrayIcon(minutes);  // Move to StateChange?
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      if (_currentState != TimerState.Started)
        return;

      _duration -= 1;

      if (_duration < 0)
      {
        // We're done!
        ChangeTimerState(TimerState.Done);
        UpdateMenuItems(TimerState.Started);
        NotifyStateChanged(TimerState.Done);

        // TODO: Update stats for successful pomodoro here
      }
      else
      {
        UpdateTrayIcon(_duration);
      }
    }
  }
}

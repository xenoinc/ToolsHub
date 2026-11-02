/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PomodoroManager.cs
 * Description:
 *  Pomodoro timer manager
 */

using System;
using Lite.EventIpc;
using PomodoroAddin.Domain;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Logging;

namespace PomodoroAddin.Managers
{
  public class PomodoroManager : IDisposable
  {
    private readonly ISettingsService _settings;
    private readonly IEventAggregator _events;
    private readonly IPomodoroTimer _timer;
    private readonly object _sync = new();

    private TimerState _currentState = TimerState.Done;
    private int _duration;
    private bool _disposed;

    public PomodoroManager(ISettingsService settings, IEventAggregator events, IPomodoroTimer timer)
    {
      _settings = settings;
      _events = events;
      _timer = timer;
      _timer.Tick += Timer_Elapsed;
    }

    public int SettingTimerDuration => _settings.GetInt(Constants.AddinId, Constants.KeyDuration, 25);

    public int SettingTimerShortBreak => _settings.GetInt(Constants.AddinId, Constants.KeyShortBreak, 5);

    public int SettingTimerLongBreak => _settings.GetInt(Constants.AddinId, Constants.KeyLongBreak, 15);

    public bool SettingAlertSound => _settings.GetBool(Constants.AddinId, Constants.KeyAlertSound, true);

    public bool SettingAlertFlash => _settings.GetBool(Constants.AddinId, Constants.KeyAlertFlash, true);

    public bool SettingAlertTrayBubble => _settings.GetBool(Constants.AddinId, Constants.KeyAlertSysTrayBubble, true);

    public bool SettingTrayIconUpdates => _settings.GetBool(Constants.AddinId, Constants.KeyTrayShowDuration, false);

    public int RemainingMinutes
    {
      get
      {
        lock (_sync)
          return _duration;
      }
    }

    public TimerState CurrentState
    {
      get
      {
        lock (_sync)
          return _currentState;
      }
    }

    public void Dispose()
    {
      lock (_sync)
      {
        if (_disposed)
          return;
        _disposed = true;
        _timer.Tick -= Timer_Elapsed;
        _timer.Stop();
        _currentState = TimerState.Stopped;
      }
    }

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
      lock (_sync)
      {
        var state = _currentState != TimerState.Paused ? TimerState.Paused : TimerState.Started;
        ChangeTimerState(state);
        return 0;
      }
    }

    public int OnStop(string target)
    {
      ChangeTimerState(TimerState.Stopped);
      return 0;
    }

    /// <summary>Send message to SysTray icon</summary>
    /// <param name="icon">icon to display. NULL for default</param>
    public void SendMessageTrayIcon(System.Drawing.Icon icon)
    {
      _events.Publish(new SystemTrayChangeIconRequest(icon));
    }

    public void RefreshMenu() => _events.Publish(new SystemTrayRefreshRequest());

    public void SendMessageTrayData(TrayItemInfo itemInfo)
    {
      Log.Warn("Method currently not in use.");

      // TODO: Create infrastructure to inform MenuItems which ones to enable/disable/change-text
      //
      _events.Publish(new SystemTrayUpdateRequest(itemInfo));
    }

    public void PlaySound(uint repeat = 1, bool isTesting = false)
    {
      if (!SettingAlertSound && !isTesting)
        return;

      try
      {
        for (uint cnt = 1; cnt <= repeat; ++cnt)
        {
          System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
          System.IO.Stream s = a.GetManifestResourceStream("PomodoroAddin.chime2.wav");
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

    private void ChangeTimerState(TimerState state, int durationMinutes = 0)
    {
      lock (_sync)
      {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (_currentState == state)
          return;

        switch (state)
        {
          case TimerState.Started:

            if (_currentState == TimerState.Paused)
            {
              // resume timer
              Log.Debug($"Resuming timer for {SettingTimerDuration} minutes");
              _timer.Start();
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
            _timer.Stop();
            break;

          case TimerState.Stopped:
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
    }

    /// <summary>Alert to user of state change</summary>
    /// <param name="state">State transition</param>
    private void NotifyStateChanged(TimerState state)
    {
      // TODO:
      //  [ ] Flash on screen
      //  [ ] System Tray Bubble

      Log.Debug($"Alert user we're at state: {state}!!");
      Log.Warn("Notify state change feature is currently not available!");

      // TODO: If we pressed stop, don't play sound. Make TimerState.Stopped (manually stopped)
      if (state == TimerState.Started)
        PlaySound(1);
      else if (state == TimerState.Done)
        PlaySound(3);
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

        case TimerState.Stopped:
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
      _timer.Stop();
      _duration = minutes;
      _timer.Start();
      UpdateTrayIcon(minutes);
    }

    private void Timer_Elapsed(object sender, EventArgs e)
    {
      lock (_sync)
      {
        if (_disposed || _currentState != TimerState.Started)
          return;

        _duration -= 1;

        if (_duration <= 0)
        {
          // We're done!
          ChangeTimerState(TimerState.Done);
          UpdateMenuItems(TimerState.Done);
          // NotifyStateChanged(TimerState.Done);

          // TODO: Update stats for successful pomodoro here
        }
        else
        {
          UpdateTrayIcon(_duration);
        }
      }
    }
  }
}

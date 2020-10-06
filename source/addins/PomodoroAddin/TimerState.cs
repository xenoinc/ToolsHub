/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-12-29
 * Author:  Damian Suess
 * File:    TimerState.cs
 * Description:
 *  Timer states
 */

namespace PomodoroAddin
{
  public enum TimerState
  {
    /// <summary>Timer started</summary>
    Started,  // Was "Start"

    /// <summary>Timer paused</summary>
    Paused,   // Was, "Pause"

    /// <summary>Timer finished</summary>
    Done,

    /// <summary>Manually stopped timer</summary>
    Stopped
  };
}

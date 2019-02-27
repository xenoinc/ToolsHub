/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-1-22
 * Author:  Damian Suess
 * File:    TimerState.cs
 * Description:
 *  Timer state enumeration
 */

namespace PomodoroAddin.Domain
{
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
}

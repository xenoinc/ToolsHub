/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Log.cs
 * Rev:     3
 * Description:
 *  File and console logging system
 */

namespace Xeno.ToolsHub.Services.Logging
{
  public enum Level
  {
    /// <summary>Debug message.</summary>
    Debug,

    /// <summary>Informational message.</summary>
    Info,

    /// <summary>Warning message.</summary>
    Warn,

    /// <summary>An error has occurred.</summary>
    Error,

    /// <summary>Fatal-error.</summary>
    Fatal
  }
}

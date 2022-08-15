/* Copyright Xeno Innovations, Inc. 2020
 * Date:    2020-12-11
 * Author:  Damian Suess
 * File:    LogDevice.cs
 */

namespace Xeno.ToolsHub.Services.Logging
{
  /// <summary>Logging output device.</summary>
  public enum LoggerType
  {
    /// <summary>Output to debug Console.</summary>
    Console,

    /// <summary>Output to file.</summary>
    File,

    /////// <summary>Output to database.</summary>
    ////Database,
    /////// <summary>Output to system-wide events.</summary>
    ////Events
  }
}

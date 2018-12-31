/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Log.cs
 * Description:
 *  File and console logging system
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System;

  public enum Level { DEBUG, INFO, WARN, ERROR, FATAL };

  public static class Log
  {
    private static ILogger _logDev = new FileLogger();
    private static Level _logLevel = Level.DEBUG;
    private static bool _muted = false;

    public static ILogger LogDevice
    {
      get => _logDev;
      set => _logDev = value;
    }

    public static Level LogLevel
    {
      get => _logLevel;
      set => _logLevel = value;
    }

    private static string FormattedTime
    {
      get
      {
        return string.Format(
          "{0:00}:{1:00}:{2:00}.{3:000}",
          DateTime.Now.Hour,
          DateTime.Now.Minute,
          DateTime.Now.Second,
          DateTime.Now.Millisecond);
      }
    }

    public static void Debug(string message, params object[] args)
    {
      Logger(Level.DEBUG, message, args);
    }

    public static void Error(string message, params object[] args)
    {
      Logger(Level.ERROR, message, args);
    }

    public static void Fatal(string message, params object[] args)
    {
      Logger(Level.FATAL, message, args);
    }

    public static void Info(string message, params object[] args)
    {
      Logger(Level.INFO, message, args);
    }

    public static void Logger(Level level, string message, params object[] args)
    {
      string method = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().Name;
      string text = $"[{FormattedTime}] [{level.ToString()}] [{method}] [{message}]";

      System.Diagnostics.Debug.Print(">> " + text);

      // TODO: use this when ready
      //if (!_muted && level >= _logLevel)
      //  _logDev.Log(level, message, args);
    }

    /// <summary>Temporary disable logging output</summary>
    public static void Mute()
    {
      _muted = true;
    }

    public static void Unmute()
    {
      _muted = false;
    }

    public static void Warn(string message, params object[] args)
    {
      Logger(Level.WARN, message, args);
    }
  }
}

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
  using System;

  public enum Level
  {
    Debug,
    Info,
    Warn,
    Error,
    Fatal
  }

  public static class Log
  {
    private static ILogger _logDev = new FileLogger();
    private static Level _logLevel = Level.Debug;
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
      LogMessage(Level.Debug, message, args);
    }

    public static void Error(string message, params object[] args)
    {
      LogMessage(Level.Error, message, args);
    }

    public static void Fatal(string message, params object[] args)
    {
      LogMessage(Level.Fatal, message, args);
    }

    public static void Info(string message, params object[] args)
    {
      LogMessage(Level.Info, message, args);
    }

    public static void LogMessage(Level level, string message, params object[] args)
    {
      string cls = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().ReflectedType.Name;
      string method = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().Name;
      string text = $"[{FormattedTime}] [{level.ToString()}] [{cls}.{method}] [{message}]";

      System.Diagnostics.Debug.WriteLine(">> " + text);

      // TODO: use this when ready
      ////if (!_muted && level >= _logLevel)
      ////  _logDev.Log(level, message, args);
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
      LogMessage(Level.Warn, message, args);
    }
  }
}

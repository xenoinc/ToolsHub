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

  public static class Log
  {
    private static ILogger _logDev = new FileLogger();
    private static Level _logLevel = Level.Debug;
    private static bool _muteLogging = false;

    public static ILogger LogDevice
    {
      get => _logDev;
      set => _logDev = value;
    }

    /// <summary>Gets or sets system-wide logging level.</summary>
    public static Level LogLevel
    {
      get => _logLevel;
      set => _logLevel = value;
    }

    /// <summary>Gets or sets a value indicating whether to output logging or not. True == disabled.</summary>
    public static bool MuteLogging
    {
      get => _muteLogging;
      set => _muteLogging = value;
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
      var diag = new System.Diagnostics.StackTrace();
      string text = string.Empty, cls = string.Empty, method = string.Empty;

      if (diag.FrameCount > 1)
      {
        cls = diag.GetFrame(2).GetMethod().ReflectedType.Name;
        method = diag.GetFrame(2).GetMethod().Name;
      }

      text = $"[{FormattedTime}] [{level}] [{cls}.{method}] [{message}]";
      System.Diagnostics.Debug.WriteLine(">> " + text);

      if (!MuteLogging && level >= _logLevel)
      {
        _logDev.Log(level, text, args);

        // TODO: The factory should decide on the formatter
        //// _logDev.Log(level, message, args);
      }
    }

    public static void Warn(string message, params object[] args)
    {
      LogMessage(Level.Warn, message, args);
    }
  }
}

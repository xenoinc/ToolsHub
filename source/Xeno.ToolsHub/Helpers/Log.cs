/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Log.cs
 * Description:
 *
 */

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Xeno.ToolsHub.Helpers
{
  public enum Level { DEBUG, INFO, WARN, ERROR, FATAL };

  public interface ILogger
  {
    void Log(Level lvl, string msg, params object[] args);
  }

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
      //if (!_muted && level >= _logLevel)
      //  _logDev.Log(level, message, args);
    }

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

  internal class ConsoleLogger : ILogger
  {
#if WIN32
    private const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;

    public ConsoleLogger()
    {
      AttachConsole(ATTACH_PARENT_PROCESS);
    }

    ~ConsoleLogger()
    {
      FreeConsole();
    }

    [DllImport("kernel32.dll")]
    public static extern bool AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();

#endif

    public void Log(Level lvl, string msg, params object[] args)
    {
      Console.Write("[{0} {1:00}:{2:00}:{3:00}.{4:000}]",
                     Enum.GetName(typeof(Level), lvl),
                     DateTime.Now.Hour,
                     DateTime.Now.Minute,
                     DateTime.Now.Second,
                     DateTime.Now.Millisecond);
      msg = string.Format(" {0}", msg);
      if (args.Length > 0)
        Console.WriteLine(msg, args);
      else
        Console.WriteLine(msg);
    }
  }

  internal class FileLogger : ILogger
  {
    private ConsoleLogger _console;
    private StreamWriter _stream;

    public FileLogger()
    {
      _console = new ConsoleLogger();

      try
      {
        // Create file directory
      }
      catch
      {
        _console.Log(Level.WARN, "Failed to create log file.");
      }
    }

    public void Log(Level level, string message, params object[] args)
    {
      Console.WriteLine(message);
    }
  }
}

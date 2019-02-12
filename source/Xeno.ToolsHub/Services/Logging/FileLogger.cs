/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    FileLogger.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System;
  using System.IO;
  using Xeno.ToolsHub.Config;

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
        _console.Log(Level.Warn, "Failed to create log file.");
      }
    }

    public void Log(Level level, string message, params object[] args)
    {
      Console.WriteLine(message);
    }
  }
}

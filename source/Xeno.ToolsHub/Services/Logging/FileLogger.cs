/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    FileLogger.cs
 * Description:
 *  File logging.
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System;

  /// <summary>File logger.</summary>
  internal class FileLogger : ILogger
  {
    private ConsoleLogger _console;

    //// File output stream
    //// private System.IO.StreamWriter _stream;

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

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    FileLogger.cs
 * Description:
 *  File logging.
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System.IO;
  using System.Reflection;


  /// <summary>File logger.</summary>
  public class FileLogger : ILogger
  {
    //// File output stream
    //// private System.IO.StreamWriter _stream;
    private readonly string _baseName = "LogFile";

    private readonly object _lock = new object();

    public FileLogger()
    {
      ////exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      ////
      // Consider logging to console as well.
      ////_console = new ConsoleLogger();
      ////try
      ////{
      ////  // Create file directory
      ////}
      ////catch
      ////{
      ////  _console.Log(Level.Warn, "Failed to create log file.");
      ////}
    }

    ////public uint MaxFiles { get; set; } = 5;

    public uint MaxSize { get; set; } = 5000000;  // 5 MB

    public string LogFile => _baseName + ".log";

    ////private ConsoleLogger _console;
    public void Log(Level level, string message, params object[] args)
    {
      lock (_lock)
      {
        RolloverChecker();

        using (StreamWriter writer = File.AppendText(LogFile))
        {
          writer.WriteLine(message);
          writer.Close();
        }
      }
    }

    /// <summary>Checks if we need to create a new file and delete old ones.</summary>
    private void RolloverChecker()
    {
      if (!File.Exists(LogFile))
        return;

      var info = new FileInfo(LogFile);
      if (info.Length > MaxSize)
      {
        info.MoveTo(_baseName + "1.log");
      }
    }
  }
}

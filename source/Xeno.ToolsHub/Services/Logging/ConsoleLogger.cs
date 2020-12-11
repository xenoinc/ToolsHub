/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    ConsoleLogger.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using System;
  using System.Runtime.InteropServices;

  internal class ConsoleLogger : ILogger
  {
#if WIN32

    public ConsoleLogger()
    {
      NativeMethods.AttachConsole(NativeMethods.ATTACH_PARENT_PROCESS);
    }

    ~ConsoleLogger()
    {
      NativeMethods.FreeConsole();
    }

#endif

    public void Log(Level lvl, string msg, params object[] args)
    {
      Console.Write(
        "[{0} {1:00}:{2:00}:{3:00}.{4:000}]",
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

    private static class NativeMethods
    {
#if WIN32

      public const uint ATTACH_PARENT_PROCESS = 0x0ffffffff;

      [DllImport("kernel32.dll")]
      public static extern bool AttachConsole(uint dwProcessId);

      [DllImport("kernel32.dll")]
      public static extern bool FreeConsole();

#endif
    }
  }
}

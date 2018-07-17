/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Program.cs
 * Description:
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Xeno.ToolsHub
{
  static class Program
  {
    public static bool AbortShutdown = false;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      RegisterSystemEvents();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Views.MainForm());
    }
    private static void RegisterSystemEvents()
    {
      // https://msdn.microsoft.com/en-us/library/microsoft.win32.systemevents.aspx
      SystemEvents.SessionEnding += SystemEvents_SessionEnding;
      SystemEvents.SessionEnded += SystemEvents_SessionEnded;
    }

    private static void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
    {
      // Occurs when the user is logging off or shutting down the system
    }

    private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
      // Occurs when the user is trying to log off or shut down the system
      
      if (AbortShutdown == true)
      {
        e.Cancel = true;

        // old abort shutdown
        //string cmd = "shutdown /a";
        //System.Diagnostics.Process.Start(cmd);
      }
    }
  }
}

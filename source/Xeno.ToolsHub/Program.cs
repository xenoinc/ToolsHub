/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Program.cs
 * Description:
 *
 */

using System;
using System.Threading;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub
{
  internal static class Program
  {
    public static bool AbortShutdown = false;

    /// <summary>global singleton</summary>
    public static Config.Settings.AppSettings Settings;

    private static Mutex _mutex = null;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Log.Debug("Starting system");
      if (HasPrevInstance())
      {
        Log.Debug("App already running. Goodbye!");
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      LoadAppSettings();

      var appContext = new MainHandler();
      Application.Run(appContext);

      //Application.Run(new Views.MainForm());
      //Application.Run(new Views.PreferencesForm());
    }

    /// <summary>Check for previous instance</summary>
    /// <returns>Returns true is app is already running</returns>
    private static bool HasPrevInstance()
    {
      const string appName = "ToolsHub-{AC52F444-759A-4681-9D5D-1E234502B5E1}";
      string dbg = string.Empty;
      bool createdNew;

      // Allows us to run from VS and
      if (Helpers.IsDebugging)
      {
        dbg = "-IsDebugging";
      }

      _mutex = new Mutex(true, appName + dbg, out createdNew);
      if (!createdNew)
        return true;
      else
        return false;
    }

    /// <summary>Load application settings</summary>
    /// <returns>False if settings file does not exist</returns>
    private static void LoadAppSettings()
    {
      Log.Debug("Loading app settings");
      //TODO: Load application settings
      //// Load application settings
      //Settings = new AppSettings();
      //Settings.InitializeDefaults();
      //Settings = Settings.Load();
      //
      //if (System.IO.File.Exists())
    }
  }
}
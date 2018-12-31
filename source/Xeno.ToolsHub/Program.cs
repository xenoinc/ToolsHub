/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Program.cs
 * Description:
 *  Main entry point
 */

namespace Xeno.ToolsHub
{
  using System;
  using System.Threading;
  using System.Windows.Forms;
  using Xeno.ToolsHub.Config;
  using Xeno.ToolsHub.Services.Logging;

  internal static class Program
  {
    private static Mutex _mutex = null;

    public static bool AbortShutdown { get; set; }

    /// <summary>Gets or sets global singleton</summary>
    /// <value>System settings</value>
    public static Xeno.ToolsHub.Managers.SettingsManager Settings { get; set; }

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

      AbortShutdown = false;
      LoadAppSettings();

      var appContext = new MainHandler();
      Application.Run(appContext);

      // Application.Run(new Views.MainForm());
      // Application.Run(new Views.PreferencesForm());
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

    /// <summary>Loads settings file according to priority</summary>
    private static void LoadAppSettings()
    {
      Log.Debug("Loading app settings");

      string portable = System.IO.Path.Combine(Helpers.GetStorageFolder(StorageMethod.PortableApp), Constants.SettingsFile);
      string allUsers = System.IO.Path.Combine(Helpers.GetStorageFolder(StorageMethod.AllUsers), Constants.SettingsFile);
      string singleUser = System.IO.Path.Combine(Helpers.GetStorageFolder(StorageMethod.SingleUser), Constants.SettingsFile);

      Settings = new Managers.SettingsManager(StorageMethod.Unknown);

      if (System.IO.File.Exists(portable))
      {
        Settings.StorageMethod = StorageMethod.PortableApp;
      }
      else if (System.IO.File.Exists(allUsers))
      {
        Settings.StorageMethod = StorageMethod.AllUsers;
      }
      else if (System.IO.File.Exists(allUsers))
      {
        Settings.StorageMethod = StorageMethod.SingleUser;
      }
      else
      {
        // TODO: New install: Ask user which mode to use
        Settings.StorageMethod = StorageMethod.PortableApp;
      }

      Settings.LoadFile();
    }
  }
}
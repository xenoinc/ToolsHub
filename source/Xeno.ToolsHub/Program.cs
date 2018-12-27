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

  internal static class Program
  {
    private static Mutex _mutex = null;

    public static bool AbortShutdown { get; set; }

    /// <summary>Gets or sets global singleton</summary>
    /// <value>System settings</value>
    // public static Config.Settings.AppSettings Settings { get; set; }
    public static Services.PropertyService.PropertiesManager Settings { get; set; }

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

    /// <summary>Load application settings</summary>
    private static void LoadAppSettings()
    {
      // Load settings file here according to priority
      // 1) Check if a current directory file exists
      // 2) Check if local user file exists
      // 3) Check if all users file exists
      // 4) Default: Ask user which mode to use
      Log.Debug("Loading app settings");

      string portable = System.IO.Path.Combine(Helpers.GetStoragePath(StorageMethod.PortableApp), Constants.SettingsFile);
      string allUsers = System.IO.Path.Combine(Helpers.GetStoragePath(StorageMethod.AllUsers), Constants.SettingsFile);
      string singleUser = System.IO.Path.Combine(Helpers.GetStoragePath(StorageMethod.SingleUser), Constants.SettingsFile);

      if (System.IO.File.Exists(portable))
      {
        Helpers.StorageMethod = StorageMethod.PortableApp;
      }
      else if (System.IO.File.Exists(allUsers))
      {
        Helpers.StorageMethod = StorageMethod.AllUsers;
      }
      else if (System.IO.File.Exists(allUsers))
      {
        Helpers.StorageMethod = StorageMethod.SingleUser;
      }
      else
      {
        // Assume new install; Prompt user which storage method they'd like
        Helpers.StorageMethod = StorageMethod.PortableApp;
        throw new NotImplementedException();
      }

      ////TODO: Load application settings
      ////// Load application settings
      ////Settings = new AppSettings();
      ////Settings.InitializeDefaults();
      ////Settings = Settings.Load();
      ////
      ////if (System.IO.File.Exists())
    }
  }
}
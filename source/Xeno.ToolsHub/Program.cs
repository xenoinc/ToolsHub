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
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Windows.Forms;
#if USE_SQUIRREL
  ////using Squirrel;
#endif
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

      Log.Debug("Let's do this!!   ლ(｀ー´ლ)");

      Task.Run(async () =>
      {
        await SquirrelAutoUpdateAsync();
      });

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      AbortShutdown = false;
      LoadAppSettings();

      var appContext = new MainHandler();
      Application.Run(appContext);

      // Application.Run(new Views.MainForm());
      // Application.Run(new Views.PreferencesForm());
    }

    /// <summary>Update application asynchronously</summary>
    /// <returns>Task</returns>
    private static async Task SquirrelAutoUpdateAsync()
    {
      ////try
      ////{
      ////  SquirrelSetCredentials();
      ////}
      ////catch (Exception ex)
      ////{
      ////  Log.Error($"Error attempting to set network creds for update location. Ex: {ex.Message}");
      ////}

      await Task.Yield();

#if USE_SQUIRREL
      ////try
      ////{
      ////  ReleaseEntry release = null;
      ////
      ////  // TODO: Network creds https://github.com/Squirrel/Squirrel.Windows/issues/946
      ////  using (var mgr = new UpdateManager(Constants.RemoteUpdateStablePath))
      ////  {
      ////    SquirrelAwareApp.HandleEvents(
      ////      onInitialInstall: _ => mgr.CreateShortcutForThisExe(),
      ////      onAppUpdate: _ => mgr.CreateShortcutForThisExe(),
      ////      onAppUninstall: _ => mgr.RemoveShortcutForThisExe(),
      ////      onFirstRun: SquirrelWelcomeMessage);
      ////
      ////    UpdateInfo updateInfo = await mgr.CheckForUpdate();
      ////    if (updateInfo.ReleasesToApply.Any())
      ////    {
      ////      Log.Debug("Update found! Applying it to application...");
      ////      release = await mgr.UpdateApp();
      ////    }
      ////    else
      ////    {
      ////      Log.Debug("No updates found in repository.");
      ////    }
      ////  }
      ////
      ////  if (release != null)
      ////  {
      ////    Log.Debug("Restarting application...");
      ////    UpdateManager.RestartApp();
      ////  }
      ////  else
      ////  {
      ////    Log.Debug("No updates are required.");
      ////  }
      ////}
      ////catch (Exception exUpdate)
      ////{
      ////  Log.Error($"Issue looking for updates. {exUpdate.Message}");
      ////}
#else
      Log.Debug($"Squirrel Updater is not enabled.");
#endif
    }

    private static void SquirrelWelcomeMessage()
    {
      Log.Debug("App running for the first time. YAY!!");
    }

    /////// <summary>Set network credentials</summary>
    ////private static void SquirrelSetCredentials()
    ////{
    ////  // Method 1: https://stackoverflow.com/questions/3700871/connect-to-network-drive-with-user-name-and-password
    ////  // Method 2: https://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share
    ////  Log.Debug("Enforcing credentials");
    ////
    ////  try
    ////  {
    ////    System.Net.NetworkCredential netCreds = new System.Net.NetworkCredential(Constants.RemoteUserName, Constants.RemoteUserPass);
    ////    System.Net.CredentialCache credCache = new System.Net.CredentialCache();
    ////    credCache.Add(new Uri(Constants.RemoteUri), "Basic", netCreds);
    ////    // string[] theFolders = Directory.GetDirectories(@"\\computer\share");
    ////  }
    ////  catch (Exception ex)
    ////  {
    ////    Log.Error($"Problem applying domain creds. {ex.Message}");
    ////  }
    ////}

    /// <summary>Check for previous instance</summary>
    /// <returns>Returns true is app is already running</returns>
    private static bool HasPrevInstance()
    {
      const string appName = "ToolsHub-{AC52F444-759A-4681-9D5D-1E234502B5E1}";
      string dbg = string.Empty;
      bool createdNew;

      // Allows us to run from VS and live
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
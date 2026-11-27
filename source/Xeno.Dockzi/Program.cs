/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Program.cs
 * Description:
 *  Main entry point
 */

namespace Xeno.Dockzi
{
  using System;
  using System.Linq;
  using System.Threading;
  using System.Windows.Forms;
  using Lite.EventIpc;
  using Microsoft.Extensions.DependencyInjection;
  using Xeno.Dockzi.Config;
  using Xeno.Dockzi.Services;
  using Xeno.Dockzi.Services.Addins;
  using Xeno.Dockzi.Services.Logging;
  using Xeno.Dockzi.Services.Messaging;

  internal static class Program
  {
    private static Mutex _mutex = null;

    public static bool AbortShutdown { get; set; }

    /// <summary>Gets or sets compatibility access for add-ins not yet using injected settings.</summary>
    /// <value>System settings</value>
    public static Xeno.Dockzi.Managers.SettingsManager Settings { get; set; }

    /// <summary>Check for previous instance</summary>
    /// <returns>Returns true is app is already running</returns>
    private static bool HasPrevInstance()
    {
      const string appName = "Dockzi-{AC52F444-759A-4681-9D5D-1E234502B5E1}";
      string dbg = string.Empty;

      // Allows us to run from VS and live
      if (Helpers.IsDebugging)
      {
        dbg = "-IsDebugging";
      }

      _mutex = new Mutex(true, appName + dbg, out bool createdNew);
      if (!createdNew)
        return true;
      else
        return false;
    }

    /// <summary>The main entry point for the application.</summary>
    [STAThread]
    private static void Main(string[] args)
    {
      Velopack.VelopackApp.Build().Run();

      // Windows startup does not guarantee the application's working directory.
      if (args.Contains("--auto-start", StringComparer.OrdinalIgnoreCase))
        System.IO.Directory.SetCurrentDirectory(AppContext.BaseDirectory);

      Log.Debug("Starting system");
      if (HasPrevInstance())
      {
        Log.Debug("App already running. Goodbye!");
        return;
      }

      Log.Debug("Let's do this!!   ლ(｀ー´ლ)");

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      AbortShutdown = false;
      ApplicationUpdates updates;

      using (var services = new ServiceCollection()
        .AddDockzi()
        .AddBundledAddinServices()
        .BuildServiceProvider(new ServiceProviderOptions
        {
          ValidateScopes = true,
          ValidateOnBuild = true,
        }))
      {
        // Compatibility bridge for add-ins that have not migrated to constructor injection yet.
        Settings = services.GetRequiredService<Managers.SettingsManager>();
        AppEvents.Instance = services.GetRequiredService<IEventAggregator>();
        var autoStart = services.GetRequiredService<IAutoStartService>();

        // Repair stale paths on manual launches, not while Windows processes its Run entries.
        if (!args.Contains("--auto-start", StringComparer.OrdinalIgnoreCase))
        {
          try
          {
            autoStart.RepairRegistration();
          }
          catch (Exception ex) when (ex is UnauthorizedAccessException || ex is System.IO.IOException || ex is System.Security.SecurityException)
          {
            Log.Warn($"Unable to repair Windows startup registration: {ex.Message}");
          }
        }

        // Check for updates
        updates = services.GetRequiredService<ApplicationUpdates>();
        if (!updates.PrepareStartupUpdateAsync().GetAwaiter().GetResult())
          Application.Run(services.GetRequiredService<MainHandler>());
      }

      updates.ApplyPendingUpdate();
    }
  }
}

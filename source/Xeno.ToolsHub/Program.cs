/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    Program.cs
 * Description:
 *
 */

using System;
using System.Windows.Forms;
using Microsoft.Win32;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Helpers;

[assembly: Mono.Addins.AddinRoot("ToolsHub", "1.0")]

namespace Xeno.ToolsHub
{
  internal static class Program
  {
    public static bool AbortShutdown = false;

    /// <summary>global singleton</summary>
    public static AppSettings Settings;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      InitMonoAddins();

      InitSystemEvents();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      LoadSettings();

      var appContext = new MainHandler();
      Application.Run(appContext);

      //Application.Run(new Views.MainForm());
      //Application.Run(new Views.PreferencesForm());
    }

    /// <summary>Load application settings</summary>
    /// <returns>False if settings file does not exist</returns>
    private static bool LoadSettings()
    {
      bool hasSettings = false;

      Settings = new AppSettings();
      Settings.InitializeDefaults();
      Settings = Settings.Load();

      return false;
    }

    #region Add-ins

    private static void InitMonoAddins()
    {
      Mono.Addins.AddinManager.AddinLoaded += OnAddinLoaded;
      Mono.Addins.AddinManager.AddinUnloaded += OnAddinUnloaded;

      Mono.Addins.AddinManager.Initialize(".");
      Mono.Addins.AddinManager.Registry.Rebuild(null);  // Rebuild registry when debugging
      Mono.Addins.AddinManager.AddExtensionNodeHandler(Helpers.ExtensionPaths.OnStartupAddinsPath, OnStartupAddins_ExtensionHandler);
    }

    private static void OnAddinLoaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Mono.Addins.Addin addin = Mono.Addins.AddinManager.Registry.GetAddin(args.AddinId);
      Log.Debug($"=============================");
      Log.Debug($"OnAddinLoaded: {args.AddinId}");
      Log.Debug($"         Name: '{addin.Name}'");
      Log.Debug($"  Description: '{addin.Description.Description}'");
      Log.Debug($"    Namespace: '{addin.Namespace}'");
      Log.Debug($"      Enabled: '{addin.Enabled}'");
      Log.Debug($"         File: '{addin.AddinFile}'");
      Log.Debug("= = = = = = = = = = = = =");
    }

    private static void OnAddinUnloaded(object sender, Mono.Addins.AddinEventArgs args)
    {
      Log.Debug($"OnAddinUnloaded: {args.AddinId}");
    }

    private static void OnStartupAddins_ExtensionHandler(object sender, Mono.Addins.ExtensionNodeEventArgs args)
    {
      Mono.Addins.TypeExtensionNode extNode = args.ExtensionNode as Mono.Addins.TypeExtensionNode;

      // Execute via class interface definition of extension path
      IStartupExtension ext = (IStartupExtension)args.ExtensionObject;
      ext.Run();

      // Execute via typeof
      //ApplicationAddin addin;
      //addin = extNode.GetInstance(typeof(ApplicationAddin)) as ApplicationAddin;
      //addin.Initialize();

      Log.Debug("###########################");
      Log.Debug("OnStartChanged");
      Log.Debug($"  Id      - '{args.ExtensionNode.Id}'");
      Log.Debug($"  Path    - '{args.Path}'");
      Log.Debug($"  Node    - '{args.ExtensionNode}'");
      Log.Debug($"  Object  - '{args.ExtensionObject}'");
      Log.Debug($"  Changed - '{args.Change.ToString()}'");
      Log.Debug("   --[ ExtensionNode ]------");
      Log.Debug($"  Id      - '{extNode.Id}'");
      Log.Debug($"  ToString- '{extNode.ToString()}'");
      Log.Debug($"  TypeName- '{extNode.TypeName}'");
      Log.Debug("  Running...");
    }

    #endregion Add-ins

    #region System Events

    private static void InitSystemEvents()
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

      foreach (Mono.Addins.TypeExtensionNode node in Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPaths.SystemShutdownPath))
      {
        ISystemShutdownExtension ext = (ISystemShutdownExtension)node.CreateInstance();
        Log.Debug($"  Running add-in titled, '{ext.Title}'");
        ext.Run();
      }

      if (AbortShutdown == true)
      {
        e.Cancel = true;
        // old abort shutdown
        //string cmd = "shutdown /a";
        //System.Diagnostics.Process.Start(cmd);
      }
    }

    #endregion System Events
  }
}
/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-9
 * Author:  Damian Suess
 * File:    MainHandler.cs
 * Description:
 *  Main application handler. We don't need a GUI form, but do need WndProc
 *
 */

[assembly: Mono.Addins.AddinRoot("ToolsHub", "0.3", Namespace = "XenoInnovations", CompatVersion = "0.3")]

namespace Xeno.ToolsHub
{
  using System.Windows.Forms;
  using Microsoft.Win32;
  using Xeno.ToolsHub.Managers;
  using Xeno.ToolsHub.Services.Logging;

  /// <summary>
  /// Main application handler. We don't need a GUI form, but do need WndProc
  /// </summary>
  public class MainHandler : ApplicationContext
  {
    private readonly WndProcManager _wndProc;
    private readonly AddinsManager _addinsManager;

    public MainHandler(AddinsManager addins, SystemTrayManager systemTray, WndProcManager wndProc)
    {
      _addinsManager = addins;
      _wndProc = wndProc;
      Log.Debug("ToolsHub initializing..");

      // The container constructs add-ins and the system tray before this context.
      InitSystemEvents();
      InitWndProc();

      // We're done loading
      InitUtilityAddins();

#if DEBUG
      DebugTests();
#endif

      Application.ApplicationExit += Application_ApplicationExit;
    }

    public AddinsManager Addins => _addinsManager;

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
        SystemEvents.SessionEnded -= SystemEvents_SessionEnded;
        Application.ApplicationExit -= Application_ApplicationExit;
        _wndProc.DestroyWindow();
      }

      base.Dispose(disposing);
    }

    private void Application_ApplicationExit(object sender, System.EventArgs e)
    {
      // TODO: Inform add-ins of closing application
      // TODO: Clean up any additional resources
      // throw new System.NotImplementedException();
    }

    private void DebugTests()
    {
    }

    private void InitUtilityAddins()
    {
      // Run free-floating utility add-ins
      _addinsManager.LoadUtilityAddins();
    }

    private void InitSystemEvents()
    {
      // https://msdn.microsoft.com/en-us/library/microsoft.win32.systemevents.aspx
      SystemEvents.SessionEnding += SystemEvents_SessionEnding;
      SystemEvents.SessionEnded += SystemEvents_SessionEnded;
    }

    /// <summary>We have a GUI form, but do need WndProc for wiring up things</summary>
    private void InitWndProc()
    {
      _wndProc.CreateWindow();
    }

    private void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
    {
      // Occurs when the user is logging off or shutting down the system
    }

    private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
    {
      // TODO: Retest handler of system log-off/shutdown.
      ////// Occurs when the user is trying to log off or shut down the system
      ////
      ////foreach (Mono.Addins.TypeExtensionNode node in Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPaths.SystemShutdownPath))
      ////{
      ////  ISystemShutdownExtension ext = (ISystemShutdownExtension)node.CreateInstance();
      ////  Log.Debug($"  Running add-in titled, '{ext.Title}'");
      ////  ext.Execute();
      ////}
      ////
      ////if (Program.AbortShutdown == true)
      ////{
      ////  e.Cancel = true;
      ////  // old abort shutdown
      ////  //string cmd = "shutdown /a";
      ////  //System.Diagnostics.Process.Start(cmd);
      ////}
    }
  }
}

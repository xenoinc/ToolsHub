/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-9
 * Author:  Damian Suess
 * File:    MainHandler.cs
 * Description:
 *  Main application handler. We don't need a GUI form, but do need WndProc
 */

using System;
using System.Windows.Forms;

// [assembly: Mono.Addins.AddinRoot("ToolsHub", "1.0")]

namespace Xeno.ToolsHub
{
  public class MainHandler : ApplicationContext
  {
    //private NotifyIcon _trayIcon = new NotifyIcon();
    //PreferencesForm prefWnd = new PreferencesForm();
    //MenuItem configMenuItem

    Managers.SystemTrayManager _sysTray;
    Managers.WndProcManager _wndProc;

    public MainHandler()
    {
      // 1. Listen for system events
      // 2. Initialize SystemTray (add-in) handler
      // 3. Initialize Sidebar (add-in) handler
      // 4. Initialize Application add-in manager

      InitSystemEvents();
      InitSystemTray();
      InitWndProc();
      //InitSideBar();
      InitMonoAddins();
      Application.ApplicationExit += Application_ApplicationExit;
    }

    private void Application_ApplicationExit(object sender, System.EventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void InitMonoAddins()
    {
    }

    private void InitSystemEvents()
    {
    }

    private void InitSystemTray()
    {
      // Consider adding this into a container
      _sysTray = new Managers.SystemTrayManager();
    }

    private void InitWndProc()
    {
      _wndProc = new Managers.WndProcManager("ToolsHub");
      _wndProc.CreateWindow();
    }
  }
}

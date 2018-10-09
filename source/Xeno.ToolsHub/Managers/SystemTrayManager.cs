/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-19
 * Author:  Damian Suess
 * File:    SystemTrayManager.cs
 * Description:
 *  Manager for SystemTray icons
 *
 * Reference:
 *  NotifyIcon icon = new NotifyIcon();
 *  https://www.codeproject.com/Articles/18683/Creating-a-Tasktray-Application
 *  https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.Managers
{
  public class SystemTrayManager
  {
    private NotifyIcon _trayIcon = new NotifyIcon();
    private string TrayIcon = "Gears.ico";

    public SystemTrayManager()
    {
    }
  }
}

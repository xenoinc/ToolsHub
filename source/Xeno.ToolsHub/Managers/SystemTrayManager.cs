/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-19
 * Author:  Damian Suess
 * File:    SystemTrayManager.cs
 * Description:
 *  Manager for SystemTray icons
 *
 * TODO:
 *  [ ] Load custom icon
 *  [ ] Add methods to load add-in sub menus
 *  [ ] Ability to add/rmv menu items at will
 *
 * Reference:
 *  NotifyIcon icon = new NotifyIcon();
 *  https://www.codeproject.com/Articles/18683/Creating-a-Tasktray-Application
 *  https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
 */

using System;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Managers
{
  public class SystemTrayManager : ApplicationContext
  {
    private MenuItem[] _trayMenu;
    private NotifyIcon _trayNotify = new NotifyIcon();

    public SystemTrayManager()
    {
      InitMenu();

      _trayNotify.Icon = InitIcon();
      _trayNotify.ContextMenu = new ContextMenu(_trayMenu);
      _trayNotify.DoubleClick += new EventHandler(OnMenu_DoubleClick);
      _trayNotify.Visible = true;
    }

    private System.Drawing.Icon InitIcon()
    {
      // TODO: Load custom icon here
      return Properties.Resources.AppIcon;
    }

    private void InitMenu()
    {
      _trayMenu = new MenuItem[]
      {
        new MenuItem("Properties", OnMenu_Properties) ,  // Make this bold
        new MenuItem("-"),
        new MenuItem("About", OnMenu_About),
        new MenuItem("Exit", OnMenu_Exit)
      };
    }

    private void OnMenu_About(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }

    private void OnMenu_DoubleClick(object sender, EventArgs e)
    {
      // Show Properties dialog (or About).
      throw new NotImplementedException();
    }

    private void OnMenu_Exit(object sender, EventArgs e)
    {
      _trayNotify.Dispose();
      Application.Exit();
    }

    private void OnMenu_Properties(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}

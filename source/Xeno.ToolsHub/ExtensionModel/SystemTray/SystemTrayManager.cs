/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-19
 * Author:  Damian Suess
 * File:    SystemTrayManager.cs
 * Description:
 *  Manager for SystemTray icons
 *
 * TODO:
 *  [ ] Load custom icon
 *  [ ] Ability to add/rmv menu items at will
 *
 * Reference:
 *  - gfx https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.ExtensionModel.SystemTray
{
  public class SystemTrayManager : ApplicationContext
  {
    public SystemTrayManager(MainHandler mainHandler)
    {
      _mainHandler = mainHandler;
      MenuRefresh();
    }

    public void AlertBubble(string message, string title)
    {
      //TODO: Use Xamarin's MessageSender and subscribe when to show balloon
      _trayNotify.BalloonTipTitle = message;
      _trayNotify.BalloonTipText = title;
      _trayNotify.ShowBalloonTip(1000);
    }

    public void MenuRefresh()
    {
      //TODO: Create a singleton or IoC pattern to call Refresh() from outside
      InitTrayMenu();
      DrawTrayNotifacation();
    }

    private MenuItem[] _trayMenu;
    private NotifyIcon _trayNotify = new NotifyIcon();
    private MainHandler _mainHandler;

    private System.Drawing.Icon ApplicationIcon
    {
      get
      {
        if (Helpers.IsDebugging)
        {
          System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(16, 16);
          using (System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(bmp))
            gfx.FillEllipse(System.Drawing.Brushes.Red, 0, 0, 16, 16);

          return System.Drawing.Icon.FromHandle(bmp.GetHicon());
        }
        else
        {
          // TODO: Load custom icon from settings/extension
          return Properties.Resources.AppIcon;
        }
      }
    }

    /// <summary>Redraw systray menu from memory</summary>
    private void DrawTrayNotifacation()
    {
      _trayNotify.Icon = ApplicationIcon;
      _trayNotify.ContextMenu = new ContextMenu(_trayMenu);
      _trayNotify.DoubleClick += new EventHandler(OnMenuDoubleClick);
      _trayNotify.Visible = true;
    }

    private void InitTrayMenu()
    {
      string dbgTag = string.Empty;
      if (Helpers.IsDebugging)
      {
        dbgTag = " (DEBUG)";
      }

      List<MenuItem> menuBuilder = new List<MenuItem>();
      menuBuilder.Add(new MenuItem("ToolsHub" + dbgTag, OnMenuProperties));
      menuBuilder.Add(new MenuItem("-"));

      List<MenuItem> epMenus = new List<MenuItem>();

      // Load add-in menus
      epMenus = LoadMenuFromExtensionPoint();
      if (epMenus.Count > 0)
        menuBuilder.AddRange(epMenus);

      menuBuilder.Add(new MenuItem("About", OnMenuAbout));
      menuBuilder.Add(new MenuItem("Exit", OnMenuExit));

      _trayMenu = menuBuilder.ToArray();
    }

    private List<MenuItem> LoadMenuFromExtensionPoint()
    {
      Log.Debug("Entering");

      List<MenuItem> addinItems = new List<MenuItem>();
      Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPath.SystemTray);

      Log.Debug($"Found '{nodes.Count}' items ...");
      foreach (Mono.Addins.ExtensionNode node in nodes)
      {
        Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;
        try
        {
          SysTrayAddin addin = typeNode.CreateInstance() as SysTrayAddin;
          Log.Debug($"SysTrayAdd-in [{addin.ToString()}]");

          addinItems = addin.MenuItems();
        }
        catch (Exception ex)
        {
          Log.Error("Couldn't create SysTrayAddin instance: " + ex.Message);
        }
      }

      return addinItems;
    }

    private void OnMenuAbout(object sender, EventArgs e)
    {
      Form f = new Views.AboutForm();
      f.ShowDialog();
    }

    private void OnMenuDoubleClick(object sender, EventArgs e)
    {
      // Show Properties dialog (or About).
      throw new NotImplementedException();
    }

    private void OnMenuExit(object sender, EventArgs e)
    {
      _trayNotify.Dispose();
      Application.Exit();
    }

    private void OnMenuProperties(object sender, EventArgs e)
    {
      Form p = new Views.PreferencesForm(_mainHandler.Addins);
      p.Show();
    }
  }
}

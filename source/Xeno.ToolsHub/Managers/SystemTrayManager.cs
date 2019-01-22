/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-19
 * Author:  Damian Suess
 * File:    SystemTrayManager.cs
 * Description:
 *  Manager for SystemTray icons
 *
 * TODO:
 *  [ ] SysTray - Interactive bubbles - http://www.codeproject.com/Articles/529753/InteractiveToolTip-Tooltips-you-can-click-on
 *  [ ] Add custom icon, and timeout duration
 *  [ ] Ability to add/rmv menu items at will
 *
 * Reference:
 *  - gfx https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services.Logging;

namespace Xeno.ToolsHub.Managers
{
  public class SystemTrayManager : ApplicationContext
  {
    private MenuItem[] _trayMenu;

    private NotifyIcon _trayNotify = new NotifyIcon();

    private MainHandler _mainHandler;

    public SystemTrayManager(MainHandler mainHandler)
    {
      _mainHandler = mainHandler;
      MessageCenterSubscribe();
      MenuRefresh();
    }

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
          return Properties.Resources.AppIcon;
        }
      }
    }

    public void MenuRefresh()
    {
      InitTrayMenu();
      DrawTrayNotifacation();
    }

    protected override void Dispose(bool disposing)
    {
      MessagesUnsubscribe();
      base.Dispose(disposing);
    }

    private void AlertBubble(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int displayTime = 5000)
    {
      // TODO: SysTray - Interactive bubbles - http://www.codeproject.com/Articles/529753/InteractiveToolTip-Tooltips-you-can-click-on
      _trayNotify.BalloonTipTitle = message;
      _trayNotify.BalloonTipText = title;
      _trayNotify.ShowBalloonTip(displayTime, title, message, icon);
    }

    /// <summary>Redraw SysTray menu from memory</summary>
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
          Log.Debug($"SysTray Add-in initializing [{addin.ToString()}]");

          addinItems.AddRange(addin.MenuItems());
        }
        catch (Exception ex)
        {
          Log.Error("Couldn't create SysTrayAddin instance: " + ex.Message);
        }
      }

      return addinItems;
    }

    /// <summary>Subscribe to messaging center messages</summary>
    private void MessageCenterSubscribe()
    {
      MessagingCenter.Subscribe<SystemTrayMessages>(this, SystemTrayMessages.Refresh, (sender) =>
      {
        Log.Debug("Refreshing system tray icon set");
        MenuRefresh();
      });

      MessagingCenter.Subscribe<SystemTrayMessages, string>(this, SystemTrayMessages.Notify, (sender, msg) =>
      {
        // TODO: Add custom icon, and timeout duration
        string title = string.Empty;
        ToolTipIcon icon = ToolTipIcon.Info;
        int timeout = 5000;

        Log.Debug($"Alert bubble title:'{title}', msg:'{msg}'");
        AlertBubble(title, msg, icon, timeout);
      });

      MessagingCenter.Subscribe<SystemTrayMessages, System.Drawing.Icon>(this, SystemTrayMessages.CustomIcon, (sender, icon) =>
      {
        if (icon != null)
        {
          Log.Debug("Changing SystemTrayIcon to custom");
          _trayNotify.Icon = icon;
        }
        else
        {
          Log.Debug("Changing SystemTrayIcon to default");
          _trayNotify.Icon = Properties.Resources.AppIcon;
        }
      });
    }

    /// <summary>Unsubscribe from messaging center messages</summary>
    private void MessagesUnsubscribe()
    {
      MessagingCenter.Unsubscribe<SystemTrayManager>(this, SystemTrayMessages.Refresh);
      MessagingCenter.Unsubscribe<SystemTrayManager, string>(this, SystemTrayMessages.Notify);
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

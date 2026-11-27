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
using Lite.EventIpc;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Services.Logging;
using Xeno.Dockzi.Services.Messaging;

namespace Xeno.Dockzi.Managers
{
  public class SystemTrayManager : ApplicationContext
  {
    private readonly AddinsManager _addins;
    private readonly Control _eventDispatcher = new();
    private readonly TrayEventSubscriptions _eventSubscriptions;
    private readonly IPreferencesDialogService _preferences;
    private readonly NotifyIcon _trayNotify = new();
    private readonly ApplicationUpdates _updates;

    private System.Drawing.Icon _customIcon;
    private ToolStripItem[] _trayMenu;

    public SystemTrayManager(AddinsManager addins, IEventAggregator events, IPreferencesDialogService preferences, ApplicationUpdates updates = null)
    {
      _addins = addins;
      _preferences = preferences;
      _updates = updates;
      _eventDispatcher.CreateControl();
      _eventSubscriptions = new TrayEventSubscriptions(events, MenuRefresh, OnTrayNotification, OnTrayIconChanged, DispatchEvent);
      _trayNotify.DoubleClick += OnMenuDoubleClick;
      MenuRefresh();
    }

    private System.Drawing.Icon ApplicationIcon
    {
      get
      {
        if (Helpers.IsDebugging)
        {
          System.Drawing.Bitmap bmp = new(16, 16);
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
      if (disposing)
      {
        _eventSubscriptions.Dispose();
        _eventDispatcher.Dispose();
        _trayNotify.DoubleClick -= OnMenuDoubleClick;
        _trayNotify.Dispose();
        _customIcon?.Dispose();
        _customIcon = null;
      }

      base.Dispose(disposing);
    }

    private static ContextMenuStrip BuildContextMenuStrip(IEnumerable<ToolStripItem> menuItems)
    {
      var contextMenuStrip = new ContextMenuStrip();
      foreach (ToolStripItem menuItem in menuItems)
      {
        contextMenuStrip.Items.Add(menuItem);
      }

      return contextMenuStrip;
    }

    private void AlertBubble(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int displayTime = 5000)
    {
      // TODO: SysTray - Interactive bubbles - http://www.codeproject.com/Articles/529753/InteractiveToolTip-Tooltips-you-can-click-on
      // Windows can have it disabled at an OS level - http://support.microsoft.com/kb/307729
      _trayNotify.BalloonTipTitle = message;
      _trayNotify.BalloonTipText = title;
      _trayNotify.ShowBalloonTip(displayTime, title, message, icon);
    }

    private void DispatchEvent(Action action)
    {
      if (_eventDispatcher.IsDisposed)
        return;

      if (_eventDispatcher.InvokeRequired)
        _eventDispatcher.BeginInvoke(action);
      else
        action();
    }

    /// <summary>Redraw SysTray menu from memory</summary>
    private void DrawTrayNotifacation()
    {
      OnTrayIconChanged(null);
      _trayNotify.ContextMenuStrip?.Dispose();
      _trayNotify.ContextMenuStrip = BuildContextMenuStrip(_trayMenu);
      _trayNotify.Visible = true;
    }

    private void InitTrayMenu()
    {
      string dbgTag = string.Empty;
      if (Helpers.IsDebugging)
      {
        dbgTag = " (DEBUG)";
      }

      List<ToolStripItem> menuBuilder = new List<ToolStripItem>();
      menuBuilder.Add(new ToolStripMenuItem("Options" + dbgTag, null, OnMenuProperties));
      menuBuilder.Add(new ToolStripSeparator());

      List<ToolStripItem> epMenus = new List<ToolStripItem>();

      // Load add-in menus
      epMenus = LoadMenuFromExtensionPoint();
      if (epMenus.Count > 0)
        menuBuilder.AddRange(epMenus);

      menuBuilder.Add(new ToolStripMenuItem("About", null, OnMenuAbout));
      if (_updates?.IsConfigured == true)
        menuBuilder.Add(new ToolStripMenuItem("Check for updates…", null, async (_, _) => await _updates.CheckAndInstallAsync()));
      menuBuilder.Add(new ToolStripMenuItem("Exit", null, OnMenuExit));

      _trayMenu = menuBuilder.ToArray();
    }

    private List<ToolStripItem> LoadMenuFromExtensionPoint()
    {
      Log.Debug("Entering");

      List<ToolStripItem> addinItems = new List<ToolStripItem>();
      Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPath.SystemTray);

      Log.Debug($"Found '{nodes.Count}' items ...");
      foreach (Mono.Addins.ExtensionNode node in nodes)
      {
        Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;
        try
        {
          SysTrayAddin addin = _addins.CreateAddin(typeNode) as SysTrayAddin;
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

    private void OnMenuAbout(object sender, EventArgs e)
    {
      Form f = new Views.AboutForm();
      f.ShowDialog();
    }

    private void OnMenuDoubleClick(object sender, EventArgs e)
    {
      // Show Properties or About?
      ShowPreferences();
    }

    private void OnMenuExit(object sender, EventArgs e)
    {
      _trayNotify.Dispose();
      Application.Exit();
    }

    private void OnMenuProperties(object sender, EventArgs e)
    {
      ShowPreferences();
    }

    private void OnTrayIconChanged(System.Drawing.Icon icon)
    {
      Log.Debug(icon == null ? "Changing SystemTrayIcon to default" : "Changing SystemTrayIcon to custom");
      var previous = _customIcon;
      _trayNotify.Icon = icon ?? ApplicationIcon;
      _customIcon = icon;
      if (!ReferenceEquals(previous, icon))
        previous?.Dispose();
    }

    private void OnTrayNotification(string message)
    {
      Log.Debug($"Alert bubble msg:'{message}'");
      AlertBubble(string.Empty, message);
    }

    private void ShowPreferences()
    {
      _preferences.Show();
    }
  }
}

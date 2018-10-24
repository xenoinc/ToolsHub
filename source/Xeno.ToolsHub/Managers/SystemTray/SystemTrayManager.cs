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
 *  - gfx https://www.red-gate.com/simple-talk/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Xeno.ToolsHub.Helpers;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  public class SystemTrayManager : ApplicationContext
  {
    private MenuItem[] _trayMenu;
    private NotifyIcon _trayNotify = new NotifyIcon();

    public SystemTrayManager()
    {
      InitTrayMenu();
      RedrawTrayNotifacation();
    }

    private System.Drawing.Icon ApplicationIcon
    {
      get
      { // TODO: Load custom icon here
        return Properties.Resources.AppIcon;
      }
    }

    #region Menu Renderer

    private void LoadAddinsForSysTray()
    {
      string pth = Helpers.ExtensionPaths.SystemTrayPath;
      Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(pth);

      foreach (Mono.Addins.ExtensionNode node in nodes)
      {
        Log.Debug("LoadAddinsForSysTray ...");
        Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;

        // SysTrayAddin
        try
        {
          SysTrayAddin addin = typeNode.CreateInstance() as SysTrayAddin;
          // Keep track of the addins added to each note
          //AttachAddin(type_node.Id, note, n_addin);
        }
        catch (Exception e)
        {
          Log.Debug($"Couldn't create a NoteAddin instance: {e.Message}");
        }
      }
    }

    private void InitTrayMenu()
    {
      List<MenuItem> menuBuilder = new List<MenuItem>();
      menuBuilder.Add(new MenuItem("ToolsHub", OnMenuProperties));
      menuBuilder.Add(new MenuItem("-"));

      // Load add-in menus
      var addinMenus = LoadMenuFromExtensionPoint();
      if (addinMenus.Count > 0)
        menuBuilder.AddRange(addinMenus);

      menuBuilder.Add(new MenuItem("About", OnMenuAbout));
      menuBuilder.Add(new MenuItem("Exit", OnMenuExit));

      _trayMenu = menuBuilder.ToArray();
    }

    /// <summary>Load tray menu items from ExtensionPoint</summary>
    private List<MenuItem> LoadMenuFromExtensionPoint()
    {
      List<MenuItem> addinItems = new List<MenuItem>();

      // TODO:
      //  1. Iterate through extension point nodes
      //  2. Add sub-items

      // foreach (var items in LoadAddinsForSysTray()) { }

      // Dummy data
      var addin1 = new MenuItem("Test Addin-1");
      addin1.MenuItems.Add(0, new SystemTray.TrayItem("SubItem 1", "tag_addin1-Sub1", true));
      addin1.MenuItems.Add(1, new SystemTray.TrayItem("SubItem 2", "tag_addin1-Sub2"));
      addin1.MenuItems.Add(2, new SystemTray.TrayItem("SubItem 3", "tag_addin1-Sub3"));

      var addin2 = new MenuItem("Test Addin-2");
      addin2.MenuItems.Add(new SystemTray.TrayItem("A2: SubItem 1", "tag_addin2-sub1"));
      addin2.MenuItems.Add(new SystemTray.TrayItem("A2: SubItem 2", "tag_addin2-sub2"));

      addinItems.Add(addin1);
      addinItems.Add(addin2);

      return addinItems;
    }

    /// <summary>Redraw systray menu from memory</summary>
    private void RedrawTrayNotifacation()
    {
      _trayNotify.Icon = ApplicationIcon;
      _trayNotify.ContextMenu = new ContextMenu(_trayMenu);
      _trayNotify.DoubleClick += new EventHandler(OnMenuDoubleClick);
      _trayNotify.Visible = true;
    }

    #endregion Menu Renderer

    #region Local Menu - Event Handlers

    private void OnMenuAbout(object sender, EventArgs e)
    {
      throw new NotImplementedException();
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
      throw new NotImplementedException();
    }

    #endregion Local Menu - Event Handlers
  }
}

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
using Xeno.ToolsHub.LocalAddins.Shortcuts;

namespace Xeno.ToolsHub.Managers.SystemTray
{
  public class SystemTrayManager : ApplicationContext
  {
    private MenuItem[] _trayMenu;
    private NotifyIcon _trayNotify = new NotifyIcon();

    public SystemTrayManager()
    {
      Refresh();
    }

    private System.Drawing.Icon ApplicationIcon
    {
      get
      {
        // TODO: Load custom icon here
        return Properties.Resources.AppIcon;
      }
    }

    #region Menu Renderer

    public void Refresh()
    {
      //TODO: Create a singleton or IoC pattern to call Refresh() from outside
      InitTrayMenu();
      RedrawTrayNotifacation();
    }

    private void InitTrayMenu()
    {
      List<MenuItem> menuBuilder = new List<MenuItem>();
      menuBuilder.Add(new MenuItem("ToolsHub", OnMenuProperties));
      menuBuilder.Add(new MenuItem("-"));

      List<MenuItem> epMenus = new List<MenuItem>();

      // Test items
      //epMenus = LoadMenuForTests();
      //if (epMenus.Count > 0)
      //  menuBuilder.AddRange(epMenus);

      // Load Shortcuts from JSON [Obsolete]
      epMenus = LoadMenuShortcuts();
      if (epMenus.Count > 0)
        menuBuilder.AddRange(epMenus);

      // Load add-in menus
      epMenus = LoadMenuFromExtensionPoint();
      if (epMenus.Count > 0)
        menuBuilder.AddRange(epMenus);

      menuBuilder.Add(new MenuItem("About", OnMenuAbout));
      menuBuilder.Add(new MenuItem("Exit", OnMenuExit));

      _trayMenu = menuBuilder.ToArray();
    }

    /// <summary>Load tray menu items from config file</summary>
    /// <remarks>TEMP code only. Future we'll use Addins</remarks>
    [Obsolete("we should be using Add-ins, not manually adding things")]
    private List<MenuItem> LoadMenuShortcuts()
    {
      List<MenuItem> addinItems = new List<MenuItem>();

      ShortcutsSysTray shortcutMenu = new ShortcutsSysTray();
      var subSet = shortcutMenu.LoadAsMenuItems();
      addinItems.Add(subSet);

      return addinItems;
    }

    private List<MenuItem> LoadMenuFromExtensionPoint()
    {
      //TODO: Uncomment below and implement

      List<MenuItem> addinItems = new List<MenuItem>();
      //string pth = Config.ExtensionPaths.SystemTrayPath;
      //Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(pth);
      //
      //Log.Debug($"LoadMenuFromExtensionPoint ({nodes.Count} items found)...");
      //foreach (Mono.Addins.ExtensionNode node in nodes)
      //{
      //  Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;
      //
      //  // SysTrayAddin
      //  try
      //  {
      //    SysTrayAddin addin = typeNode.CreateInstance() as SysTrayAddin;
      //    Log.Debug($"LoadMenuFromExtensionPoint: addin[{addin.ToString()}]");
      //
      //    // Keep track of the addins added to each note
      //    //AttachAddin(type_node.Id, note, n_addin);
      //  }
      //  catch (Exception e)
      //  {
      //    Log.Error($"Couldn't create a NoteAddin instance: {e.Message}");
      //  }
      //}
      //
      return addinItems;
    }

    ///// <summary>Load tray menu items from config file</summary>
    //private List<MenuItem> LoadMenuForTests()
    //{
    //  List<MenuItem> addinItems = new List<MenuItem>();
    //
    //  // TODO:
    //  //  1. Read config file
    //  //  2. Return submenu
    //
    //  // foreach (var items in LoadAddinsForSysTray()) { }
    //
    //  // Dummy data
    //  var addin1 = new MenuItem("Test Manual-1");
    //  addin1.MenuItems.Add(0, new SystemTray.TrayItem("SubItem 1", "tag_addin1-Sub1", true));
    //  addin1.MenuItems.Add(1, new SystemTray.TrayItem("SubItem 2", "tag_addin1-Sub2"));
    //  addin1.MenuItems.Add(2, new SystemTray.TrayItem("SubItem 3", "tag_addin1-Sub3"));
    //
    //  var addin2 = new MenuItem("Test Manual-2");
    //  addin2.MenuItems.Add(new SystemTray.TrayItem("A2: SubItem 1", "tag_addin2-sub1"));
    //  addin2.MenuItems.Add(new SystemTray.TrayItem("A2: SubItem 2", "tag_addin2-sub2"));
    //
    //  addinItems.Add(addin1);
    //  addinItems.Add(addin2);
    //
    //  return addinItems;
    //}

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
      Form p = new Views.PreferencesForm();
      p.Show();
    }

    #endregion Local Menu - Event Handlers
  }
}

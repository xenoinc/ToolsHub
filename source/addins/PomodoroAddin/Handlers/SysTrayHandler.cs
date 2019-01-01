/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    SysTrayHandler.cs
 * Description:
 *  System Tray extension point for Pomodoro add-in
 */

namespace PomodoroAddin.Handlers
{
  using System.Collections.Generic;
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.SysTrayAddin,
    Path = ExtensionPath.SystemTray)]
  public class SysTrayHandler : Xeno.ToolsHub.ExtensionModel.SystemTray.SysTrayAddin
  {
    private PomodoroManager _pomodoro;

    public SysTrayHandler()
    {
      _pomodoro = new PomodoroManager();
      this.IsInitialized = true;
    }

    public override bool IsInitialized { get; }

    public override void Initialize()
    {
    }

    public override List<MenuItem> MenuItems()
    {
      return _pomodoro.MenuItems;
    }
  }
}

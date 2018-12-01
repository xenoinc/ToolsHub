/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-10
 * Author:  Damian Suess
 * File:    ShortcutsAddin.cs
 * Description:
 *  Extends the functionality of a Shortcut.
 *  If you wish to extend ToolsHub in a more broad spectrum, you'll need
 *  to create an ApplicationAddin
 *
 * TODO:
 *  [ ] Consider renaming namespace to ShortcutsAddin and this class to, Shortcuts
 */

using System;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  public class ShortcutsAddin : UtilityAddin
  {
    private bool _initialized = false;

    public override bool IsInitialized => _initialized;

    public override void Initialize()
    {
      Log.Debug("Shortcuts add-in initializing...");
      _initialized = true;
    }

    public override void Execute()
    {
      throw new NotImplementedException();
    }

    public override void Shutdown()
    {
      Log.Debug("Shortcuts add-in shutting down");
      throw new NotImplementedException();
    }
  }
}

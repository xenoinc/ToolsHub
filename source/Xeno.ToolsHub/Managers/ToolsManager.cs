/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    ToolsManager.cs
 * Description:
 *
 */

using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.Managers
{
  public class ToolsManager
  {
    private AddinManager _addinManager;

    public string _toolsDir;

    public ToolsManager() : this ("")
    {
    }

    public ToolsManager(string cfgDirectory)
    {
      Log.Debug($"ToolsManager created path '{cfgDirectory}'");

      _toolsDir = cfgDirectory;
      _addinManager = new AddinManager(cfgDirectory);

    }

    public AddinManager AddinManager { get => _addinManager; }
  }
}

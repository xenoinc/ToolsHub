/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    AddinManager.cs
 * Description:
 *  Manager for system wide add-ins
 */

using System;
using System.Collections.Generic;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.Managers
{
  public class AddinManager
  {
    /// <summary>
    ///   Key = TypeExtensionNode.Id
    /// </summary>
    private Dictionary<string, ApplicationAddin> _appAddins;

    public AddinManager(string configDir)
    {
      _appAddins = new Dictionary<string, ApplicationAddin>();

      InitAddins();
    }

    public event EventHandler OnApplicationAddinListChanged;

    public PreferenceAddin[] GetPreferenceAddins()
    {
      PreferenceAddin[] addins;

      try
      {
        addins = (PreferenceAddin[])Mono.Addins.AddinManager.GetExtensionObjects(
          Config.ExtensionPaths.PreferencePath, typeof(PreferenceAddin));
      }
      catch(Exception ex)
      {
        Log.Warn($"No perferenceAddins found '{ex.Message}'");
        addins = new PreferenceAddin[0];
      }

      return addins;
    }

    private void InitAddins()
    {
      Config.Log.Info("Initialize Mono.Addins");
    }
  }
}

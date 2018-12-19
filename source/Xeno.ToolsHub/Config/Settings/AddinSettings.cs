/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-8-13
 * Author:  Damian Suess
 * File:    Settings.cs
 * Description:
 *  OLD METHOD!  Add-in Settings manager
 *
 *  Current an in-memory test
 */

using System;
using System.Collections.Generic;

namespace Xeno.ToolsHub.Config.Settings
{
  [Obsolete]
  public static class AddinSettings
  {
    private static Dictionary<string, string> _settings = new Dictionary<string, string>();

    public static string Load(string title, string name, string defValue)
    {
      string value = defValue;

      if (_settings.ContainsKey(name))
      {
        value = _settings[name];
      }

      return value;
    }

    public static void Save(string title, string name, string value)
    {
      _settings.Add(name, value);
    }
  }
}

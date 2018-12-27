/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    Properties.cs
 * Description:
 *  Property Collection (PROTYPE TESTING)
 */

namespace Xeno.ToolsHub.Tests.SystemTests.PropertySingleton
{
  using System;
  using System.Collections.Generic;
  using Xeno.ToolsHub.Config;

  public class Properties
  {
    private static Dictionary<string, object> _props = new Dictionary<string, object>();

    private readonly object _mutex;

    public Properties()
    {
      _mutex = new object();
    }

    public T Get<T>(string key, T defaultValue)
    {
      object val;
      if (_props.TryGetValue(key, out val))
      {
        try
        {
          return (T)val;
        }
        catch (Exception ex)
        {
          Log.Warn(ex.Message);
          return defaultValue;
        }
      }
      else
      {
        return defaultValue;
      }
    }

    ////public void Set<T>(string key, T value)
    ////{
    ////  if (value == null)
    ////  {
    ////    // Remove(key);
    ////    return;
    ////  }
    ////
    ////  lock(_mutex)
    ////  {
    ////    object oldValue;
    ////    if (_dict.TryGetValue(key, out oldValue))
    ////    {
    ////      if (object.Equals(serializedValue, oldValue))
    ////        return;
    ////      HandleOldValue(oldValue);
    ////    }
    ////    _dict[key] = serializedValue;
    ////  }
    ////}
  }
}

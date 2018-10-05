/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-3
 * Author:  Damian Suess
 * File:    Sample.cs
 * Description:
 *  Entry point to Sample add-in
 */

using System;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.Helpers;

namespace Xeno.ToolsHub.SampleAddin
{
  public class Sample : ApplicationAddin
  {
    private bool _initialized = false;

    public override bool Initialized => _initialized;

    public override void Initialize()
    {
      _initialized = true;

      Log.Debug("Sample add-in initializing");
      throw new NotImplementedException();
    }

    public override void Shutdown()
    {
      Log.Debug("Sample add-in shutting down");
      throw new NotImplementedException();
    }
  }
}

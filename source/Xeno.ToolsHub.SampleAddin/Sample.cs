/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-3
 * Author:  Damian Suess
 * File:    Sample.cs
 * Description:
 *  Entry point to Sample add-in
 */

using System;
using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleAddin
{
  public class Sample : UtilityAddin
  {
    private bool _initialized = false;

    public override bool Initialized => _initialized;

    public override void Initialize()
    {
      _initialized = true;

      Log.Debug("Sample add-in initializing");
      MessageBox.Show("Samples Init!");

      throw new NotImplementedException();
    }

    public override void Shutdown()
    {
      Log.Debug("Sample add-in shutting down");
      MessageBox.Show("Samples Shutdown!");

      throw new NotImplementedException();
    }
  }
}

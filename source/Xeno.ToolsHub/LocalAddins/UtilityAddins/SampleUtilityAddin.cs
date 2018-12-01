/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-30
 * Author:  Damian Suess
 * File:    SampleUtilityAddin.cs
 * Description:
 *  Sample utility, complete with deconstructor
 */

using System;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.LocalAddins.UtilityAddins
{
  public class SampleUtilityAddin : UtilityAddin
  {
    private bool _initialized = false;

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("Utility Sample (internal) executed");

      //System.Windows.Forms.MessageBox.Show("Utility Sample (internal) executed!");
    }

    public override void Initialize()
    {
      _initialized = true;
      Log.Debug("Utility Sample (internal add-in) initialized");
    }

    public override void Shutdown()
    {
      throw new NotImplementedException();
    }
  }
}

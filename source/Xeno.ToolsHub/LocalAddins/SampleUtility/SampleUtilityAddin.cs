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

namespace Xeno.ToolsHub.LocalAddins.SampleUtility
{
  public class SampleUtilityAddin : UtilityAddin
  {
    private bool _initialized = false;

    public SampleUtilityAddin()
    {
      _initialized = true;
      Log.Debug("SampleXml internal Utility add-in, initialized!");
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("SampleXml internal Utility add-in, executed!");
      //System.Windows.Forms.MessageBox.Show("SampleXml internal Utility add-in, executed!");
    }

    public override void Shutdown()
    {
      throw new NotImplementedException();
    }
  }
}

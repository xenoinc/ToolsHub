/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinUtilityHandler.cs
 * Description:
 *
 */

using System;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace SampleAssmAddin
{
  public class AddinUtilityHandler : UtilityAddin
  {
    private bool _initialized = false;

    public AddinUtilityHandler()
    {
      _initialized = true;
      Log.Debug("SampleAssm external Utility add-in, initialized");

      System.Windows.Forms.MessageBox.Show("SampleAssm external Utility add-in, initialized!");
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("SampleAssm external Utility add-in, executed!");
      System.Windows.Forms.MessageBox.Show("SampleAssm external Utility add-in, executed!");
    }

    public override void Shutdown()
    {
      throw new NotImplementedException();
    }
  }
}

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinUtilityHandler.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.SampleAssmAddin.Handlers
{
  using Xeno.ToolsHub.ExtensionModel;
  using Xeno.ToolsHub.Services.Logging;

  [Mono.Addins.Extension(NodeName = ExtensionName.UtilityAddin, Path = ExtensionPath.Utility)]
  public class UtilityHandler : UtilityAddin
  {
    private bool _initialized = false;

    public UtilityHandler()
    {
      _initialized = true;
      Log.Debug("External SampleAssm Utility add-in: Initialized");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleAssm Utility add-in: Initialized!");
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      Log.Debug("External SampleAssm Utility add-in: Executed!");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleAssm Utility add-in: Executed!");
    }

    public override void Shutdown()
    {
      Log.Debug("External SampleAssm Utility add-in: Shutting down!");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleAssm Utility add-in: Shutting down!");
    }
  }
}

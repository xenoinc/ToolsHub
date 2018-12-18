/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinOnStartHandler.cs
 * Description:
 *  SampleAssembly OnStart extension
 */

using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleAssmAddin.Handlers
{
  [Mono.Addins.Extension(NodeName = ExtensionName.OnStartupAddin, Path = ExtensionPath.OnStartup)]
  public class OnStartHandler : ExtensionModel.IOnStartupExtension
  {
    public OnStartHandler()
    {
      this.Title = "External SampleAssm OnStartup add-in";
      Log.Debug("External SampleAssm OnStartup, initialized");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleAssm OnStartup, initialized!");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("External SampleAssm OnStartup, executed");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleAssm OnStartup, executed!");
    }
  }
}

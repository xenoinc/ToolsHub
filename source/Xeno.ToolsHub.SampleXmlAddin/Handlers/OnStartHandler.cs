/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinOnStartHandler.cs
 * Description:
 *  SampleXml OnStart extension
 */

namespace Xeno.ToolsHub.SampleXmlAddin.Handlers
{
  using Xeno.ToolsHub.Config;

  public class OnStartHandler : ExtensionModel.IOnStartupExtension
  {
    public OnStartHandler()
    {
      this.Title = "External SampleXml OnStart add-in";
      Log.Debug("External SampleXml OnStart add-in: Initialized!");

      ////if (Helpers.IsDebugging)
      ////  System.Windows.Forms.MessageBox.Show("External SampleXml OnStart add-in: Initialized!");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("External SampleXml OnStart add-in: Executed");

      ////if (Helpers.IsDebugging)
      ////  System.Windows.Forms.MessageBox.Show("External SampleXml OnStart add-in: Executed!");
    }
  }
}

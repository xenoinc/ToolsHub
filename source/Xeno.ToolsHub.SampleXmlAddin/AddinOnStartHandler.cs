/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinOnStartHandler.cs
 * Description:
 *  SampleXml OnStart extension
 */

using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public class AddinOnStartHandler : ExtensionModel.IOnStartupExtension
  {
    public AddinOnStartHandler()
    {
      this.Title = "External SampleXml OnStart add-in";
      Log.Debug("External SampleXml OnStart add-in: Initialized!");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleXml OnStart add-in: Initialized!");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("External SampleXml OnStart add-in: Executed");

      //if (Helpers.IsDebugging)
      //  System.Windows.Forms.MessageBox.Show("External SampleXml OnStart add-in: Executed!");
    }
  }
}

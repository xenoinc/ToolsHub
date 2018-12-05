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
      this.Title = "SampleXml external OnStart add-in";
      Log.Debug("SampleXml external OnStart, initialized");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("SampleXml external OnStart, executed");

      //System.Windows.Forms.MessageBox.Show("OnStart Sample (internal add-in) executed!");
    }
  }
}

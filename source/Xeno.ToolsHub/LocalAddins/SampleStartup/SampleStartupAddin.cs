/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-29
 * Author:  Damian Suess
 * File:    SampleStartupAddin.cs
 * Description:
 *  Sample OnStartup add-in
 */

using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.LocalAddins.SampleStartup
{
  public class SampleStartupAddin : ExtensionModel.IOnStartupExtension
  {
    public SampleStartupAddin()
    {
      this.Title = "SampleXml internal OnStartup Add-In";
      Log.Debug("SampleXml internal OnStartup, initialized!");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("SampleXml internal OnStartup, executed!");
      //System.Windows.Forms.MessageBox.Show("SampleXml internal OnStartup, executed!");
    }
  }
}

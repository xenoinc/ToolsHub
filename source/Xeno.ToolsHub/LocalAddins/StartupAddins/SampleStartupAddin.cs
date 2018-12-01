/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-29
 * Author:  Damian Suess
 * File:    SampleStartupAddin.cs
 * Description:
 *  Sample OnStartup add-in
 */

using Xeno.ToolsHub.Config;

namespace Xeno.ToolsHub.LocalAddins.StartupAddins
{
  public class SampleStartupAddin : ExtensionModel.IOnStartupExtension
  {
    public SampleStartupAddin()
    {
      this.Title = "Sample Startup Add-In";
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("OnStart Sample (internal) executed");

      //System.Windows.Forms.MessageBox.Show("OnStart Sample (internal add-in) executed!");
    }
  }
}

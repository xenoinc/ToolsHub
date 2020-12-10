/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-29
 * Author:  Damian Suess
 * File:    SampleStartupAddin.cs
 * Description:
 *  Sample OnStartup add-in
 */

namespace Xeno.ToolsHub.LocalAddins.SampleStartup
{
  using Xeno.ToolsHub.Services.Logging;

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
    }
  }
}

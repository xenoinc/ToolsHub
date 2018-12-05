/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-3
 * Author:  Damian Suess
 * File:    SampleXmlUtility.cs
 * Description:
 *  Sample Utility add-in
 */

using System.Windows.Forms;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public class SampleXmlUtility : UtilityAddin
  {
    private bool _initialized = false;

    public SampleXmlUtility()
    {
      _initialized = true;

      Log.Debug("SampleXml external utility add-in initializing");
    }

    public override bool IsInitialized => _initialized;

    public override void Execute()
    {
      MessageBox.Show("SampleXml external utility add-in Executed!");
    }

    public override void Shutdown()
    {
      Log.Debug("SampleXml external utility add-in Shutting Down");
      // MessageBox.Show("Samples Shutdown!");
    }
  }
}

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferencePageHandler.cs
 * Description:
 *
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleAssmAddin
{
  [Mono.Addins.Extension(NodeName = ExtensionName.PreferencePageAddin, Path = ExtensionPath.PreferencePage)]
  public class PreferencePageHandler : PreferencePageExtension
  {
    public override string Title { get { return "Sample Ext-Assm"; } }

    public override UserControl InitializePage()
    {
      return new PreferencePageControl();
    }
  }
}
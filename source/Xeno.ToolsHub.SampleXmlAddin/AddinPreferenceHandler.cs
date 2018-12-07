/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-7
 * Author:  Damian Suess
 * File:    AddinPreferenceHandler.cs
 * Description:
 *
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public class AddinPreferenceHandler : PreferencePageExtension
  {
    public override string Title { get { return "Sample Ext-XML"; } }

    public override UserControl InitializePage()
    {
      return new PreferencePageCtrl();
    }
  }
}

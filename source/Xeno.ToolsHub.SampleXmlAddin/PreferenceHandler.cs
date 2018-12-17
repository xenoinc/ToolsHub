/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-7
 * Author:  Damian Suess
 * File:    AddinPreferenceHandler.cs
 * Description:
 *  Sample XML Add-in Preference Page handler
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleXmlAddin
{
  public class PreferenceHandler : PreferencePageExtension
  {
    private PreferencePageCtrl _page;

    public override string Title { get { return "Sample Ext-XML"; } }

    public override Control Page => _page;

    public override bool IsModified => _page.IsModified;

    public override void InitializePage()
    {
      _page = new PreferencePageCtrl();
    }

    public override void OnSave()
    {
      _page.OnSave();
    }
  }
}

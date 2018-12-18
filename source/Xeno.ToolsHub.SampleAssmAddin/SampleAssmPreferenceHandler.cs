/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferencePageHandler.cs
 * Description:
 *
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.SampleAssmAddin
{
  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class SampleAssmPreferenceHandler : PreferencePageExtension
  {
    private SampleAssmPreferencesPage _page;

    public override Form Page => _page;

    public override string Title { get { return "Sample Ext-Assm"; } }

    public override bool IsModified => _page.IsModified;

    public override void InitializePage()
    {
      _page = new SampleAssmPreferencesPage();
    }

    public override void OnSave()
    {
      _page.OnSave();
    }
  }
}
/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sample Preference Page handler
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.SampleAssmAddin.Views;

namespace Xeno.ToolsHub.SampleAssmAddin.Handlers
{
  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class PreferenceHandler : IPreferencePageExtension
  {
    private PreferencesPage _page;

    public string Id { get; set; }

    public Form Page => _page;

    public string Title { get { return "Sample Ext-Assm"; } }

    public bool IsModified => _page.IsModified;

    public void InitializePage()
    {
      _page = new PreferencesPage();
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}
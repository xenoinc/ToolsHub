/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sidebar prefreferences add-in
 */

using Xeno.ToolsHub.SidebarAddin.Views;

namespace Xeno.ToolsHub.SidebarAddin.Handlers
{
  using System.Windows.Forms;
  using Xeno.ToolsHub.ExtensionModel;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class PreferenceHandler : IPreferencePageExtension
  {
    private PreferencesPage _page;

    public string Id { get; set; }

    public Form Page => _page;

    public string Title { get { return "Sidebar"; } }

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

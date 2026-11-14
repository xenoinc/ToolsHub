/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sidebar prefreferences add-in
 */

using Xeno.Dockzi.SidebarAddin.Views;

namespace Xeno.Dockzi.SidebarAddin.Handlers
{
  using System.Windows.Forms;
  using Xeno.Dockzi.ExtensionModel;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class PreferenceHandler : IPreferencePageExtension
  {
    private readonly PreferencesPage _page;

    public PreferenceHandler(PreferencesPage page)
    {
      _page = page;
    }

    public string Id { get; set; }

    public bool IsModified => _page.IsModified;

    public Form Page => _page;

    public string Title => "Sidebar";

    public void InitializePage()
    {
      // Page dependencies and lifetime are supplied by the dialog scope.
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}

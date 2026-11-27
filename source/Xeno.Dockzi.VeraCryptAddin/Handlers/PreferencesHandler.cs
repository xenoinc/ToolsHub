/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCryptPreferencesHandler.cs
 * Description:
 *  VeraCrypt Add-in Preferences Handler
 */

using System.Windows.Forms;
using Xeno.Dockzi.ExtensionModel;

namespace Xeno.Dockzi.VeraCryptAddin.Handlers;

// We're using an "addin.xml" manifest
////[Mono.Addins.Extension(
////  NodeName = ExtensionName.PreferencePageAddin,
////  Path = ExtensionPath.PreferencePage)]
public class PreferencesHandler : IPreferencePageExtension
{
  private readonly Views.PreferencesPage _page;

  public PreferencesHandler(Views.PreferencesPage page)
  {
    _page = page;
  }

  public string Id { get; set; }

  public bool IsModified => _page.IsModified;

  public Form Page => _page;

  public string Title => "VeraCrypt";

  public void InitializePage()
  {
    // Page dependencies and lifetime are supplied by the dialog scope.
  }

  public void OnSave()
  {
    _page.OnSave();
  }
}

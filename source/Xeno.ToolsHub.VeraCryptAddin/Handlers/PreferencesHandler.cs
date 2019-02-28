/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCryptPreferencesHandler.cs
 * Description:
 *  VeraCrypt Add-in Preferences Handler
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.VeraCryptAddin.Handlers
{
  // We're using an "addin.xml" manifest
  ////[Mono.Addins.Extension(
  ////  NodeName = ExtensionName.PreferencePageAddin,
  ////  Path = ExtensionPath.PreferencePage)]
  public class PreferencesHandler : IPreferencePageExtension
  {
    private Views.PreferencesPage _page;

    public string Title => "VeraCrypt";

    public Form Page => _page;

    public bool IsModified => _page.IsModified;

    public string Id { get; set; }

    public void InitializePage()
    {
      _page = new Views.PreferencesPage();
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}

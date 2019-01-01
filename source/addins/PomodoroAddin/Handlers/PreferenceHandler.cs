/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *
 */

namespace PomodoroAddin.Handlers
{
  using Xeno.ToolsHub.ExtensionModel;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class PreferenceHandler : IPreferencePageExtension
  {
    private Views.PreferencePage _page;

    public string Id { get; set; }

    public bool IsModified => _page.IsModified;

    public System.Windows.Forms.Form Page => throw new System.NotImplementedException();

    public string Title => throw new System.NotImplementedException();

    public void InitializePage()
    {
      throw new System.NotImplementedException();
    }

    public void OnSave()
    {
      throw new System.NotImplementedException();
    }
  }
}

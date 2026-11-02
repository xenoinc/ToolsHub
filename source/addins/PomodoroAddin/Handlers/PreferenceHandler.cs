/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *
 */

namespace PomodoroAddin.Handlers
{
  using System.Windows.Forms;
  using PomodoroAddin.Views;
  using Xeno.ToolsHub.ExtensionModel;

  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public class PreferenceHandler : IPreferencePageExtension
  {
    private readonly PreferencePage _page;

    public PreferenceHandler(PreferencePage page)
    {
      _page = page;
    }

    public string Id { get; set; }

    public bool IsModified => _page.IsModified;

    public Form Page => _page;

    public string Title { get { return "Pomodoro"; } }

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

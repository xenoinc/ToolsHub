/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-11
 * Author:  Damian Suess
 * File:    PreferencesHandler.cs
 * Description:
 *  Add-in handler for PreferencePage
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs
{
  public class PreferencesHandler : IPreferencePageExtension
  {
    private ShortcutsPreferences _page;

    public string Id { get; set; }

    public string Title => "Shortcuts";

    public Form Page => _page;

    public bool IsModified => _page.IsModified;

    public void InitializePage()
    {
      _page = new ShortcutsPreferences();
    }

    public void OnSave()
    {
      _page.OnSave(); // Save settings was initiated
    }
  }
}

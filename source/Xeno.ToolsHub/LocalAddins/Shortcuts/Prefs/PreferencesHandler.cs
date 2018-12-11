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
  public class PreferencesHandler : PreferencePageExtension
  {
    private PreferencePageCtrl _page;

    public override string Title => "Shortcuts";

    public override Control Page => _page;

    public override UserControl InitializePage()
    {
      _page = new PreferencePageCtrl();
      return _page;
    }

    public override void OnSave()
    {
      _page.Save(); // Save settings was initiated
    }
  }
}

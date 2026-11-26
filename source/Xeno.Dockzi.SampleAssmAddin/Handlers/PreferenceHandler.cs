/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sample Preference Page handler
 */

using System.Windows.Forms;
using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.SampleAssmAddin.Views;

namespace Xeno.Dockzi.SampleAssmAddin.Handlers
{
  [Mono.Addins.Extension(
    NodeName = ExtensionName.PreferencePageAddin,
    Path = ExtensionPath.PreferencePage)]
  public sealed class PreferenceHandler : IPreferencePageExtension, System.IDisposable
  {
    private PreferencesPage _page;

    public string Id { get; set; }

    public Form Page => _page;

    public string Title => "Sample Ext-Assm";

    public bool IsModified => _page.IsModified;

    public void Dispose() => _page?.Dispose();

    public void InitializePage()
    {
      _page?.Dispose();
      _page = new PreferencesPage();
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}

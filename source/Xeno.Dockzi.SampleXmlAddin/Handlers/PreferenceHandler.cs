/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-7
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sample XML Add-in Preference Page handler
 */

using System.Windows.Forms;
using Xeno.Dockzi.ExtensionModel;
using Xeno.Dockzi.SampleXmlAddin.Views;

namespace Xeno.Dockzi.SampleXmlAddin.Handlers
{
  public sealed class PreferenceHandler : IPreferencePageExtension, System.IDisposable
  {
    private PerferencePage _page;

    public string Id { get; set; }

    public Form Page => _page;

    public string Title => "Sample Ext-XML";

    public bool IsModified => _page.IsModified;

    public void Dispose() => _page?.Dispose();

    public void InitializePage()
    {
      _page?.Dispose();
      _page = new PerferencePage();
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}

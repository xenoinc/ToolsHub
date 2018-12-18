/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-7
 * Author:  Damian Suess
 * File:    PreferenceHandler.cs
 * Description:
 *  Sample XML Add-in Preference Page handler
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.SampleXmlAddin.Views;

namespace Xeno.ToolsHub.SampleXmlAddin.Handlers
{
  public class PreferenceHandler : IPreferencePageExtension
  {
    private PerferencePage _page;

    public string Id { get; set; }

    public Form Page => _page;

    public string Title => "Sample Ext-XML";

    public bool IsModified => _page.IsModified;

    public void InitializePage()
    {
      _page = new PerferencePage();
    }

    public void OnSave()
    {
      _page.OnSave();
    }
  }
}

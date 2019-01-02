/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCryptPreferencesHandler.cs
 * Description:
 *  VeraCrypt Add-in Preferences Handler
 */

using System.Windows.Forms;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public class VeraCryptPreferencesHandler : IPreferencePageExtension
  {
    public string Title => throw new System.NotImplementedException();

    public Form Page => throw new System.NotImplementedException();

    public bool IsModified => throw new System.NotImplementedException();

    public string Id { get; set; }

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

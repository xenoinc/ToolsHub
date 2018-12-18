/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    IPreferencePageForm.cs
 * Description:
 *  Preference page form interface. Not required, but for best-practice
 */

namespace Xeno.ToolsHub.ExtensionModel.Preferences
{
  public interface IPreferencePageForm
  {
    bool IsModified { get; }

    void OnSave();
  }
}

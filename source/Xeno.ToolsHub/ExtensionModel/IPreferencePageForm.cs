/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    IPreferencePageForm.cs
 * Description:
 *  Preference page form interface. Not required, but for best-practice
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public interface IPreferencePageForm
  {
    bool IsModified { get; }

    bool OnSave();

    /// <summary>Save preference page</summary>
    /// <param name="cancelFlag">If true, request to cancel the save process or discard faulted changes</param>
    //void OnSave2(out bool cancelFlag = false, out string message);
  }
}

﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-17
 * Author:  Damian Suess
 * File:    IPreferencePageActions.cs
 * Description:
 *  Preference page form interface. Not required, but for best-practice
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public interface IPreferencePageActions
  {
    bool IsModified { get; set; }

    /// <summary>User clicked Save in parent Preference window.</summary>
    /// <returns>
    ///   True tell parent this page successfully save.
    ///   False informs parent a child page failed to save and needs attention.
    /// </returns>
    bool OnSave();

    /////// <summary>Save preference page</summary>
    /////// <param name="errorMessage">Error message to display.</param>
    /////// <returns>Success of save. When false, request to cancel the save process or discard faulted changes.</returns>
    ////bool OnSave(out string errorMessage);
  }
}

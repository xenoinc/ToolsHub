/* Copyright Xeno Innovations, Inc. 2020
 * Date:    2020-10-15
 * Author:  Damian Suess
 * File:    BasePreferencePageActions.cs
 * Description:
 *  Preference page base-page helper
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public class BasePreferencePageActions : IPreferencePageActions
  {
    public const bool Success = true;
    public const bool Failure = false;

    public virtual bool IsModified { get; set; }

    public virtual bool OnSave()
    {
      return Success;
    }
  }
}

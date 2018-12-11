/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    PreferencePageExtension.cs
 * Description:
 *
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel
{
  public abstract class PreferencePageExtension
  {
    public string Id;

    public abstract string Title { get; }

    public abstract Control Page { get; }

    public abstract System.Windows.Forms.UserControl InitializePage();

    public abstract void OnSave();

    //public abstract bool GetPreferenceAddin(object parentDialog,
    //                                        out string titleText,
    //                                        out System.Windows.Forms.Panel preferencePanel);
  }
}

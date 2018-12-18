/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    PreferencePageExtension.cs
 * Description:
 *  Interface for preference pages
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel.Preferences
{
  public abstract class PreferencePageExtension
  {
    public string Id;

    /// <summary>Page title</summary>
    public abstract string Title { get; }

    /// <summary>Preference Page</summary>
    public abstract Form Page { get; }

    /// <summary>Used by the OnSave feature to trigger if we need to save page changes or not</summary>
    public abstract bool IsModified { get; }

    /// <summary>Load the page into memory.</summary>
    /// <remarks>
    ///   We don't load the form during the constructor so that we can save
    ///   on memory. We want QUICK things
    /// </remarks>
    public abstract void InitializePage();

    /// <summary>Save your page's settings (if IsModified). Triggered by the main preference window's OK button</summary>
    public abstract void OnSave();

    //public abstract bool GetPreferenceAddin(object parentDialog,
    //                                        out string titleText,
    //                                        out System.Windows.Forms.Panel preferencePanel);
  }
}

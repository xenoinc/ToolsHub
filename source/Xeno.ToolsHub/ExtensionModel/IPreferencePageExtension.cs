/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    PreferencePageExtension.cs
 * Description:
 *  Interface for preference pages
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.ExtensionModel
{
  public interface IPreferencePageExtension
  {
    string Id { get; set; }

    /// <summary>Used by the OnSave feature to trigger if we need to save page changes or not</summary>
    bool IsModified { get; }

    /// <summary>Preference Page</summary>
    Form Page { get; }

    /// <summary>Page title</summary>
    string Title { get; }

    /// <summary>Load the page into memory.</summary>
    /// <remarks>
    ///   We don't load the form during the constructor so that we can save
    ///   on memory. We want QUICK things
    /// </remarks>
    void InitializePage();

    /// <summary>Save your page's settings (if IsModified). Triggered by the main preference window's OK button</summary>
    void OnSave();

    //bool GetPreferenceAddin(object parentDialog,
    //                        out string titleText,
    //                        out System.Windows.Forms.Panel preferencePanel);
  }
}

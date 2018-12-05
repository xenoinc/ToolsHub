﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-23
 * Author:  Damian Suess
 * File:    PreferencePageExtension.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public abstract class PreferencePageExtension : AbstractAddin
  {
    public abstract bool GetPreferenceAddin(object parentDialog,
                                            out string titleText,
                                            out System.Windows.Forms.Panel preferencePanel);
  }
}
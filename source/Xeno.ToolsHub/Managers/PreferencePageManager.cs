﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    PreferencePageManager.cs
 * Description:
 *
 */

using System;
using System.Collections.Generic;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;
using Xeno.ToolsHub.ExtensionModel.Preferences;

namespace Xeno.ToolsHub.Managers
{
  public class PreferencePageManager
  {
    private Dictionary<string, PreferencePageExtension> _preferencePages;

    public PreferencePageManager()
    {
      _preferencePages = new Dictionary<string, PreferencePageExtension>();

      ReloadPages();
    }

    public void ReloadPages()
    {
      _preferencePages.Clear();

      Mono.Addins.ExtensionNodeList nodes = Mono.Addins.AddinManager.GetExtensionNodes(ExtensionPath.PreferencePage);
      foreach (Mono.Addins.ExtensionNode node in nodes)
      {
        Mono.Addins.TypeExtensionNode typeNode = node as Mono.Addins.TypeExtensionNode;

        try
        {
          //BROKEN: 2018-12-05
          //PreferencePageExtension instance = typeNode.CreateInstance() as PreferencePageExtension;
          //InitPage(instance, typeNode.Id, typeNode.TypeName);
        }
        catch (Exception ex)
        {
          Log.Error($"Couldn't initialize PreferencePage id '{typeNode.Id}': " + ex.Message);
        }
      }
    }

    //BROKEN: 2018-12-05
    //private void InitPage(PreferencePageExtension page, string addinId, string addinTypeName)
    //{
    //  try
    //  {
    //    var ctrl = page.InitializePage();
    //    _preferencePages.Add(addinId, page);
    //
    //    //var pageTitle = string.Empty;
    //    //var pagePanel = new System.Windows.Forms.Panel();
    //    //var parentDialog = new object();
    //    //var result = page.GetPreferenceAddin(parentDialog, out pageTitle, out pagePanel);
    //
    //    if (result)
    //    {
    //      Log.Debug("PreferenePage add-in initialized.");
    //    }
    //    else
    //    {
    //      Log.Debug($"PreferenePage '{addinId}' failed to fully initial.");
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    Log.Error($"Error while attempting to initialize UtilityAddin, Id: '{addinId}', TypeName: '{addinTypeName}': {ex.Message}");
    //  }
    //}
  }
}

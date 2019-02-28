/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCrypt.cs
 * Description:
 *  Entry point to VeraCrypt add-in
 */

using System;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Messaging;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public class VeraCryptManager
  {
    public VeraCryptManager()
    {
      SettingHcDrive = SettingsService.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z");
      SettingHcPath = SettingsService.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty);
      SettingHcPass = SettingsService.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty);
      SettingOnStartMount = SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false);
      SettingOnExitDismount = SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false);
    }

    public string SettingHcDrive { get; set; }

    public string SettingHcPath { get; set; }

    public string SettingHcPass { get; set; }

    public bool SettingOnStartMount { get; set; }

    public bool SettingOnExitDismount { get; set; }

    ////public static void SettingSave(string setting, string value)
    ////{
    ////  Xeno.ToolsHub.Services.SettingsService.SetValue("VeraCrypt", setting, value);
    ////}
    ////
    ////public static string SettingLoad(string setting, string defaultValue)
    ////{
    ////  return Xeno.ToolsHub.Services.SettingsService.GetValue("VeraCrypt", setting, defaultValue);
    ////}

    public int OnMount(string target)
    {
      throw new NotImplementedException();
      return 0;
    }

    public int OnDismount(string target)
    {
      throw new NotImplementedException();
      return 0;
    }

    /// <summary>Inform parent objects to refresh</summary>
    public void RefreshSysTray()
    {
      // TODO: Inform parents to refresh (SysTray/Sidebar)
      MessagingCenter.Send<SystemTrayMessages>(new SystemTrayMessages(), SystemTrayMessages.Refresh);
    }
  }
}

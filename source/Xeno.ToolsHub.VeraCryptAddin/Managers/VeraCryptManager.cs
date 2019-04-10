/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCrypt.cs
 * Description:
 *  Entry point to VeraCrypt add-in
 */

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel.SystemTray;
using Xeno.ToolsHub.Services;
using Xeno.ToolsHub.Services.Logging;
using Xeno.ToolsHub.Services.Messaging;
using Xeno.ToolsHub.VeraCryptAddin.Domain;

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public class VeraCryptManager
  {
    public VeraCryptManager()
    {
      // SettingInstallPath = SettingsService.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe");
      // SettingsForceDismounts = SettingsService.GetBool(Constants.AddinId, Constants.KeyForceDismounts, false);
      // SettingHcDrive = SettingsService.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z");
      // SettingHcPath = SettingsService.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty);
      // SettingHcPass = SettingsService.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty);
      // Not in use
      // SettingOnStartMount = SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false);
      // SettingOnExitDismount = SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false);
    }

    public bool IsValidInstall
    {
      get
      {
        var pth = SettingsService.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe");
        return System.IO.File.Exists(pth) ? true : false;
      }
    }

    public string SettingInstallPath => SettingsService.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe");

    public bool SettingsForceDismounts => SettingsService.GetBool(Constants.AddinId, Constants.KeyForceDismounts, false);

    public string SettingHcDrive => SettingsService.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z");

    public string SettingHcPath => SettingDecrypt(SettingsService.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty));

    public string SettingHcPass => SettingDecrypt(SettingsService.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty));

    public bool SettingOnStartMount => SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false);

    public bool SettingOnExitDismount => SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false);

    /// <summary>Get VeraCrypt's install path</summary>
    /// <returns>Path of installed VeraCrypt.exe or empty</returns>
    public string GetInstallPath()
    {
      string pth = string.Empty;

      try
      {
        // "C:\Program Files\VeraCrypt\VeraCrypt Setup.exe" /u
        string keyPth = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\VeraCrypt";
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPth))
        {
          if (key != null)
          {
            var o = key.GetValue("DisplayIcon");
            if (o != null)
            {
              string data = o as string;
              data = data.Replace("\"", string.Empty);
              data = data.Replace("VeraCrypt Setup.exe", "VeraCrypt.exe");
              pth = data.Trim();
            }
          }
        }

        // If it's shit, discard it
        if (!System.IO.File.Exists(pth))
          pth = string.Empty;
      }
      catch (Exception ex)
      {
        Log.Error($"Issue reading registry key. Error: {ex.Message}");
      }

      return pth;
    }

    /// <summary>Get list of drives not in use</summary>
    /// <returns>Available drives</returns>
    public List<string> GetAvailableDrives()
    {
      List<string> availDrives = new List<string>();

      // pre-load A-Z
      for (char c = 'A'; c <= 'Z'; c++)
        availDrives.Add(c.ToString());

      DriveInfo[] drivesInUse = DriveInfo.GetDrives();
      foreach (DriveInfo d in drivesInUse)
      {
        string name = d.Name;
        name = name.Replace(@":\", string.Empty);
        availDrives.Remove(name);
      }

      return availDrives;
    }

    /// <summary>Mount drive</summary>
    /// <param name="targetIndexId">Index id of mount in settings db</param>
    /// <returns>0 on success</returns>
    public int OnMount(string targetIndexId)
    {
      if (!IsValidInstall)
      {
        Log.Warn($"Unable to mount. VeraCrypt install path is invalid '{SettingInstallPath}'");
        return -1;
      }

      // 1. Use VolumeInfo lookup based on targetIndexId
      // 2. Validate if it's already mounted or not
      // 3. Update SysTray menu item's checkbox with mount state
      string drive = SettingHcDrive;
      string path = SettingHcPath;
      string pass = SettingHcPass;

      VirtualDrive mnt = new VirtualDrive(SettingInstallPath, drive, path, pass);
      mnt.IsSilent = false;
      int ret = mnt.Mount();

      return 0;
    }

    /// <summary>Dismount VeraCrypt virtual drive</summary>
    /// <param name="targetIndexId">Mounted drive id</param>
    /// <returns>meh</returns>
    public int OnDismount(string targetIndexId)
    {
      if (!IsValidInstall)
      {
        Log.Warn($"Unable to dismount. VeraCrypt install path is invalid '{SettingInstallPath}'");
        return -1;
      }

      // 1. Use VolumeInfo lookup based on targetIndexId
      // 2. Validate if it's already mounted or not
      // 3. Update SysTray menu item's checkbox with mount state
      string drive = SettingHcDrive;
      string path = SettingHcPath;
      string pass = SettingHcPass;

      VirtualDrive mnt = new VirtualDrive(SettingInstallPath, drive, path);
      mnt.IsSilent = false;
      int ret = mnt.Dismount(SettingsForceDismounts);

      return 0;
    }

    /// <summary>Inform parent objects to refresh</summary>
    public void RefreshSysTray()
    {
      // TODO: Inform parents to refresh (SysTray/Sidebar)
      MessagingCenter.Send<SystemTrayMessages>(new SystemTrayMessages(), SystemTrayMessages.Refresh);
    }

    public string SettingDecrypt(string setting)
    {
      var crypto = new Crypto();
      var data = crypto.Decrypt(setting);
      return data;
    }

    public string SettingEncrypt(string setting)
    {
      var crypto = new Crypto();
      var data = crypto.Encrypt(setting);
      return data;
    }
  }
}
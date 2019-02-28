/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCrypt.cs
 * Description:
 *  Entry point to VeraCrypt add-in
 */

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

    public string SettingInstallPath { get => SettingsService.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe"); }

    public bool SettingsForceDismounts { get => SettingsService.GetBool(Constants.AddinId, Constants.KeyForceDismounts, false); }

    public string SettingHcDrive { get => SettingsService.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z"); }

    public string SettingHcPath { get => SettingsService.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty); }

    public string SettingHcPass { get => SettingsService.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty); }

    public bool SettingOnStartMount { get => SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false); }

    public bool SettingOnExitDismount { get => SettingsService.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false); }

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
  }
}
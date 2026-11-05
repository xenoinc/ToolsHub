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
using Lite.EventIpc;
using Microsoft.Win32;
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Services.Logging;
using Xeno.Dockzi.VeraCryptAddin.Services;

namespace Xeno.Dockzi.VeraCryptAddin;

public class VeraCryptManager
{
  private readonly ISettingsService _settings;
  private readonly IEventAggregator _events;
  private readonly IVeraCryptOperations _operations;
  private readonly ISecretProtector _secrets;

  public VeraCryptManager(ISettingsService settings, IEventAggregator events, IVeraCryptOperations operations, ISecretProtector secrets)
  {
    _settings = settings;
    _events = events;
    _operations = operations;
    _secrets = secrets;
  }

  public bool IsValidInstall => _operations.IsInstalled(SettingInstallPath);

  public string SettingInstallPath => _settings.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe");

  public bool SettingsForceDismounts => _settings.GetBool(Constants.AddinId, Constants.KeyForceDismounts, false);

  public string SettingHcDrive => _settings.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z");

  public string SettingHcPath => SettingDecrypt(_settings.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty));

  public string SettingHcPass => SettingDecrypt(_settings.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty));

  public bool SettingOnStartMount => _settings.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false);

  public bool SettingOnExitDismount => _settings.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false);

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

    return _operations.Mount(SettingInstallPath, drive, path, pass);
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
    return _operations.Dismount(SettingInstallPath, drive, path, SettingsForceDismounts);
  }

  /// <summary>Inform parent objects to refresh</summary>
  public void RefreshSysTray()
  {
    // TODO: Inform parents to refresh (SysTray/Sidebar)
    _events.Publish(new SystemTrayRefreshRequest());
  }

  public string SettingDecrypt(string setting) => _secrets.Decrypt(setting);

  public string SettingEncrypt(string setting) => _secrets.Encrypt(setting);
}

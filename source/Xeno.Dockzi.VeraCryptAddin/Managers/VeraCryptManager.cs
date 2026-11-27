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
using System.Linq;
using Lite.EventIpc;
using Microsoft.Win32;
using Xeno.Dockzi.ExtensionModel.SystemTray;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Services.Logging;
using Xeno.Dockzi.VeraCryptAddin.Services;

namespace Xeno.Dockzi.VeraCryptAddin;

public class VeraCryptManager(
  ISettingsService settings,
  IEventAggregator events,
  IVeraCryptOperations operations,
  ISecretProtector secrets)
{
  private readonly IEventAggregator _events = events;
  private readonly IVeraCryptOperations _operations = operations;
  private readonly ISecretProtector _secrets = secrets;
  private readonly ISettingsService _settings = settings;

  public bool CanDismount => IsValidInstall && IsMounted(ConfiguredDriveRoot);

  public bool CanMount => IsValidInstall && ConfiguredDriveRoot != null
    && !string.IsNullOrWhiteSpace(SettingHcPath) && !_operations.IsDriveInUse(ConfiguredDriveRoot);

  public string ConfiguredDriveRoot => VeraCryptOperations.NormalizeDriveRoot(SettingHcDrive);

  public bool IsValidInstall => _operations.IsInstalled(SettingInstallPath);

  public string SettingHcDrive => _settings.GetString(Constants.AddinId, Constants.KeyHcDriveLetter, "Z");

  public string SettingHcPass => SettingDecrypt(_settings.GetString(Constants.AddinId, Constants.KeyHcPass, string.Empty));

  public string SettingHcPath => SettingDecrypt(_settings.GetString(Constants.AddinId, Constants.KeyHcPath, string.Empty));

  public string SettingInstallPath => _settings.GetString(Constants.AddinId, Constants.KeyInstallPath, @"C:\Program Files\VeraCrypt\VeraCrypt.exe");

  public bool SettingOpenExplorerAfterMount => _settings.GetBool(Constants.AddinId, Constants.KeyOpenExplorerAfterMount, false);

  public bool SettingOnExitDismount => _settings.GetBool(Constants.AddinId, Constants.KeyHcOnExitDismount, false);

  public bool SettingOnStartMount => _settings.GetBool(Constants.AddinId, Constants.KeyHcOnStartMount, false);

  public bool SettingsForceDismounts => _settings.GetBool(Constants.AddinId, Constants.KeyForceDismounts, false);

  public static List<string> GetMountedVeraCryptDrives() => new(new VeraCryptOperations().GetMountedDrives());

  /// <summary>Get list of drives not in use.</summary>
  /// <returns>Available drives.</returns>
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

  public IReadOnlyList<string> GetMountedDrives() => _operations.GetMountedDrives();

  /// <summary>Dismount VeraCrypt virtual drive.</summary>
  /// <param name="targetIndexId">Mounted drive id.</param>
  /// <returns>meh</returns>
  public int OnDismount(string targetIndexId)
  {
    if (!CanDismount)
    {
      Log.Warn("Unable to dismount: the configured drive is not a mounted VeraCrypt drive or the installation is unavailable.");
      return -1;
    }

    string drive = ConfiguredDriveRoot[..1];
    string path = SettingHcPath;
    return _operations.Dismount(SettingInstallPath, drive, path, SettingsForceDismounts);
  }

  /// <summary>Mount drive</summary>
  /// <param name="targetIndexId">Index id of mount in settings db.</param>
  /// <returns>0 on success</returns>
  public int OnMount(string targetIndexId)
  {
    if (!CanMount)
    {
      Log.Warn("Unable to mount: the configured drive is occupied, the volume path is empty, or the installation is unavailable.");
      return -1;
    }

    string root = ConfiguredDriveRoot;
    string drive = root[..1];
    string path = SettingHcPath;
    string pass = SettingHcPass;
    int result = _operations.Mount(SettingInstallPath, drive, path, pass);
    if (result == 0 && SettingOpenExplorerAfterMount)
      OnOpenInExplorer(root);

    return result;
  }

  public int OnOpenInExplorer(string drive)
  {
    string root = VeraCryptOperations.NormalizeDriveRoot(drive);
    if (!IsMounted(root))
      return -1;

    try
    {
      _operations.OpenInExplorer(root);
      return 0;
    }
    catch (Exception ex)
    {
      Log.Warn("Unable to open the VeraCrypt drive in Explorer: " + ex.Message);
      return -1;
    }
  }

  /// <summary>Inform parent objects to refresh.</summary>
  public void RefreshSysTray()
  {
    // TODO: Inform parents to refresh (SysTray/Sidebar)
    _events.Publish(new SystemTrayRefreshRequest());
  }

  public string SettingDecrypt(string setting) => _secrets.Decrypt(setting);

  public string SettingEncrypt(string setting) => _secrets.Encrypt(setting);

  private bool IsMounted(string root) => root != null && _operations.GetMountedDrives().Contains(root, StringComparer.OrdinalIgnoreCase);
}

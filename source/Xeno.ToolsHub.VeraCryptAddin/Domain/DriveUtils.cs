/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2018-08-14
 * Author:  Damian Suess
 * File:    DriveUtils.cs
 * Description:
 *  VeraCrypt helper functions
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public static class DriveUtils
  {
    public static string FindInstall()
    {
      throw new NotImplementedException();
    }

    /// <summary>Look-up the volume using GetMountList to see if it's mounted or not</summary>
    /// <param name="volumePath">Path to encrypted volume</param>
    /// <returns>Returns true if encrypted volume is mounted</returns>
    public static async Task<bool> IsMounted(string volumePath)
    {
      var dict = await GetMountList();
      var isMounted = dict.ContainsValue(volumePath);

      return isMounted;
    }

    public static async Task<Dictionary<string, string>> GetMountList()
    {
      ////return await Task.Run<Dictionary<char, MountInfo>>(() =>
      ////{
      ////  var mountInfo = new Dictionary<char, MountInfo>();
      ////  var size = MOUNT_LIST_STRUCT
      ////});

      // throw new NotImplementedException();
      return await Task.FromResult(new Dictionary<string, string>());
    }

    /// <summary>Mount drive</summary>
    /// <remarks>Make more dynamic to accept other methods of mounting</remarks>
    public static bool Mount(char driveLetter, string hcPath, string password)
    {
      throw new NotImplementedException();

      ////Mount drive = new Mount(driveLetter, hcPath, password);
      ////drive.MountDrive();
    }

    public static bool Dismount(bool force = false, bool dismountAll = false)
    {
      throw new NotImplementedException();
      ////Mount drive = new Mount(driveLetter, hcPath, password);
      ////drive.DismountDrive();
    }
  }
}

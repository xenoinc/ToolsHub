/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2018-08-14
 * Author:  Damian Suess
 * File:    Utils.cs
 * Description:
 *  VeraCrypt helper functions
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public static class Utils
  {
    public static string FindInstall()
    {
      throw new NotImplementedException();
    }

    public static async Task<Dictionary<string, string>> GetMountList()
    {
      ////return await Task.Run<Dictionary<char, MountInfo>>(() =>
      ////{
      ////  var mountInfo = new Dictionary<char, MountInfo>();

      ////  var size = MOUNT_LIST_STRUCT
      ////});

      throw new NotImplementedException();
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

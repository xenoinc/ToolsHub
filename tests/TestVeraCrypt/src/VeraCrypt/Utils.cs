// Author: Damian Suess
// Date: 2018-08-14
// Reference:
// https://github.com/BananaAcid/VeraCrypt-Cmd/blob/master/Utils.VcGetMounts.v2.cs
// https://github.com/xyder/truecrypt-volumes-lister/blob/master/XTrueCryptVolumesLister/Program.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestVeraCrypt.VeraCrypt
{
  public static class Utils
  {
    public static string FindInstall()
    {
      throw new NotImplementedException();
    }

    public static async Task<Dictionary<string, string>> GetMountList()
    {
      //return await Task.Run<Dictionary<char, MountInfo>>(() =>
      //{
      //  var mountInfo = new Dictionary<char, MountInfo>();

      //  var size = MOUNT_LIST_STRUCT
      //});

      throw new NotImplementedException();
    }

    /// <summary>Mount drive</summary>
    /// <remarks>Make more dynamic to accept other methods of mounting</remarks>
    public static bool Mount(char driveLetter, string hcPath, string password)
    {
      throw new NotImplementedException();

      //Mount drive = new Mount(driveLetter, hcPath, password);
      //drive.MountDrive();
    }

    public static bool Dismount(bool force = false, bool dismountAll = false)
    {
      throw new NotImplementedException();
      //Mount drive = new Mount(driveLetter, hcPath, password);
      //drive.DismountDrive();
    }
  }
}

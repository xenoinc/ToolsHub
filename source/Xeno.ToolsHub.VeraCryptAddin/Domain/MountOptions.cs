/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2018-08-14
 * Author:  Damian Suess
 * File:    MountOptions.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public static class MountOptions
  {
    public static readonly string Dismount = "/d";

    public static readonly string Password = "/p";

    public static readonly string Silent = "/s";

    public static readonly string Beep = "/b";

    public static readonly string Force = "/f";

    public static readonly string KeyFile = "/k";

    public static readonly string DriveLetter = "/l";

    public static readonly string MountOption = " /m ";

    /// <summary>Mount volume as read-only</summary>
    public static readonly string MountOptionReadonly = "ro";

    /// <summary>Mount volume as removable medium (see section Volume Mounted as Removable Medium)</summary>
    public static readonly string MountOptionRemovable = "rm";

    public static readonly string Quit = "/q";

    public static readonly string Volume = "/v";

    /// <summary>It must be followed by a positive integer indicating the PIM (Personal Iterations Multiplier) to use for the volume</summary>
    public static readonly string Pim = "/pim";

    /// <summary>Activate TrueCrypt compatibility mode which enables mounting volumes created with TrueCrypt 6.x and 7.x series </summary>
    public static readonly string Truecrypt = "/tc";

    public static readonly string Hash = "/hash";
  }
}

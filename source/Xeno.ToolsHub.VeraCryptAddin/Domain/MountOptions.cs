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
    public static string Dismount { get => "/d"; }

    public static string Password { get => "/p"; }

    public static string Silent { get => "/s"; }

    public static string Beep { get => "/b"; }

    public static string Force { get => "/f"; }

    public static string KeyFile { get => "/k"; }

    public static string DriveLetter { get => "/l"; }

    public static string MountOption { get => " /m "; }

    public static string MountOptionReadonly { get => "ro"; }

    public static string MountOptionRemovable { get => "rm"; }

    public static string Quit { get => "/q"; }

    public static string Volume { get => "/v"; }

    public static string Pim { get => "/pim"; }

    public static string Truecrypt { get => "/tc"; }

    public static string Hash { get => "/hash"; }
  }
}

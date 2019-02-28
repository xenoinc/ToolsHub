/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-28
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our add-in constants
 */

namespace Xeno.ToolsHub.VeraCryptAddin
{
  public static class Constants
  {
    public static string AddinId => "VeraCryptAddin";

    /// <summary>Gets the VeraCrypt install path</summary>
    /// <value>VeraCrypt install path</value>
    public static string KeyInstallPath => "InstallPath";

    public static string KeyForceDismounts => "ForceDismounts";

    //// First-pass settings; these belong in an array

    public static string KeyHcDriveLetter => "HcDrive";

    public static string KeyHcPath => "HcPath";

    public static string KeyHcPass => "HcPass";

    public static string KeyHcOnStartMount => "HcOnStartMount";

    public static string KeyHcOnExitDismount => "HcOnExitDismount";
  }
}

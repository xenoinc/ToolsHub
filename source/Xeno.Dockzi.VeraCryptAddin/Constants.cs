/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-2-28
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our add-in constants
 */

namespace Xeno.Dockzi.VeraCryptAddin;

public static class Constants
{
  public static string AddinId => "VeraCryptAddin";

  public static string AddinTitle => "VeraCrypt";

  public static string KeyForceDismounts => "ForceDismounts";

  public static string KeyHcDriveLetter => "HcDrive";

  public static string KeyHcOnExitDismount => "HcOnExitDismount";

  public static string KeyHcOnStartMount => "HcOnStartMount";

  public static string KeyOpenExplorerAfterMount => "OpenExplorerAfterMount";

  public static string KeyHcPass => "HcPass";

  //// First-pass settings; these belong in an array
  public static string KeyHcPath => "HcPath";

  /// <summary>Gets the VeraCrypt install path</summary>
  /// <value>VeraCrypt install path</value>
  public static string KeyInstallPath => "InstallPath";
}

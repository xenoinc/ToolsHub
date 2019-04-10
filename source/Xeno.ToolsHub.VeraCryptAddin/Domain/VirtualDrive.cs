/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2018-08-14
 * Author:  Damian Suess
 * File:    VirtualDrive.cs
 * Description:
 *  VeraCrypt virtual drive wrapper
 *
 */

using System;
using System.Diagnostics;

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public class VirtualDrive
  {
    private string _driveLetter = string.Empty;
    private string _hcPath = string.Empty;
    private string _veraCryptPath = string.Empty;
    private Process _proc = new Process();
    private ProcessStartInfo _procInfo = new ProcessStartInfo();

    public VirtualDrive(string vcPath, string driveLetter, string hcPath = "", string password = "")
    {
      _veraCryptPath = vcPath;

      _driveLetter = driveLetter;
      _hcPath = hcPath;
      Password = password;
    }

    public string Password { get; set; }

    public bool IsSilent { get; set; }

    public bool IsBeep { get; set; }

    public bool IsReadOnly { get; set; }

    public bool IsRemoveable { get; set; }

    /// <summary>Dismount virtual drive</summary>
    /// <returns>Process exit code (1= failure)</returns>
    /// <param name="force">Force dismount</param>
    public int Dismount(bool force = true)
    {
      string args = $"{MountOptions.Dismount} {_driveLetter} {MountOptions.Quit}";

      if (IsSilent)
        args += $" {MountOptions.Silent}";

      if (IsBeep)
        args += $" {MountOptions.Beep}";

      if (force)
        args += $" {MountOptions.Force}";

      return Execute(args);
    }

    /// <summary>Mount virtual drive</summary>
    /// <returns>Process exit code (1= failure)</returns>
    public int Mount()
    {
      // Build execution parameter set
      string args = $"{MountOptions.DriveLetter} {_driveLetter} {MountOptions.Quit}";

      if (Password.Length > 0)
        args += $" {MountOptions.Password} \"{Password}\"";

      if (IsSilent)
        args += $" {MountOptions.Silent}";

      if (IsReadOnly)
        args += $" {MountOptions.MountOption} {MountOptions.MountOptionReadonly}";

      if (IsRemoveable)
        args += $" {MountOptions.MountOption} {MountOptions.MountOptionRemovable}";

      args = $"{MountOptions.Volume} \"{_hcPath}\" {args}";
      return Execute(args);
    }

    /// <summary>Execute process wrapper</summary>
    /// <param name="arguments">File execution arguments</param>
    /// <returns>Process exit code (1= failure)</returns>
    private int Execute(string arguments)
    {
      int exitCode = 0;

      _procInfo.FileName = _veraCryptPath;
      _procInfo.RedirectStandardOutput = true;
      _procInfo.UseShellExecute = false;
      _procInfo.Arguments = arguments;

      try
      {
        _proc.StartInfo = _procInfo;
        _proc.Start();
        _proc.WaitForExit();
        exitCode = _proc.ExitCode;
        _proc.Close();
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(ex.Message);
        exitCode = 1;
      }

      return exitCode;
    }
  }
}

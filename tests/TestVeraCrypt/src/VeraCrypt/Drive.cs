// Copyright 2018 Xeno Innovations, Inc.
// Author: Damian Suess
// Date: 2018-08-14
//
// Reference:
//  https://github.com/LordSandwurm/VeraCryptMounter/blob/master/TrueCrypt%20Mounter/Mount.cs

using System;
using System.Diagnostics;

namespace TestVeraCrypt.VeraCrypt
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

  public class Drive
  {
    // private char _driveLetter = "";
    private string _driveLetter = "";

    private string _hcPath = "";

    private string _veraCryptPath = "";

    private Process _proc = new Process();
    private ProcessStartInfo _procInfo = new ProcessStartInfo();

    public Drive(string vcPath, string driveLetter, string hcPath = "", string password = "")
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
    public int Dismount(bool force = true)
    {
      string args = $"{MountOptions.Dismount} {_driveLetter} {MountOptions.Quit}";

      if (IsSilent) args += $" {MountOptions.Silent}";
      if (IsBeep) args += $" {MountOptions.Beep}";
      if (force) args += $" {MountOptions.Force}";

      return Execute(args);
    }

    /// <summary>Mount virtual drive</summary>
    /// <returns>Process exit code (1= failure)</returns>
    public int Mount()
    {
      //Build execution parameter set
      string args = $"{MountOptions.DriveLetter} {_driveLetter} {MountOptions.Password} {Password} {MountOptions.Quit}";

      if (IsSilent) args += $" {MountOptions.Silent}";
      if (IsReadOnly) args += $" {MountOptions.MountOption} {MountOptions.MountOptionReadonly}";
      if (IsRemoveable) args += $" {MountOptions.MountOption} {MountOptions.MountOptionRemovable}";

      args = $"{MountOptions.Volume} {_hcPath} {args};
      return Execute(args);
    }

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

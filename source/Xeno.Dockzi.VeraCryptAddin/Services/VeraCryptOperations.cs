namespace Xeno.Dockzi.VeraCryptAddin.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Xeno.Dockzi.VeraCryptAddin.Domain;

public sealed class VeraCryptOperations : IVeraCryptOperations
{
  public static ProcessStartInfo CreateExplorerStartInfo(string drive)
  {
    string root = NormalizeDriveRoot(drive) ?? throw new ArgumentException("A single drive letter is required.", nameof(drive));
    var info = new ProcessStartInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"))
    {
      UseShellExecute = true,
    };

    info.ArgumentList.Add(root);
    return info;
  }

  public static string NormalizeDriveRoot(string drive)
  {
    drive = drive?.Trim();
    if (string.IsNullOrEmpty(drive) ||
        (drive.Length != 1 && !(drive.Length == 2 && drive[1] == ':') &&
        !(drive.Length == 3 && drive[1] == ':' && drive[2] == '\\')))
    {
      return null;
    }

    char letter = char.ToUpperInvariant(drive[0]);
    return letter >= 'A' && letter <= 'Z' ? $"{letter}:\\" : null;
  }

  public int Dismount(string executable, string drive, string path, bool force) =>
    new VirtualDrive(executable, drive, path) { IsSilent = false }.Dismount(force);

  public IReadOnlyList<string> GetMountedDrives()
  {
    var drives = new List<string>();
    var target = new StringBuilder(32768);
    for (char letter = 'A'; letter <= 'Z'; letter++)
    {
      target.Clear();
      if (NativeMethods.QueryDosDevice($"{letter}:", target, target.Capacity) != 0 &&
          target.ToString().StartsWith(@"\Device\VeraCryptVolume", StringComparison.OrdinalIgnoreCase))
        drives.Add($"{letter}:\\");
    }

    return drives;
  }

  public bool IsDriveInUse(string drive)
  {
    string root = NormalizeDriveRoot(drive);
    if (root == null)
      return true;

    var target = new StringBuilder(32768);
    return NativeMethods.QueryDosDevice(root.TrimEnd('\\'), target, target.Capacity) != 0
      || Marshal.GetLastWin32Error() != 2;
  }

  public bool IsInstalled(string executable) => File.Exists(executable);

  public int Mount(string executable, string drive, string path, string password) =>
    new VirtualDrive(executable, drive, path, password) { IsSilent = false }.Mount();

  public void OpenInExplorer(string drive)
  {
    using var process = Process.Start(CreateExplorerStartInfo(drive));
  }

  private static class NativeMethods
  {
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint QueryDosDevice(string deviceName, StringBuilder targetPath, int capacity);
  }
}

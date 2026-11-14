namespace Xeno.Dockzi.SidebarAddin.Services;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Win32.SafeHandles;

/// <summary>Matches launcher targets to processes with visible top-level windows.</summary>
public sealed class WindowsRunningApplications : IRunningApplications
{
  private readonly Dictionary<string, (DateTime Modified, string Path)> _links = new(StringComparer.OrdinalIgnoreCase);

  [return: MarshalAs(UnmanagedType.Bool)]
  private delegate bool EnumWindowCallback(IntPtr window, IntPtr parameter);

  public IReadOnlySet<string> GetOpenExecutables()
  {
    var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var processes = new HashSet<uint>();
    EnumWindows((window, _) =>
    {
      if (!IsWindowVisible(window))
        return true;

      DwmGetWindowAttribute(window, 14, out int cloaked, sizeof(int));

      if (cloaked != 0)
        return true;

      GetWindowThreadProcessId(window, out uint processId);

      if (!processes.Add(processId))
        return true;

      using var process = OpenProcess(0x1000, false, processId);

      if (process.IsInvalid)
        return true;

      var path = new StringBuilder(32768);
      int length = path.Capacity;

      if (QueryFullProcessImageName(process, 0, path, ref length))
        paths.Add(path.ToString());

      return true;
    }, IntPtr.Zero);
    return paths;
  }

  public string ResolveExecutable(string target)
  {
    if (string.IsNullOrWhiteSpace(target))
      return null;

    target = Environment.ExpandEnvironmentVariables(target);

    if (string.Equals(Path.GetExtension(target), ".lnk", StringComparison.OrdinalIgnoreCase))
    {
      DateTime modified = File.GetLastWriteTimeUtc(target);
      if (_links.TryGetValue(target, out var cached) && cached.Modified == modified)
        return cached.Path;

      string resolved = ResolveLink(target);
      _links[target] = (modified, resolved);
      return resolved;
    }

    return string.Equals(Path.GetExtension(target), ".exe", StringComparison.OrdinalIgnoreCase)
      ? Path.GetFullPath(target) : null;
  }

  [DllImport("dwmapi.dll")]
  private static extern int DwmGetWindowAttribute(IntPtr window, int attribute, out int value, int size);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool EnumWindows(EnumWindowCallback callback, IntPtr parameter);

  [DllImport("user32.dll")]
  private static extern uint GetWindowThreadProcessId(IntPtr window, out uint processId);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool IsWindowVisible(IntPtr window);

  [DllImport("kernel32.dll")]
  private static extern SafeProcessHandle OpenProcess(uint access, [MarshalAs(UnmanagedType.Bool)] bool inherit, uint processId);

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool QueryFullProcessImageName(SafeProcessHandle process, uint flags, StringBuilder name, ref int size);

  private static string ResolveLink(string target)
  {
    object link = null;
    try
    {
      link = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046")));
      ((IPersistFile)link).Load(target, 0);

      var path = new StringBuilder(32768);

      ((IShellLink)link).GetPath(path, path.Capacity, IntPtr.Zero, 4);
      string resolved = Environment.ExpandEnvironmentVariables(path.ToString());

      return string.IsNullOrWhiteSpace(resolved) ? null : Path.GetFullPath(resolved);
    }
    catch (Exception ex) when (ex is COMException || ex is IOException || ex is ArgumentException)
    {
      return null;
    }
    finally
    {
      if (link != null)
        Marshal.FinalReleaseComObject(link);
    }
  }
}

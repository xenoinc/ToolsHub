namespace Xeno.Dockzi.SidebarAddin.Services;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xeno.Dockzi.SidebarAddin.Models;

public sealed class WindowsLauncherShell : ILauncherShell
{
  public static ProcessStartInfo CreateStartInfo(DockLauncher item)
  {
    ArgumentNullException.ThrowIfNull(item);
    ArgumentException.ThrowIfNullOrWhiteSpace(item.Target);
    string target = Environment.ExpandEnvironmentVariables(item.Target);
    var info = new ProcessStartInfo(target) { UseShellExecute = true };

    // Let the shell honor a .lnk's stored arguments and working directory.
    if (File.Exists(target) && !string.Equals(Path.GetExtension(target), ".lnk", StringComparison.OrdinalIgnoreCase))
      info.WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(target));
    else if (Directory.Exists(target))
      info.WorkingDirectory = Path.GetFullPath(target);

    return info;
  }

  public ImageSource GetIcon(string target)
  {
    // SHGetFileInfo resolves executable, file, directory and Windows shortcut icons.
    var info = new ShellFileInfo();
    SHGetFileInfo(target, 0, ref info, (uint)Marshal.SizeOf<ShellFileInfo>(), 0x100);
    if (info.Icon == IntPtr.Zero)
      return CreateImage(System.Drawing.SystemIcons.Application.Handle);

    try
    {
      return CreateImage(info.Icon);
    }
    finally
    {
      DestroyIcon(info.Icon);
    }
  }

  public DockLauncher Import(string path)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(path);
    if (Uri.TryCreate(path, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
      return new DockLauncher { Title = uri.Host, Target = uri.AbsoluteUri };

    path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(path.Trim('"')));

    if (!File.Exists(path) && !Directory.Exists(path))
      throw new FileNotFoundException("The dropped item could not be found.", path);

    return new DockLauncher
    {
      Title = Directory.Exists(path) ? new DirectoryInfo(path).Name : Path.GetFileNameWithoutExtension(path),
      Target = path,
    };
  }

  public void Launch(DockLauncher item)
  {
    using var process = Process.Start(CreateStartInfo(item));
  }

  private static ImageSource CreateImage(IntPtr icon)
  {
    var image = Imaging.CreateBitmapSourceFromHIcon(icon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    image.Freeze();
    return image;
  }

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool DestroyIcon(IntPtr icon);

  [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
  private static extern IntPtr SHGetFileInfo(string path, uint attributes, ref ShellFileInfo info, uint size, uint flags);

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  private struct ShellFileInfo
  {
    public IntPtr Icon;

    public int IconIndex;

    public uint Attributes;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string DisplayName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string TypeName;
  }
}

namespace Xeno.Dockzi.PomodoroAddin.Domain;

using System;
using System.Drawing;
using System.Runtime.InteropServices;

public sealed class IconGenerator
{
  private readonly string _text;

  public IconGenerator(string text) => _text = text;

  /// <summary>Creates an icon owned by the caller.</summary>
  /// <returns>An independent icon that the caller must dispose.</returns>
  public Icon ToIcon()
  {
    using var bitmap = new Bitmap(16, 16);
    using var graphics = Graphics.FromImage(bitmap);
    using var font = new Font(FontFamily.GenericSansSerif, 8);
    using var accent = new SolidBrush(Color.Black);
    using var color = new SolidBrush(Color.Red);
    graphics.DrawString(_text, font, accent, 1, 1);
    graphics.DrawString(_text, font, color, 0, 0);
    IntPtr handle = bitmap.GetHicon();
    try
    {
      using var borrowed = Icon.FromHandle(handle);
      return (Icon)borrowed.Clone();
    }
    finally
    {
      NativeMethods.DestroyIcon(handle);
    }
  }

  private static class NativeMethods
  {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyIcon(IntPtr icon);
  }
}

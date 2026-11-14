namespace Xeno.Dockzi.NotesAddin;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>A message-only window owns and releases the Windows hotkey registration.</summary>
internal sealed class GlobalShortcut : NativeWindow, IDisposable
{
  private const int HotkeyId = 0x4E54;
  private readonly Action _action;

  public GlobalShortcut(Keys key, bool windows, Action action)
  {
    _action = action;
    CreateHandle(new CreateParams { Caption = "Dockzi Notes shortcut", Parent = new IntPtr(-3) });
    uint modifiers = 0x4000; // MOD_NOREPEAT

    if ((key & Keys.Control) != 0)
      modifiers |= 2;

    if ((key & Keys.Alt) != 0)
      modifiers |= 1;

    if ((key & Keys.Shift) != 0)
      modifiers |= 4;

    if (windows)
      modifiers |= 8;

    if (!RegisterHotKey(Handle, HotkeyId, modifiers, (uint)(key & Keys.KeyCode)))
    {
      int error = Marshal.GetLastWin32Error();
      DestroyHandle();
      throw new Win32Exception(error, "The Notes shortcut could not be registered. It may already be in use.");
    }
  }

  public void Dispose()
  {
    if (Handle == IntPtr.Zero)
      return;

    UnregisterHotKey(Handle, HotkeyId);
    DestroyHandle();
  }

  protected override void WndProc(ref Message message)
  {
    if (message.Msg == 0x0312 && message.WParam.ToInt32() == HotkeyId)
      _action();

    base.WndProc(ref message);
  }

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool RegisterHotKey(IntPtr window, int id, uint modifiers, uint key);

  [DllImport("user32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool UnregisterHotKey(IntPtr window, int id);
}

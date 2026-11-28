namespace Xeno.Dockzi.NotesAddin;

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Xeno.Dockzi.Services.Logging;

/// <summary>Gives note windows their own taskbar group instead of inheriting Dockzi's pinned icon.</summary>
internal static class NoteTaskbarIdentity
{
  private const string AppId = "Xeno.Dockzi.Notes";
  private static readonly PropertyKey AppIdKey = new(new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), 5);

  [ComImport]
  [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  private interface IPropertyStore
  {
    void GetCount(out uint count);

    void GetAt(uint index, out PropertyKey key);

    void GetValue(ref PropertyKey key, out PropVariant value);

    void SetValue(ref PropertyKey key, ref PropVariant value);

    void Commit();
  }

  public static void Attach(Window window) => window.SourceInitialized += (_, _) =>
    {
      var handle = new WindowInteropHelper(window).Handle;
      try
      {
        SetAppId(handle, AppId);
        HwndSource.FromHwnd(handle)?.AddHook((IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled) =>
        {
          // Release window properties while the native window still exists (WM_DESTROY).
          if (message == 0x0002)
          {
            try
            {
              SetAppId(hwnd, null);
            }
            catch (COMException ex)
            {
              Log.Warn("Unable to clear the Notes taskbar identity: " + ex.Message);
            }
          }

          return IntPtr.Zero;
        });
      }
      catch (COMException ex)
      {
        // Shell integration must not prevent a user from opening or saving a note.
        Log.Warn("Unable to set the Notes taskbar identity: " + ex.Message);
      }
    };

  private static void SetAppId(IntPtr window, string appId)
  {
    var iid = typeof(IPropertyStore).GUID;
    Marshal.ThrowExceptionForHR(SHGetPropertyStoreForWindow(window, ref iid, out var store));
    var value = new PropVariant();
    try
    {
      if (appId != null)
      {
        value.Type = 31; // VT_LPWSTR; VT_EMPTY clears the property on destruction.
        value.Pointer = Marshal.StringToCoTaskMemUni(appId);
      }

      var key = AppIdKey;
      store.SetValue(ref key, ref value);
    }
    finally
    {
      if (value.Pointer != IntPtr.Zero)
        Marshal.FreeCoTaskMem(value.Pointer);

      Marshal.ReleaseComObject(store);
    }
  }

  [DllImport("shell32.dll")]
  private static extern int SHGetPropertyStoreForWindow(IntPtr window, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out IPropertyStore store);

  [StructLayout(LayoutKind.Sequential)]
  private struct PropertyKey
  {
    public Guid Format;
    public uint Id;

    public PropertyKey(Guid format, uint id)
    {
      Format = format;
      Id = id;
    }
  }

  // PROPVARIANT is 16 bytes on x86 and 24 bytes on x64/ARM64.
  [StructLayout(LayoutKind.Sequential)]
  private struct PropVariant
  {
    public ushort Type;
    public ushort Reserved1;
    public ushort Reserved2;
    public ushort Reserved3;
    public IntPtr Pointer;
    public IntPtr Padding;
  }
}

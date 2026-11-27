namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.NotesAddin;

[TestClass]
[DoNotParallelize]
[OSCondition(OperatingSystems.Windows)]
public class NoteTaskbarTests
{
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

  [TestMethod]
  public void NotesUseTheirOwnSharedNativeTaskbarIdentityAndNoteIcon()
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      var first = new NoteWindow(new Note(Guid.NewGuid(), "# First", DateTime.UtcNow), _ => null);
      var second = new NoteWindow(new Note(Guid.NewGuid(), "# Second", DateTime.UtcNow), _ => null);
      var ordinary = new Window();
      try
      {
        first.Show();
        second.Show();
        ordinary.Show();

        Assert.AreEqual("Xeno.Dockzi.Notes", ReadAppId(first));
        Assert.AreEqual(ReadAppId(first), ReadAppId(second));
        Assert.AreNotEqual(ReadAppId(first), ReadAppId(ordinary));
        Assert.Contains("NoteIcon.ico", first.Icon.ToString());
        Assert.AreNotEqual(IntPtr.Zero, SendMessage(new WindowInteropHelper(first).Handle, 0x007F, new IntPtr(1), IntPtr.Zero));
      }
      catch (Exception ex)
      {
        failure = ex;
      }
      finally
      {
        first.CloseWithoutSaving();
        second.CloseWithoutSaving();
        ordinary.Close();
      }
    });

    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }

  [DllImport("ole32.dll")]
  private static extern int PropVariantClear(ref PropVariant value);

  private static string ReadAppId(Window window)
  {
    var iid = typeof(IPropertyStore).GUID;
    Marshal.ThrowExceptionForHR(SHGetPropertyStoreForWindow(new WindowInteropHelper(window).Handle, ref iid, out var store));
    var value = new PropVariant();
    try
    {
      var key = new PropertyKey { Format = new Guid("9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3"), Id = 5 };
      store.GetValue(ref key, out value);

      return value.Type == 31 ? Marshal.PtrToStringUni(value.Pointer) : string.Empty;
    }
    finally
    {
      PropVariantClear(ref value);
      Marshal.ReleaseComObject(store);
    }
  }

  [DllImport("user32.dll")]
  private static extern IntPtr SendMessage(IntPtr window, uint message, IntPtr wParam, IntPtr lParam);

  [DllImport("shell32.dll")]
  private static extern int SHGetPropertyStoreForWindow(IntPtr window, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out IPropertyStore store);

  [StructLayout(LayoutKind.Sequential)]
  private struct PropertyKey
  {
    public Guid Format;
    public uint Id;
  }

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

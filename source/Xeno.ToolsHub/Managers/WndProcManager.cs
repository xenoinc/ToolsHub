/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-9
 * Author:  Damian Suess
 * File:    WndProcManager.cs
 * Description:
 *  Add-in manager for listening to Windows event messages (without a Form)
 *  This listens to system shutdown/log-off and custom messages
 *  and triggers add-ins accordingly.
 *
 * Reference:
 *  https://stackoverflow.com/questions/43885905/nativewindows-assignhandle-without-a-parent-form
 *    - https://stackoverflow.com/questions/935608/how-do-i-create-a-message-only-window-from-windows-forms
 *    - https://stackoverflow.com/questions/16245706/check-for-device-change-add-remove-events/16245901#16245901
 */

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Xeno.ToolsHub.Managers
{
  public class WndProcManager : NativeWindow
  {
    private readonly string _caption;

    public WndProcManager(string caption)
    {
      _caption = caption;
    }

    public bool CreateWindow()
    {
      if (Handle == IntPtr.Zero)
      {
        CreateHandle(new CreateParams
        {
          Style = 0,
          ExStyle = 0,
          ClassStyle = 0,
          Caption = _caption,
          Parent = (IntPtr)NativeMethods.HWND_MESSAGE
        });
      }

      return Handle != IntPtr.Zero;
    }

    public override void DestroyHandle()
    {
      DestroyWindow(false, IntPtr.Zero);
      base.DestroyHandle();
    }

    public void DestroyWindow()
    {
      DestroyWindow(true, IntPtr.Zero);
    }

    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void WndProc(ref Message m)
    {
      switch (m.Msg)
      {
        case NativeMethods.WM_QUERYENDSESSION:
          // System is about to log-off, shutdown, or reboot
          // TODO: Signal add-ins!!
          NotifySystemShutDown();
          break;
      }

      base.WndProc(ref m);
    }

    private void DestroyWindow(bool destroyHwnd, IntPtr hWnd)
    {
      if (hWnd == IntPtr.Zero)
        hWnd = Handle;

      if (GetInvokeRequired(hWnd))
      {
        NativeMethods.PostMessage(new HandleRef(this, hWnd), NativeMethods.WM_CLOSE, 0, 0);
        return;
      }

      lock (this)
      {
        if (destroyHwnd)
          base.DestroyHandle();
      }
    }

    private bool GetInvokeRequired(IntPtr hWnd)
    {
      return false;
    }

    /// <summary>Notify add-ins that the system is shutting down.</summary>
    private void NotifySystemShutDown()
    {
      // TODO: Notify add-ins of system shutdown.
    }

    /// <summary>CA1060 compliant class for Native Methods.</summary>
    private static class NativeMethods
    {
      public const int WM_CLOSE = 0x0010;
      public const int WM_QUERYENDSESSION = 0x0011;

      public static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

      ////[SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources")]
      ////private static readonly HandleRef HwndMessage = new HandleRef(null, new IntPtr(-3));

      [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
      [ResourceExposure(ResourceScope.Process)]
      public static extern int GetCurrentThreadId();

      [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
      [ResourceExposure(ResourceScope.Process)]
      public static extern int GetWindowThreadProcessId(HandleRef hWnd, out int lpdwProcessId);

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      [ResourceExposure(ResourceScope.None)]
      public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);
    }
  }
}

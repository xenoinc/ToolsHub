// Author: Damian Suess
// Date: 2018-08-14

// https://stackoverflow.com/questions/17912969/detect-when-a-new-virtual-drive-is-created

using System;
using System.Runtime.InteropServices;

namespace TestVeraCrypt.VeraCrypt
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
  internal struct MOUNT_LIST_STRUCT_VOLUME_NAME
  {
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I2, SizeConst = 260)]
    public readonly char[] wszVolume; // Volume names of mounted volumes

    public override string ToString()
    {
      String ret = (new String(wszVolume)).TrimEnd('\0');
      ret = ret.StartsWith("\\??\\") ? ret.Substring(4) : ret;
      return ret;
    }
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
  internal class MOUNT_LIST_STRUCT
  {
    /// <summary>Bitfield of all mounted drive letters</summary>
    public readonly UInt32 ulMountedDrives;

    /// <summary>Volume names of mounted volumes</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
    public readonly MOUNT_LIST_STRUCT_VOLUME_NAME[] wszVolume;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
    public readonly UInt64[] diskLength;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
    public readonly int[] ea;

    /// <summary>Volume type (e.g. PROP_VOL_TYPE_OUTER, PROP_VOL_TYPE_OUTER_VOL_WRITE_PREVENTED, etc.)</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
    public readonly int[] volumeType;
  }
}

// Author: Damian Suess
// Date: 2018-08-14

namespace TestVeraCrypt.VeraCrypt
{
  public class MountInfo
  {
    public char DriveLetter { get; set; }

    public string VolumeName { get; set; }

    public string VolumeLabel { get; set; }

    public ulong DiskSize { get; set; }

    public bool TrueCryptMode { get; set; }
  }
}

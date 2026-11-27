namespace Xeno.Dockzi.VeraCryptAddin.Services;

using System.Collections.Generic;

public interface IVeraCryptOperations
{
  int Dismount(string executable, string drive, string path, bool force);

  IReadOnlyList<string> GetMountedDrives();

  bool IsDriveInUse(string drive);

  bool IsInstalled(string executable);

  int Mount(string executable, string drive, string path, string password);

  void OpenInExplorer(string drive);
}

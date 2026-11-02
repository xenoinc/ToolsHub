namespace Xeno.ToolsHub.VeraCryptAddin.Services;

using System.IO;
using Xeno.ToolsHub.VeraCryptAddin.Domain;

public sealed class VeraCryptOperations : IVeraCryptOperations
{
  public bool IsInstalled(string executable) => File.Exists(executable);

  public int Mount(string executable, string drive, string path, string password) =>
    new VirtualDrive(executable, drive, path, password) { IsSilent = false }.Mount();

  public int Dismount(string executable, string drive, string path, bool force) =>
    new VirtualDrive(executable, drive, path) { IsSilent = false }.Dismount(force);
}

namespace Xeno.ToolsHub.VeraCryptAddin.Services;

public interface IVeraCryptOperations
{
  bool IsInstalled(string executable);

  int Mount(string executable, string drive, string path, string password);

  int Dismount(string executable, string drive, string path, bool force);
}

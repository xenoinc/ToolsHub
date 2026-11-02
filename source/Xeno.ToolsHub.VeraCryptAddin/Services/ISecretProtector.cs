namespace Xeno.ToolsHub.VeraCryptAddin.Services;

public interface ISecretProtector
{
  string Encrypt(string value);

  string Decrypt(string value);
}

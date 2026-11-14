namespace Xeno.Dockzi.VeraCryptAddin.Services;

public interface ISecretProtector
{
  string Encrypt(string value);

  string Decrypt(string value);
}

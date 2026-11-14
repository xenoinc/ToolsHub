namespace Xeno.Dockzi.VeraCryptAddin.Services;

using Xeno.Dockzi.VeraCryptAddin.Domain;

public sealed class SecretProtector : ISecretProtector
{
  private readonly Crypto _crypto = new();

  public string Encrypt(string value) => _crypto.Encrypt(value);

  public string Decrypt(string value) => _crypto.Decrypt(value);
}

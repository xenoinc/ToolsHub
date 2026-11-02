namespace Xeno.ToolsHub.VeraCryptAddin.Services;

using Xeno.ToolsHub.VeraCryptAddin.Domain;

public sealed class SecretProtector : ISecretProtector
{
  private readonly Crypto _crypto = new();

  public string Encrypt(string value) => _crypto.Encrypt(value);

  public string Decrypt(string value) => _crypto.Decrypt(value);
}

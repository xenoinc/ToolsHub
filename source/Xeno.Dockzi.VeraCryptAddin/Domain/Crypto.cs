/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-10
 * Author:  Damian Suess
 * File:    Crypto.cs
 * Description:
 *  Simple encryption engine
 *
 * Ref:
 *  https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.7.2
 *
 * Future:
 *  - [ ] Allow user to provide their OWN salt since this is OpenSource!
 *  - [ ] Select which cipher engine to use
 *  - [ ] Make a stronger salt/entropy
 *  - [ ] Consider pre-pending version number at beginning for auto-decipher
 */

using System;
using System.Buffers.Binary;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xeno.Dockzi.Services.Logging;

namespace Xeno.Dockzi.VeraCryptAddin.Domain;

public class Crypto
{
  // Preserve the original AES helper format and PBKDF2 defaults for compatibility.
  // These helpers are separate from the DPAPI settings path and are not a new encryption format.
  private const int LegacyAesIterations = 1000;
  private const int AesKeySizeBytes = 32;
  private const int AesIvSizeBytes = 16;

  private readonly byte[] _entropy = { 8, 3, 198, 33, 7, 83, 1, 235 };

  public Crypto()
  {
    ProtectionScope = DataProtectionScope.LocalMachine;
  }

  public DataProtectionScope ProtectionScope { get; set; }

  public string Encrypt(string data)
  {
    return ProtectDataEncrypt(data);
  }

  public string Decrypt(string data)
  {
    return ProtectDataDecrypt(data);
  }

  private string ProtectDataEncrypt(string data)
  {
    string output = string.Empty;
    try
    {
      byte[] bytes = ConvertStringToBytes(data);
      byte[] masked = ProtectedData.Protect(bytes, _entropy, ProtectionScope);

      output = System.Convert.ToBase64String(masked);
      //// data = Version1 + BytesToString(masked);
    }
    catch (Exception ex)
    {
      Log.Error($"Unable to encrypt data. {ex.Message}");
    }

    return output;
  }

  private string ProtectDataDecrypt(string data)
  {
    if (data.Length < 2)
      return data;
    ////else if (data.Substring(0, 2) != Version1)
    ////  return data;
    ////else
    ////  data = data.Substring(2);

    try
    {
      byte[] bytes = System.Convert.FromBase64String(data);
      byte[] unmasked = ProtectedData.Unprotect(bytes, _entropy, ProtectionScope);

      data = ConvertBytesToString(unmasked);
    }
    catch (Exception ex)
    {
      Log.Error($"Unable to encrypt data. {ex.Message}");
    }

    return data;
  }

  private string ConvertBytesToString(byte[] data)
  {
    return Encoding.UTF8.GetString(data);
  }

  private byte[] ConvertStringToBytes(string data)
  {
    return Encoding.UTF8.GetBytes(data);
  }

  /// <summary>Encrypts text using the original password-based AES compatibility format.</summary>
  /// <param name="plainText">The text to encrypt.</param>
  /// <param name="sharedSecret">The password used to derive the encryption key.</param>
  /// <returns>Base64 containing the little-endian IV length, IV, and AES-CBC ciphertext.</returns>
  private string EncryptStringAES(string plainText, string sharedSecret)
  {
    if (string.IsNullOrEmpty(plainText))
      throw new ArgumentNullException(nameof(plainText));

    if (string.IsNullOrEmpty(sharedSecret))
      throw new ArgumentNullException(nameof(sharedSecret));

    byte[] key = Rfc2898DeriveBytes.Pbkdf2(sharedSecret, _entropy, LegacyAesIterations, HashAlgorithmName.SHA1, AesKeySizeBytes);
    try
    {
      using var aes = Aes.Create();
      aes.Key = key;
      aes.Mode = CipherMode.CBC;
      aes.Padding = PaddingMode.PKCS7;
      aes.GenerateIV();

      using var output = new MemoryStream();
      Span<byte> ivLength = stackalloc byte[sizeof(int)];
      BinaryPrimitives.WriteInt32LittleEndian(ivLength, aes.IV.Length);
      output.Write(ivLength);
      output.Write(aes.IV);

      using (var encryptor = aes.CreateEncryptor())
      using (var encrypted = new CryptoStream(output, encryptor, CryptoStreamMode.Write, leaveOpen: true))
      using (var writer = new StreamWriter(encrypted))
        writer.Write(plainText);

      return Convert.ToBase64String(output.ToArray());
    }
    finally
    {
      CryptographicOperations.ZeroMemory(key);
    }
  }

  /// <summary>Decrypts text written by the original or updated AES compatibility helper.</summary>
  /// <param name="cipherText">Base64 containing the IV length, IV, and AES-CBC ciphertext.</param>
  /// <param name="sharedSecret">The password used for encryption.</param>
  /// <returns>The decrypted text.</returns>
  private string DecryptStringAES(string cipherText, string sharedSecret)
  {
    if (string.IsNullOrEmpty(cipherText))
      throw new ArgumentNullException(nameof(cipherText));

    if (string.IsNullOrEmpty(sharedSecret))
      throw new ArgumentNullException(nameof(sharedSecret));

    using var input = new MemoryStream(Convert.FromBase64String(cipherText));
    byte[] iv = ReadByteArray(input);
    long remaining = input.Length - input.Position;
    if (remaining == 0 || remaining % AesIvSizeBytes != 0)
      throw new CryptographicException("The AES ciphertext must contain complete blocks.");

    byte[] key = Rfc2898DeriveBytes.Pbkdf2(sharedSecret, _entropy, LegacyAesIterations, HashAlgorithmName.SHA1, AesKeySizeBytes);
    try
    {
      using var aes = Aes.Create();
      aes.Key = key;
      aes.IV = iv;
      aes.Mode = CipherMode.CBC;
      aes.Padding = PaddingMode.PKCS7;
      using var decryptor = aes.CreateDecryptor();
      using var decrypted = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
      using var reader = new StreamReader(decrypted);
      return reader.ReadToEnd();
    }
    finally
    {
      CryptographicOperations.ZeroMemory(key);
    }
  }

  private byte[] ReadByteArray(Stream stream)
  {
    Span<byte> length = stackalloc byte[sizeof(int)];
    stream.ReadExactly(length);
    if (BinaryPrimitives.ReadInt32LittleEndian(length) != AesIvSizeBytes)
      throw new CryptographicException("The AES initialization vector must be 16 bytes.");

    byte[] iv = new byte[AesIvSizeBytes];
    stream.ReadExactly(iv);
    return iv;
  }
}

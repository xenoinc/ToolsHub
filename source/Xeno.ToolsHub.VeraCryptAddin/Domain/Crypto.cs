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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xeno.ToolsHub.Services.Logging;

namespace Xeno.ToolsHub.VeraCryptAddin.Domain
{
  public class Crypto
  {
    private const string Version1 = "10"; // Version and cipher type

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

    /// <summary>
    /// Encrypt the given string using AES.  The string can be decrypted using
    /// DecryptStringAES().  The sharedSecret parameters must match.
    /// </summary>
    /// <param name="plainText">The text to encrypt.</param>
    /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
    /// <returns>Encrypted string</returns>
    private string EncryptStringAES(string plainText, string sharedSecret)
    {
      if (string.IsNullOrEmpty(plainText))
        throw new ArgumentNullException("plainText");
      if (string.IsNullOrEmpty(sharedSecret))
        throw new ArgumentNullException("sharedSecret");

      string outStr = null;                       // Encrypted string to return
      RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

      try
      {
        // generate the key from the shared secret and the salt
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _entropy);

        // Create a RijndaelManaged object
        aesAlg = new RijndaelManaged();
        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

        // Create an encryptor to perform the stream transform.
        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        // Create the streams used for encryption.
        using (MemoryStream memEncrypt = new MemoryStream())
        {
          // Suffix with the initialization vector (IV)
          memEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
          memEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
          using (CryptoStream cstreamEncrypt = new CryptoStream(memEncrypt, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter writerEncrypt = new StreamWriter(cstreamEncrypt))
            {
              // Write all data to the stream.
              writerEncrypt.Write(plainText);
            }
          }

          outStr = Convert.ToBase64String(memEncrypt.ToArray());
        }
      }
      finally
      {
        // Clear the RijndaelManaged object.
        if (aesAlg != null)
          aesAlg.Clear();
      }

      // Return the encrypted bytes from the memory stream.
      return outStr;
    }

    /// <summary>
    ///   Decrypt the given string.  Assumes the string was encrypted using
    ///   EncryptStringAES(), using an identical sharedSecret.
    /// </summary>
    /// <param name="cipherText">The text to decrypt.</param>
    /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
    /// <returns>Decrypted string</returns>
    private string DecryptStringAES(string cipherText, string sharedSecret)
    {
      if (string.IsNullOrEmpty(cipherText))
        throw new ArgumentNullException("cipherText");
      if (string.IsNullOrEmpty(sharedSecret))
        throw new ArgumentNullException("sharedSecret");

      // Declare the RijndaelManaged object used to decrypt the data.
      RijndaelManaged aesAlg = null;

      // Declare the string used to hold the decrypted text.
      string plaintext = null;

      try
      {
        // generate the key from the shared secret and the salt
        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _entropy);

        // Create the streams used for decryption.
        byte[] bytes = Convert.FromBase64String(cipherText);
        using (MemoryStream memDecrypt = new MemoryStream(bytes))
        {
          // Create a RijndaelManaged object
          // with the specified key and IV.
          aesAlg = new RijndaelManaged();
          aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

          // Get the initialization vector from the encrypted stream
          aesAlg.IV = ReadByteArray(memDecrypt);

          // Create a decryptor to perform the stream transform.
          ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
          using (CryptoStream cstreamDecrypt = new CryptoStream(memDecrypt, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader readerDecrypt = new StreamReader(cstreamDecrypt))

              // Read the decrypted bytes from the decrypting stream
              // and place them in a string.
              plaintext = readerDecrypt.ReadToEnd();
          }
        }
      }
      finally
      {
        // Clear the RijndaelManaged object.
        if (aesAlg != null)
          aesAlg.Clear();
      }

      return plaintext;
    }

    private byte[] ReadByteArray(Stream s)
    {
      byte[] rawLength = new byte[sizeof(int)];
      if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
      {
        throw new SystemException("Stream did not contain properly formatted byte array");
      }

      byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
      if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
      {
        throw new SystemException("Did not read byte array properly");
      }

      return buffer;
    }
  }
}

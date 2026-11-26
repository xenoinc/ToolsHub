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
using System.Security.Cryptography;
using System.Text;
using Xeno.Dockzi.Services.Logging;

namespace Xeno.Dockzi.VeraCryptAddin.Domain;

public class Crypto
{
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
}

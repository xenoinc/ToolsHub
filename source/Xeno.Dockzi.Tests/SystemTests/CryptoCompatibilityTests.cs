namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.VeraCryptAddin.Domain;

[TestClass]
public class CryptoCompatibilityTests
{
  private const string Password = "Dockzi legacy compatibility password";
  private const string LegacyPlainText = "Dockzi legacy AES compatibility test";

  // Generated with .NET Framework RijndaelManaged and the original Rfc2898DeriveBytes
  // constructor, using the original salt, default settings, and IV bytes 0 through 15.
  private const string LegacyCipherText = "EAAAAAABAgMEBQYHCAkKCwwNDg/YCBu0cdqUuA5K8J/BxWbDqbDOFpIK2dp8mrEfNyvmCdkc/0PDojen63gsupsp+UM=";

  [TestMethod]
  public void ReadsCiphertextFromTheOriginalFrameworkImplementation()
  {
    Assert.AreEqual(LegacyPlainText, Helper("DecryptStringAES")(LegacyCipherText, Password));
  }

  [TestMethod]
  [DataRow("A short value")]
  [DataRow("Unicode: \u03a3 \u00e9 \u4e2d\u6587 \ud83c\udf45\nSecond line")]
  [DataRow("0123456789abcdef0123456789abcdef")]
  public void WritesTheOriginalEnvelopeAndKeyFormatWithAFreshIv(string text)
  {
    var encrypt = Helper("EncryptStringAES");
    string first = encrypt(text, Password);
    string second = encrypt(text, Password);
    Assert.AreNotEqual(first, second);
    Assert.AreEqual(text, Helper("DecryptStringAES")(first, Password));

    byte[] envelope = Convert.FromBase64String(first);
    Assert.AreEqual(16, BinaryPrimitives.ReadInt32LittleEndian(envelope));
    Assert.IsFalse(envelope.AsSpan(4, 16).SequenceEqual(Convert.FromBase64String(second).AsSpan(4, 16)));

    // Independently derived by the original .NET Framework constructor, not by the helper under test.
    using var aes = Aes.Create();
    aes.Key = Convert.FromHexString("066EECDD13C08911664BA153A8C76CC794604AB1B16578210AAB959DAACD41FD");
    byte[] restored = aes.DecryptCbc(envelope.AsSpan(20), envelope.AsSpan(4, 16), PaddingMode.PKCS7);
    Assert.AreEqual(text, Encoding.UTF8.GetString(restored));
  }

  [TestMethod]
  [DataRow(-1)]
  [DataRow(0)]
  [DataRow(15)]
  [DataRow(32)]
  [DataRow(int.MaxValue)]
  public void RejectsInvalidIvLengthsBeforeAllocatingFromInput(int length)
  {
    byte[] envelope = new byte[20];
    BinaryPrimitives.WriteInt32LittleEndian(envelope, length);
    Assert.ThrowsExactly<CryptographicException>(() => Helper("DecryptStringAES")(Convert.ToBase64String(envelope), Password));
  }

  [TestMethod]
  public void RejectsTruncatedAndInvalidCiphertext()
  {
    var decrypt = Helper("DecryptStringAES");
    Assert.ThrowsExactly<FormatException>(() => decrypt("not base64", Password));
    Assert.ThrowsExactly<EndOfStreamException>(() => decrypt(Convert.ToBase64String(new byte[3]), Password));
    Assert.ThrowsExactly<EndOfStreamException>(() => decrypt(Convert.ToBase64String(new byte[] { 16, 0, 0, 0 }), Password));

    byte[] envelope = Convert.FromBase64String(LegacyCipherText);
    Assert.ThrowsExactly<CryptographicException>(() => decrypt(Convert.ToBase64String(envelope.AsSpan(0, 20)), Password));
    Assert.ThrowsExactly<CryptographicException>(() => decrypt(Convert.ToBase64String(envelope.AsSpan(0, envelope.Length - 1)), Password));
  }

  [TestMethod]
  [DataRow("EncryptStringAES")]
  [DataRow("DecryptStringAES")]
  public void PreservesArgumentValidation(string name)
  {
    var helper = Helper(name);
    Assert.ThrowsExactly<ArgumentNullException>(() => helper(null, Password));
    Assert.ThrowsExactly<ArgumentNullException>(() => helper(string.Empty, Password));
    Assert.ThrowsExactly<ArgumentNullException>(() => helper(LegacyCipherText, null));
    Assert.ThrowsExactly<ArgumentNullException>(() => helper(LegacyCipherText, string.Empty));
  }

  private static Func<string, string, string> Helper(string name)
  {
    MethodInfo method = typeof(Crypto).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.IsNotNull(method);
    return method.CreateDelegate<Func<string, string, string>>(new Crypto());
  }
}

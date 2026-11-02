namespace Xeno.ToolsHub.Tests.SystemTests;

using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Managers;

[TestClass]
[DoNotParallelize]
public class JsonCompatibilityTests
{
  [TestInitialize]
  public void Initialize() => TestHelpers.PrepareTestsFolder();

  [TestCleanup]
  public void Cleanup() => TestHelpers.CleanTestsFolder();

  [TestMethod]
  public void LegacySettingsAndEncodedShortcutsSurviveSaveAndReload()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    const string shortcuts = "[{\"Title\":\"SDR++ café\",\"Target\":\"C:\\\\Apps\\\\SDR++\\\\sdrpp.exe\",\"Params\":\"--name \\\"test\\\"\",\"RunAsAdmin\":true}]";
    string encoded = Helpers.Base64Encode(shortcuts);

    // This is the existing settings-file shape, including the dictionary and opaque values.
    File.WriteAllText(settings.SettingsFilePath,
      "[{\"Items\":{\"ShortcutItems\":\"" + encoded + "\",\"Secret\":\"opaque+/=\"},\"Id\":\"Shortcuts\"}]");
    settings.LoadFile();
    var items = settings.GetObject<ShortcutItems>("Shortcuts", "ShortcutItems");
    Assert.HasCount(1, items);
    Assert.AreEqual("SDR++ café", items[0].Title);
    Assert.AreEqual("C:\\Apps\\SDR++\\sdrpp.exe", items[0].Target);
    Assert.AreEqual("--name \"test\"", items[0].Params);
    Assert.IsTrue(items[0].RunAsAdmin);

    settings.SetObject("Shortcuts", "ShortcutItems", items);
    settings.SaveFile();
    var reloaded = new SettingsManager(StorageMethod.UnitTest);
    reloaded.LoadFile();
    Assert.AreEqual("opaque+/=", reloaded.GetValue("Shortcuts", "Secret"));
    Assert.AreEqual(items[0].Title, reloaded.GetObject<ShortcutItems>("Shortcuts", "ShortcutItems")[0].Title);
    using var saved = JsonDocument.Parse(File.ReadAllText(settings.SettingsFilePath));
    Assert.AreEqual("Shortcuts", saved.RootElement[0].GetProperty("Id").GetString());
    Assert.AreEqual(JsonValueKind.Object, saved.RootElement[0].GetProperty("Items").ValueKind);
  }

  [TestMethod]
  public void EmptyAndNullObjectsReturnDefaultsWhileCorruptionThrows()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    Assert.IsNull(settings.GetObject<ShortcutItems>("test", "missing"));
    foreach (string json in new[] { string.Empty, "  ", "null" })
    {
      settings.SetValue("test", "items", Helpers.Base64Encode(json));
      Assert.IsNull(settings.GetObject<ShortcutItems>("test", "items"));
    }

    settings.SetValue("test", "items", "invalid base64!");
    Assert.ThrowsExactly<FormatException>(() => settings.GetObject<ShortcutItems>("test", "items"));
    settings.SetValue("test", "items", Helpers.Base64Encode("[broken"));
    Assert.ThrowsExactly<JsonException>(() => settings.GetObject<ShortcutItems>("test", "items"));
    Assert.IsNull(Helpers.ToObject<ShortcutItems>(" "));
  }

  [TestMethod]
  public void LegacyJsonOptionsAndFileHelpersPreserveRuntimeObjectProperties()
  {
    const string json = "[/* legacy */{\"title\":\"Launcher\",\"target\":\"app.exe\",},]";
    var items = (ShortcutItems)Helpers.ToObject<ShortcutItems>(json);
    Assert.AreEqual("Launcher", items[0].Title);
    string path = Path.Combine(Helpers.GetStorageFolder(StorageMethod.UnitTest), "shortcuts.json");
    object value = items;
    string formatted = Helpers.FileSerialize(value, path);
    Assert.IsTrue(formatted.Contains('\n'));
    Assert.IsFalse(Helpers.ToJson(value).Contains('\n'));
    Assert.AreEqual("app.exe", Helpers.FileDeserialize<ShortcutItems>(path)[0].Target);
    var manager = new ShortcutsManager(new SettingsManager(StorageMethod.UnitTest));
    Assert.IsTrue(manager.ValidateJson(json, out _));
    Assert.IsFalse(manager.ValidateJson("[broken", out string error));
    Assert.IsFalse(string.IsNullOrEmpty(error));
  }

  [TestMethod]
  public void NullSettingsFileLeavesStoreUsable()
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    File.WriteAllText(settings.SettingsFilePath, "null");
    settings.LoadFile();
    settings.SetValue("test", "key", "value");
    Assert.AreEqual("value", settings.GetValue("test", "key"));
  }
}

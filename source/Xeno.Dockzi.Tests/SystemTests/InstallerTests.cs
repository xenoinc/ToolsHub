namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Services;

[TestClass]
public class InstallerTests
{
  [TestMethod]
  public void PackagedVersionsSharePersistentDataOutsideTheirApplicationDirectories()
  {
    WithFolder(root =>
    {
      string first = Path.Combine(root, "v1", "current");
      string second = Path.Combine(root, "v2", "current");
      Directory.CreateDirectory(first);
      Directory.CreateDirectory(second);
      File.WriteAllText(Path.Combine(first, "Dockzi.install.json"), "{}");
      File.WriteAllText(Path.Combine(second, "Dockzi.install.json"), "{}");

      string local = Path.Combine(root, "user-data");
      var oldPaths = new ApplicationPaths(first, local);
      var newPaths = new ApplicationPaths(second, local);

      Assert.IsFalse(oldPaths.IsPortable);
      Assert.AreEqual(Config.StorageMethod.SingleUser, SettingsLoader.SelectStorageMethod(oldPaths));
      Assert.AreEqual(Path.Combine(local, "XI", "Dockzi"), oldPaths.DataDirectory);
      Assert.AreEqual(oldPaths.DataDirectory, newPaths.DataDirectory);
      Assert.AreEqual(oldPaths.UserAddinsDirectory, newPaths.UserAddinsDirectory);
      Assert.AreEqual(oldPaths.AddinRegistryDirectory, newPaths.AddinRegistryDirectory);
      Assert.IsFalse(newPaths.DownloadsDirectory.StartsWith(second, StringComparison.OrdinalIgnoreCase));
      Assert.IsFalse(newPaths.LogsDirectory.StartsWith(second, StringComparison.OrdinalIgnoreCase));
    });
  }

  [TestMethod]
  public void PortableMarkerExplicitlySelectsPortableDataAndStartupUsesStableInstalledStub()
  {
    WithFolder(root =>
    {
      string app = Path.Combine(root, "current");
      string portableData = Path.Combine(root, "portable-data");
      Directory.CreateDirectory(app);
      var development = new ApplicationPaths(app, root, portableData);

      Assert.IsTrue(development.IsPortable);
      Assert.AreEqual(portableData, development.DataDirectory);

      File.WriteAllText(Path.Combine(app, "Dockzi.install.json"), "{}");
      File.WriteAllText(Path.Combine(root, "Dockzi.exe"), string.Empty);
      Assert.AreEqual(Path.Combine(root, "Dockzi.exe"), new ApplicationPaths(app, root).LauncherPath);
      File.WriteAllText(Path.Combine(app, "Dockzi.portable"), string.Empty);
      var portable = new ApplicationPaths(app, root, portableData);

      Assert.AreEqual(portableData, portable.DataDirectory);
      Assert.AreEqual(Config.StorageMethod.PortableApp, SettingsLoader.SelectStorageMethod(portable));
    });
  }

  [TestMethod]
  public void InstallationConfigurationSupportsOfflineInstallAndHttpsUpdateFeed()
  {
    WithFolder(root =>
    {
      Assert.AreEqual(string.Empty, InstallationConfiguration.Load(root).UpdateUrl);
      string path = Path.Combine(root, "Dockzi.install.json");
      File.WriteAllText(path, "{\"UpdateUrl\":\"https://updates.example.com/dockzi/\",\"Channel\":\"win\"}");
      var configuration = InstallationConfiguration.Load(root);

      Assert.AreEqual("https://updates.example.com/dockzi/", configuration.UpdateUrl);
      Assert.AreEqual("win", configuration.Channel);

      File.WriteAllText(path, "{\"UpdateUrl\":\"http://updates.example.com/\"}");
      Assert.ThrowsExactly<InvalidDataException>(() => InstallationConfiguration.Load(root));

      File.WriteAllText(path, "{\"UpdateUrl\":\"C:\\\\packages\"}");
      Assert.ThrowsExactly<InvalidDataException>(() => InstallationConfiguration.Load(root));
    });
  }

  private static void WithFolder(Action<string> action)
  {
    string root = Path.Combine(Path.GetTempPath(), "DockziInstallerTests-" + Guid.NewGuid().ToString("N"));
    Directory.CreateDirectory(root);
    try
    {
      action(root);
    }
    finally
    {
      Directory.Delete(root, true);
    }
  }
}

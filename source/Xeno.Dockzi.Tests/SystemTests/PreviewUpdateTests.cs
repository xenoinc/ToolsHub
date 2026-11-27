namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Velopack.Sources;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.Tests.Helpers;
using Xeno.Dockzi.Views.Preferences;

[TestClass]
[DoNotParallelize]
[OSCondition(OperatingSystems.Windows)]
public class PreviewUpdateTests
{
  [TestCleanup]
  public void Cleanup() => TestHelpers.CleanTestsFolder();

  [TestMethod]
  public void DefaultIsOffAndDiscardingThePageDoesNotSaveTheChoice()
  {
    RunSta(() =>
    {
      var settings = new SettingsManager(StorageMethod.UnitTest);
      var preferences = new UpdatePreferences(settings);
      using (var page = new GeneralPreferencesPage(new FakeStartup(), preferences))
      {
        var checkbox = FindPreview(page);
        Assert.IsFalse(checkbox.Checked);
        checkbox.Checked = true;
      }

      Assert.IsFalse(preferences.ReceivePreviewUpdates);
      Assert.IsFalse(File.Exists(settings.SettingsFilePath));
    });
  }

  [TestMethod]
  public void FailedSaveRollsBackCachedPolicyAndAllowsRetry()
  {
    RunSta(() =>
    {
      var settings = new SettingsManager(StorageMethod.UnitTest);
      var preferences = new UpdatePreferences(settings);
      using var page = new GeneralPreferencesPage(new FakeStartup(), preferences);

      Directory.CreateDirectory(settings.SettingsFilePath);
      FindPreview(page).Checked = true;

      Assert.IsFalse(page.TrySave(out string error));
      Assert.Contains("Unable to save update settings", error);
      Assert.IsFalse(preferences.ReceivePreviewUpdates);

      Directory.Delete(settings.SettingsFilePath);
      Assert.IsTrue(page.TrySave(out _));
      Assert.IsTrue(preferences.ReceivePreviewUpdates);
    });
  }

  [TestMethod]
  public void GeneralSettingSavesAndReloadsWithoutAnyAddinChanges()
  {
    RunSta(() =>
    {
      var settings = new SettingsManager(StorageMethod.UnitTest);
      using var page = new GeneralPreferencesPage(new FakeStartup(), new UpdatePreferences(settings));
      FindPreview(page).Checked = true;

      Assert.IsTrue(page.TrySave(out _));

      var reloaded = new SettingsManager(StorageMethod.UnitTest);
      reloaded.LoadFile();

      Assert.AreEqual("Dockzi.config", Path.GetFileName(reloaded.SettingsFilePath));
      Assert.IsTrue(new UpdatePreferences(reloaded).ReceivePreviewUpdates);

      FindPreview(page).Checked = false;
      Assert.IsTrue(page.TrySave(out _));

      reloaded.LoadFile();
      Assert.IsFalse(new UpdatePreferences(reloaded).ReceivePreviewUpdates);
    });
  }

  [TestMethod]
  public void GithubSourceIncludesPrereleasesOnlyWhenOptedIn()
  {
    var configuration = new InstallationConfiguration { UpdateUrl = "https://github.com/xenoinc/ToolsHub" };
    Assert.IsFalse(((GithubSource)configuration.CreateUpdateSource()).Prerelease);
    Assert.IsTrue(((GithubSource)configuration.CreateUpdateSource(true)).Prerelease);
    Assert.IsFalse(((GithubSource)configuration.CreateUpdateSource(false)).Prerelease);
  }

  [TestInitialize]
  public void Initialize()
  {
    TestHelpers.CleanTestsFolder();
    TestHelpers.PrepareTestsFolder();
  }

  [TestMethod]
  public void StartupRegistryFailureDoesNotDisablePreviewPreferences()
  {
    RunSta(() =>
    {
      var preferences = new UpdatePreferences(new SettingsManager(StorageMethod.UnitTest));
      using var page = new GeneralPreferencesPage(new FakeStartup { DenyRead = true }, preferences);
      FindPreview(page).Checked = true;

      Assert.IsTrue(page.TrySave(out _));
      Assert.IsTrue(preferences.ReceivePreviewUpdates);
    });
  }

  private static CheckBox FindPreview(GeneralPreferencesPage page) =>
    (CheckBox)page.Controls.Find("ReceivePreviewUpdates", true)[0];

  private static void RunSta(Action action)
  {
    Exception failure = null;
    var thread = new Thread(() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        failure = ex;
      }
    });

    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();

    if (failure != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(failure).Throw();
  }

  private sealed class FakeStartup : IAutoStartService
  {
    public bool DenyRead { get; set; }

    public bool IsEnabled => DenyRead ? throw new IOException("Test registry access denied.") : false;

    public void RepairRegistration()
    {
    }

    public void SetEnabled(bool enabled) => throw new InvalidOperationException("Unexpected startup change.");
  }
}

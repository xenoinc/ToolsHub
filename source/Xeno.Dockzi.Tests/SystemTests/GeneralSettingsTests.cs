namespace Xeno.Dockzi.Tests.SystemTests
{
  using System;
  using System.IO;
  using System.Threading;
  using System.Windows.Forms;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Microsoft.Win32;
  using Xeno.Dockzi.Services;
  using Xeno.Dockzi.Views.Preferences;

  [TestClass]
  [OSCondition(OperatingSystems.Windows)]
  public class GeneralSettingsTests
  {
    [TestMethod]
    public void StartupRegistrationQuotesExecutableAndPreservesOtherEntries()
    {
      string testPath = @"Software\Xeno.Dockzi.Tests\AutoStart-" + Guid.NewGuid().ToString("N");
      try
      {
        using (var key = Registry.CurrentUser.CreateSubKey(testPath))
          key.SetValue("OtherApp", "leave this alone");

        var service = new AutoStartService(@"C:\Program Files\Dockzi\Dockzi.exe", testPath, "TestApp");
        Assert.IsFalse(service.IsEnabled);

        service.SetEnabled(false);
        service.SetEnabled(true);
        Assert.IsTrue(service.IsEnabled);
        service.SetEnabled(true);

        using (var key = Registry.CurrentUser.OpenSubKey(testPath))
          Assert.AreEqual(@"""C:\Program Files\Dockzi\Dockzi.exe"" --auto-start", key.GetValue("TestApp"));

        service.SetEnabled(false);
        service.SetEnabled(false);
        Assert.IsFalse(service.IsEnabled);

        using (var key = Registry.CurrentUser.OpenSubKey(testPath))
          Assert.AreEqual("leave this alone", key.GetValue("OtherApp"));
      }
      finally
      {
        Registry.CurrentUser.DeleteSubKeyTree(testPath, false);
      }
    }

    [TestMethod]
    public void LegacyStartupRegistrationMigratesOnlyTheSameInstallation()
    {
      string testPath = @"Software\Xeno.Dockzi.Tests\AutoStart-" + Guid.NewGuid().ToString("N");
      try
      {
        using (var key = Registry.CurrentUser.CreateSubKey(testPath))
        {
          key.SetValue("ToolsHub", @"""C:\Apps\ToolsHub.exe"" --auto-start");
          key.SetValue("OtherApp", "leave this alone");
        }

        var service = new AutoStartService(@"C:\Apps\Dockzi.exe", testPath, "Dockzi", "ToolsHub");
        Assert.IsTrue(service.IsEnabled);
        using (var key = Registry.CurrentUser.OpenSubKey(testPath))
        {
          Assert.AreEqual(@"""C:\Apps\Dockzi.exe"" --auto-start", key.GetValue("Dockzi"));
          Assert.IsNull(key.GetValue("ToolsHub"));
          Assert.AreEqual("leave this alone", key.GetValue("OtherApp"));
        }

        service.SetEnabled(false);
        using (var key = Registry.CurrentUser.CreateSubKey(testPath))
          key.SetValue("ToolsHub", @"""C:\OtherInstallation\ToolsHub.exe"" --auto-start");

        service = new AutoStartService(@"C:\Apps\Dockzi.exe", testPath, "Dockzi", "ToolsHub");
        Assert.IsFalse(service.IsEnabled);

        using (var key = Registry.CurrentUser.OpenSubKey(testPath))
          Assert.AreEqual(@"""C:\OtherInstallation\ToolsHub.exe"" --auto-start", key.GetValue("ToolsHub"));
      }
      finally
      {
        Registry.CurrentUser.DeleteSubKeyTree(testPath, false);
      }
    }

    [TestMethod]
    public void StartupRepairReplacesMissingLauncherAndPreservesOtherEntries()
    {
      string testPath = @"Software\Xeno.Dockzi.Tests\AutoStart-" + Guid.NewGuid().ToString("N");
      string folder = Path.Combine(Path.GetTempPath(), "DockziAutoStartTests-" + Guid.NewGuid().ToString("N"));

      Directory.CreateDirectory(folder);

      string launcher = Path.Combine(folder, "Dockzi.exe");
      string oldLauncher = Path.Combine(folder, "removed", "Dockzi.exe");
      File.WriteAllText(launcher, string.Empty);

      try
      {
        using (var key = Registry.CurrentUser.CreateSubKey(testPath))
        {
          key.SetValue("Dockzi", $"\"{oldLauncher}\" --auto-start");
          key.SetValue("OtherApp", "leave this alone");
        }

        var service = new AutoStartService(launcher, testPath, "Dockzi");
        service.RepairRegistration();
        service.RepairRegistration();
        using var repaired = Registry.CurrentUser.OpenSubKey(testPath);

        Assert.AreEqual($"\"{launcher}\" --auto-start", repaired.GetValue("Dockzi"));
        Assert.AreEqual("leave this alone", repaired.GetValue("OtherApp"));
      }
      finally
      {
        Registry.CurrentUser.DeleteSubKeyTree(testPath, false);
        Directory.Delete(folder, true);
      }
    }

    [TestMethod]
    public void StartupRepairPreservesDisabledOtherInstallationsAndCustomCommands()
    {
      string testPath = @"Software\Xeno.Dockzi.Tests\AutoStart-" + Guid.NewGuid().ToString("N");
      string folder = Path.Combine(Path.GetTempPath(), "DockziAutoStartTests-" + Guid.NewGuid().ToString("N"));

      Directory.CreateDirectory(folder);

      string launcher = Path.Combine(folder, "Dockzi.exe");
      string otherLauncher = Path.Combine(folder, "other", "Dockzi.exe");

      Directory.CreateDirectory(Path.GetDirectoryName(otherLauncher));
      File.WriteAllText(launcher, string.Empty);
      File.WriteAllText(otherLauncher, string.Empty);

      try
      {
        var service = new AutoStartService(launcher, testPath, "Dockzi");
        service.RepairRegistration();

        Assert.IsFalse(service.IsEnabled);

        using var key = Registry.CurrentUser.CreateSubKey(testPath);

        foreach (string command in new[] { string.Empty, $"\"{otherLauncher}\" --auto-start", $"\"{launcher}\" --custom", "custom launcher command" })
        {
          key.SetValue("Dockzi", command);
          service.RepairRegistration();
          Assert.AreEqual(command, key.GetValue("Dockzi"));
        }

        File.Delete(launcher);
        string missingCommand = $"\"{Path.Combine(folder, "removed", "Dockzi.exe")}\" --auto-start";
        key.SetValue("Dockzi", missingCommand);
        service.RepairRegistration();

        Assert.AreEqual(missingCommand, key.GetValue("Dockzi"));
      }
      finally
      {
        Registry.CurrentUser.DeleteSubKeyTree(testPath, false);
        Directory.Delete(folder, true);
      }
    }

    [TestMethod]
    public void SavingAlreadyEnabledStartupRepairsRegistrationAndReportsFailures()
    {
      RunSta(() =>
      {
        var service = new FakeAutoStartService { IsEnabled = true, FailSave = true };
        using var page = new GeneralPreferencesPage(service);

        Assert.IsFalse(page.TrySave(out string error));
        Assert.Contains("Unable to update Windows startup settings", error);

        service.FailSave = false;
        Assert.IsTrue(page.TrySave(out _));
        Assert.AreEqual(1, service.RepairCount);
        Assert.AreEqual(0, service.SaveCount);
      });
    }

    [TestMethod]
    public void GeneralPageDefersChangesUntilSaveAndSupportsDisabling()
    {
      RunSta(() =>
      {
        var service = new FakeAutoStartService();
        using var page = new GeneralPreferencesPage(service);
        var checkBox = (CheckBox)page.Controls.Find("EnableAutoStart", true)[0];

        Assert.IsFalse(checkBox.Checked);

        checkBox.Checked = true;

        Assert.AreEqual(0, service.SaveCount);
        Assert.IsTrue(page.TrySave(out _));
        Assert.IsTrue(service.IsEnabled);
        Assert.AreEqual(1, service.SaveCount);
        Assert.IsTrue(page.TrySave(out _));
        Assert.AreEqual(1, service.SaveCount);

        checkBox.Checked = false;
        Assert.IsTrue(page.TrySave(out _));
        Assert.IsFalse(service.IsEnabled);
      });
    }

    [TestMethod]
    public void DiscardedPageDoesNotChangeStartupRegistration()
    {
      RunSta(() =>
      {
        var service = new FakeAutoStartService { IsEnabled = true };
        using (var page = new GeneralPreferencesPage(service))
        {
          var checkBox = (CheckBox)page.Controls.Find("EnableAutoStart", true)[0];
          Assert.IsTrue(checkBox.Checked);
          checkBox.Checked = false;
        }

        Assert.IsTrue(service.IsEnabled);
        Assert.AreEqual(0, service.SaveCount);
      });
    }

    [TestMethod]
    public void FailedSaveReportsErrorAndAllowsRetry()
    {
      RunSta(() =>
      {
        var service = new FakeAutoStartService { FailSave = true };
        using var page = new GeneralPreferencesPage(service);
        ((CheckBox)page.Controls.Find("EnableAutoStart", true)[0]).Checked = true;

        Assert.IsFalse(page.TrySave(out string error));
        Assert.Contains("Unable to update Windows startup settings", error);
        Assert.IsFalse(service.IsEnabled);

        service.FailSave = false;
        Assert.IsTrue(page.TrySave(out _));
        Assert.IsTrue(service.IsEnabled);
      });
    }

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

    private sealed class FakeAutoStartService : IAutoStartService
    {
      public bool IsEnabled { get; set; }

      public bool FailSave { get; set; }

      public int SaveCount { get; private set; }

      public int RepairCount { get; private set; }

      public void RepairRegistration()
      {
        if (FailSave)
          throw new IOException("Startup settings unavailable.");

        RepairCount++;
      }

      public void SetEnabled(bool enabled)
      {
        if (FailSave)
          throw new IOException("Startup settings unavailable.");

        IsEnabled = enabled;
        SaveCount++;
      }
    }
  }
}

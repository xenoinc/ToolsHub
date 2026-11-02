namespace Xeno.ToolsHub.Tests.SystemTests
{
  using System;
  using System.IO;
  using System.Threading;
  using System.Windows.Forms;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Microsoft.Win32;
  using Xeno.ToolsHub.Services;
  using Xeno.ToolsHub.Views.Preferences;

  [TestClass]
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public class GeneralSettingsTests
  {
    [TestMethod]
    public void StartupRegistrationQuotesExecutableAndPreservesOtherEntries()
    {
      string testPath = @"Software\Xeno.ToolsHub.Tests\AutoStart-" + Guid.NewGuid().ToString("N");
      try
      {
        using (var key = Registry.CurrentUser.CreateSubKey(testPath))
          key.SetValue("OtherApp", "leave this alone");

        var service = new AutoStartService(@"C:\Program Files\ToolsHub\ToolsHub.exe", testPath, "TestApp");
        Assert.IsFalse(service.IsEnabled);
        service.SetEnabled(false);
        service.SetEnabled(true);
        Assert.IsTrue(service.IsEnabled);
        service.SetEnabled(true);
        using (var key = Registry.CurrentUser.OpenSubKey(testPath))
          Assert.AreEqual(@"""C:\Program Files\ToolsHub\ToolsHub.exe"" --auto-start", key.GetValue("TestApp"));

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
        StringAssert.Contains(error, "Unable to update Windows startup settings");
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

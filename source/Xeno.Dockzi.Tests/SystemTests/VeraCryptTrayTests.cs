namespace Xeno.Dockzi.Tests.SystemTests;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Lite.EventIpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xeno.Dockzi.Config;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services;
using Xeno.Dockzi.VeraCryptAddin;
using Xeno.Dockzi.VeraCryptAddin.Handlers;
using Xeno.Dockzi.VeraCryptAddin.Services;

[TestClass]
[OSCondition(OperatingSystems.Windows)]
public class VeraCryptTrayTests
{
  [TestMethod]
  public void TrayReflectsExternalMountChangesAndOpensEveryMountedDrive()
  {
    RunSta(() =>
    {
      var operations = new FakeOperations();
      var manager = CreateManager(operations);
      using var menu = (ToolStripMenuItem)new SysTrayHandler(manager).MenuItems()[0];
      Assert.IsTrue(menu.DropDownItems[0].Enabled);
      Assert.IsFalse(menu.DropDownItems[1].Enabled);
      Assert.IsFalse(menu.DropDownItems[3].Enabled);

      operations.Mounted.Add(@"Z:\");
      operations.Mounted.Add(@"Y:\");
      Refresh(menu);
      Assert.IsFalse(menu.DropDownItems[0].Enabled);
      Assert.IsTrue(menu.DropDownItems[1].Enabled);
      var explore = (ToolStripMenuItem)menu.DropDownItems[3];
      Assert.IsTrue(explore.Enabled);
      Assert.HasCount(2, explore.DropDownItems);
      explore.DropDownItems[1].PerformClick();
      Assert.AreEqual(@"Y:\", operations.Opened);

      operations.Mounted.Clear();
      Refresh(menu);
      Assert.IsTrue(menu.DropDownItems[0].Enabled);
      Assert.IsFalse(menu.DropDownItems[1].Enabled);
      Assert.IsFalse(explore.Enabled);
      Assert.HasCount(0, explore.DropDownItems);
    });
  }

  [TestMethod]
  public void OccupiedNonVeraCryptLetterCannotBeMountedDismountedOrOpened()
  {
    var operations = new FakeOperations { Occupied = true };
    var manager = CreateManager(operations);
    Assert.IsFalse(manager.CanMount);
    Assert.IsFalse(manager.CanDismount);
    Assert.AreEqual(-1, manager.OnMount(string.Empty));
    Assert.AreEqual(-1, manager.OnDismount(string.Empty));
    Assert.AreEqual(-1, manager.OnOpenInExplorer("Z"));
    Assert.AreEqual(0, operations.MountCount);
    Assert.AreEqual(0, operations.DismountCount);
    Assert.IsNull(operations.Opened);
  }

  [TestMethod]
  public void CommandsRecheckStatusAndPermitBrowsingWithoutTheExecutable()
  {
    var operations = new FakeOperations();
    var manager = CreateManager(operations);
    Assert.AreEqual(0, manager.OnMount(string.Empty));
    Assert.IsTrue(manager.CanDismount);
    Assert.AreEqual(-1, manager.OnMount(string.Empty));
    operations.Installed = false;
    Assert.IsFalse(manager.CanMount);
    Assert.IsFalse(manager.CanDismount);
    Assert.AreEqual(0, manager.OnOpenInExplorer("z:"));
    Assert.AreEqual(@"Z:\", operations.Opened);
    operations.Installed = true;
    Assert.AreEqual(0, manager.OnDismount(string.Empty));
    Assert.AreEqual(-1, manager.OnOpenInExplorer("Z"));
    Assert.AreEqual(-1, manager.OnDismount(string.Empty));
  }

  [TestMethod]
  [DataRow(false, 0, true, false)]
  [DataRow(true, 0, true, true)]
  [DataRow(true, 1, true, false)]
  [DataRow(true, 0, false, false)]
  public void AutoOpenRequiresTheSettingASuccessfulExitAndAMountedDrive(bool enabled, int exitCode, bool mounted, bool opens)
  {
    var operations = new FakeOperations { MountResult = exitCode, ReportMountedAfterMount = mounted };
    var manager = CreateManager(operations, openAfterMount: enabled);
    Assert.AreEqual(exitCode, manager.OnMount(string.Empty));
    Assert.AreEqual(opens ? @"Z:\" : null, operations.Opened);
  }

  [TestMethod]
  public void ExplorerFailureDoesNotTurnASuccessfulMountIntoAFailure()
  {
    var operations = new FakeOperations { FailOpen = true };
    Assert.AreEqual(0, CreateManager(operations, openAfterMount: true).OnMount(string.Empty));
    Assert.HasCount(1, operations.Mounted);
  }

  [TestMethod]
  [DataRow("z")]
  [DataRow("Z:")]
  [DataRow("z:\\")]
  public void ExplorerReceivesOnlyTheValidatedDriveRoot(string drive)
  {
    var info = VeraCryptOperations.CreateExplorerStartInfo(drive);
    Assert.IsTrue(info.UseShellExecute);
    Assert.AreEqual("explorer.exe", System.IO.Path.GetFileName(info.FileName));
    Assert.HasCount(1, info.ArgumentList);
    Assert.AreEqual(@"Z:\", info.ArgumentList[0]);
  }

  [TestMethod]
  [DataRow("")]
  [DataRow("Z:\\other")]
  [DataRow("Z & calc.exe")]
  [DataRow("\\\\server\\share")]
  public void InvalidDriveTargetsAreRejected(string drive)
  {
    Assert.IsNull(VeraCryptOperations.NormalizeDriveRoot(drive));
    Assert.ThrowsExactly<ArgumentException>(() => VeraCryptOperations.CreateExplorerStartInfo(drive));
    Assert.IsFalse(CreateManager(new FakeOperations(), drive).CanMount);
  }

  private static VeraCryptManager CreateManager(FakeOperations operations, string drive = "Z", bool openAfterMount = false)
  {
    var settings = new SettingsManager(StorageMethod.UnitTest);
    settings.SetValue("VeraCryptAddin", "HcDrive", drive);
    settings.SetValue("VeraCryptAddin", "HcPath", "volume.hc");
    settings.SetValue("VeraCryptAddin", "OpenExplorerAfterMount", openAfterMount.ToString());
    return new VeraCryptManager(new ApplicationSettings(settings), new EventAggregator(), operations, new PlainSecrets());
  }

  private static void Refresh(ToolStripMenuItem menu) =>
    typeof(ToolStripDropDownItem).GetMethod("OnDropDownShow", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(menu, new object[] { EventArgs.Empty });

  private static void RunSta(Action action)
  {
    Exception error = null;
    var thread = new Thread(() =>
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        error = ex;
      }
    });
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();
    if (error != null)
      System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error).Throw();
  }

  private sealed class PlainSecrets : ISecretProtector
  {
    public string Encrypt(string value) => value;

    public string Decrypt(string value) => value;
  }

  private sealed class FakeOperations : IVeraCryptOperations
  {
    public List<string> Mounted { get; } = new();

    public int MountResult { get; set; }

    public bool ReportMountedAfterMount { get; set; } = true;

    public bool FailOpen { get; set; }

    public bool Occupied { get; set; }

    public bool Installed { get; set; } = true;

    public string Opened { get; private set; }

    public int MountCount { get; private set; }

    public int DismountCount { get; private set; }

    public bool IsInstalled(string executable) => Installed;

    public bool IsDriveInUse(string drive) => Occupied || Mounted.Contains(drive);

    public IReadOnlyList<string> GetMountedDrives() => Mounted;

    public void OpenInExplorer(string drive)
    {
      if (FailOpen)
        throw new InvalidOperationException("Test Explorer launch failure.");

      Opened = drive;
    }

    public int Mount(string executable, string drive, string path, string password)
    {
      MountCount++;
      if (MountResult == 0 && ReportMountedAfterMount)
        Mounted.Add(VeraCryptOperations.NormalizeDriveRoot(drive));

      return MountResult;
    }

    public int Dismount(string executable, string drive, string path, bool force)
    {
      DismountCount++;
      Mounted.Remove(VeraCryptOperations.NormalizeDriveRoot(drive));
      return 0;
    }
  }
}

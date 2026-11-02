namespace Xeno.ToolsHub.Tests.SystemTests.Shortcuts
{
  using System;
  using System.IO;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.LocalAddins.Shortcuts;

  [TestClass]
  public class ShortcutLauncherTests
  {
    [TestMethod]
    public void ExecutableStartsInItsOwnFolderWithArgumentsAndElevation()
    {
      string folder = Path.Combine(Path.GetTempPath(), "ToolsHub launch " + Guid.NewGuid().ToString("N"));
      Directory.CreateDirectory(folder);
      string target = Path.Combine(folder, "portable.exe");
      try
      {
        File.WriteAllText(target, string.Empty);
        var options = ShortcutLauncher.CreateStartInfo(new ShortcutItem("Portable", target, "--profile \"My profile\"")
        {
          RunAsAdmin = true,
        });

        Assert.AreEqual(target, options.FileName);
        Assert.AreEqual(folder, options.WorkingDirectory);
        Assert.AreEqual("--profile \"My profile\"", options.Arguments);
        Assert.IsTrue(options.UseShellExecute);
        Assert.AreEqual("runas", options.Verb);
      }
      finally
      {
        File.Delete(target);
        Directory.Delete(folder);
      }
    }

    [TestMethod]
    public void UrlIsPassedToWindowsShell()
    {
      var options = ShortcutLauncher.CreateStartInfo(new ShortcutItem { Target = "https://example.com/path" });
      Assert.AreEqual("https://example.com/path", options.FileName);
      Assert.IsTrue(options.UseShellExecute);
      Assert.AreEqual(string.Empty, options.WorkingDirectory);
      Assert.AreEqual(string.Empty, options.Arguments);
      Assert.AreEqual(string.Empty, options.Verb);
    }

    [TestMethod]
    public void FolderIsOpenedThroughWindowsShell()
    {
      string folder = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar);
      var options = ShortcutLauncher.CreateStartInfo(new ShortcutItem { Target = folder });
      Assert.AreEqual(folder, options.FileName);
      Assert.AreEqual(folder, options.WorkingDirectory);
      Assert.IsTrue(options.UseShellExecute);
    }

    [TestMethod]
    public void EnvironmentVariablesAreExpandedForExecutableTargets()
    {
      string folder = Path.GetTempPath();
      string fileName = "ToolsHub-launch-" + Guid.NewGuid().ToString("N") + ".exe";
      string target = Path.Combine(folder, fileName);
      try
      {
        File.WriteAllText(target, string.Empty);
        var options = ShortcutLauncher.CreateStartInfo(new ShortcutItem { Target = @"%TEMP%\" + fileName });
        Assert.AreEqual(Path.GetFullPath(target), options.FileName);
        Assert.AreEqual(Path.GetDirectoryName(target), options.WorkingDirectory);
      }
      finally
      {
        File.Delete(target);
      }
    }

    [TestMethod]
    public void BlankTargetIsRejected()
    {
      Assert.ThrowsExactly<ArgumentException>(() => ShortcutLauncher.CreateStartInfo(new ShortcutItem { Target = " " }));
    }
  }
}

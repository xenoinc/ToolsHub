namespace Xeno.ToolsHub.Tests.SystemTests.Logging
{
  using System;
  using System.Collections.Generic;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.ToolsHub.Managers;
  using Xeno.ToolsHub.Services.Logging;

  [TestClass]
  [DoNotParallelize]
  public class LogTests
  {
    private ILogger _previousDevice;
    private Level _previousLevel;
    private bool _previousMute;
    private RecordingLogger _device;

    [TestInitialize]
    public void Initialize()
    {
      _previousDevice = Log.LogDevice;
      _previousLevel = Log.LogLevel;
      _previousMute = Log.MuteLogging;
      _device = new RecordingLogger();
      Log.LogDevice = _device;
      Log.LogLevel = Level.Debug;
      Log.MuteLogging = false;
    }

    [TestCleanup]
    public void Cleanup()
    {
      Log.LogDevice = _previousDevice;
      Log.LogLevel = _previousLevel;
      Log.MuteLogging = _previousMute;
    }

    [TestMethod]
    public void AllSeverityMethodsWriteToDevice()
    {
      Log.Debug("debug");
      Log.Info("info");
      Log.Warn("warning");
      Log.Error("error");
      Log.Fatal("fatal");

      CollectionAssert.AreEqual(
        new[] { Level.Debug, Level.Info, Level.Warn, Level.Error, Level.Fatal },
        _device.Levels.ToArray());
      StringAssert.Contains(_device.Messages[4], "[Fatal]");
      StringAssert.Contains(_device.Messages[4], "[fatal]");
    }

    [TestMethod]
    public void SeverityThresholdIncludesBoundaryAndHigherLevels()
    {
      Log.LogLevel = Level.Warn;
      Log.Debug("hidden");
      Log.Info("hidden");
      Log.Warn("visible");
      Log.Error("visible");
      Log.Fatal("visible");

      CollectionAssert.AreEqual(new[] { Level.Warn, Level.Error, Level.Fatal }, _device.Levels.ToArray());
    }

    [TestMethod]
    public void MutingSuppressesDeviceOutputAndCanBeReversed()
    {
      Log.MuteLogging = true;
      Log.Fatal("muted");
      Assert.AreEqual(0, _device.Messages.Count);
      Log.MuteLogging = false;
      Log.Info("visible");
      Assert.AreEqual(1, _device.Messages.Count);
    }

    [TestMethod]
    public void DirectLogMessagePreservesArgumentsAndCaller()
    {
      object[] arguments = { 42, "test" };
      Log.LogMessage(Level.Info, "value {0} {1}", arguments);

      Assert.AreSame(arguments, _device.Arguments);
      StringAssert.Contains(_device.Messages[0], typeof(LogTests).FullName);
      StringAssert.Contains(_device.Messages[0], "[value {0} {1}]");
    }

    [TestMethod]
    public void ApplicationAssemblyCallerIsReported()
    {
      var settings = new SettingsManager();
      settings.SetValue("logging-test", "invalid", "!invalid-base64!");

      Assert.ThrowsExactly<FormatException>(() => settings.GetObject<object>("logging-test", "invalid"));

      Assert.AreEqual(1, _device.Messages.Count);
      StringAssert.Contains(_device.Messages[0], typeof(SettingsManager).FullName);
      Assert.AreEqual(Level.Error, _device.Levels[0]);
    }

    private sealed class RecordingLogger : ILogger
    {
      public List<Level> Levels { get; } = new List<Level>();

      public List<string> Messages { get; } = new List<string>();

      public object[] Arguments { get; private set; }

      public void Log(Level lvl, string msg, params object[] args)
      {
        Levels.Add(lvl);
        Messages.Add(msg);
        Arguments = args;
      }
    }
  }
}

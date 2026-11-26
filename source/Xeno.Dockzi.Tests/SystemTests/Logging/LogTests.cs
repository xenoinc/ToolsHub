namespace Xeno.Dockzi.Tests.SystemTests.Logging
{
  using System;
  using System.Collections.Generic;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using Xeno.Dockzi.Managers;
  using Xeno.Dockzi.Services.Logging;

  [TestClass]
  [DoNotParallelize]
  public class LogTests
  {
    private RecordingLogger _device;
    private ILogger _previousDevice;
    private Level _previousLevel;
    private bool _previousMute;

    [TestMethod]
    public void AllSeverityMethodsWriteToDevice()
    {
      Log.Debug("debug");
      Log.Info("info");
      Log.Warn("warning");
      Log.Error("error");
      Log.Fatal("fatal");

      Assert.AreSequenceEqual(
        [Level.Debug, Level.Info, Level.Warn, Level.Error, Level.Fatal],
        [.. _device.Levels]); // Equivilant to: _device.Levels.ToArray()

      Assert.Contains("[Fatal]", _device.Messages[4]);
      Assert.Contains("[fatal]", _device.Messages[4]);
    }

    [TestMethod]
    public void ApplicationAssemblyCallerIsReported()
    {
      var settings = new SettingsManager();
      settings.SetValue("logging-test", "invalid", "!invalid-base64!");

      Assert.ThrowsExactly<FormatException>(() => settings.GetObject<object>("logging-test", "invalid"));
      Assert.HasCount(1, _device.Messages);
      Assert.Contains(typeof(SettingsManager).FullName, _device.Messages[0]);
      Assert.AreEqual(Level.Error, _device.Levels[0]);
    }

    [TestCleanup]
    public void Cleanup()
    {
      Log.LogDevice = _previousDevice;
      Log.LogLevel = _previousLevel;
      Log.MuteLogging = _previousMute;
    }

    [TestMethod]
    public void DirectLogMessagePreservesArgumentsAndCaller()
    {
      object[] arguments = { 42, "test" };
      Log.LogMessage(Level.Info, "value {0} {1}", arguments);

      Assert.AreSame(arguments, _device.Arguments);
      Assert.Contains(typeof(LogTests).FullName, _device.Messages[0]);
      Assert.Contains("[value {0} {1}]", _device.Messages[0]);
    }

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

    [TestMethod]
    public void MutingSuppressesDeviceOutputAndCanBeReversed()
    {
      Log.MuteLogging = true;
      Log.Fatal("muted");
      Assert.IsEmpty(_device.Messages);

      Log.MuteLogging = false;
      Log.Info("visible");
      Assert.HasCount(1, _device.Messages);
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

      Assert.AreSequenceEqual(
        [Level.Warn, Level.Error, Level.Fatal],
        [.. _device.Levels]);
    }

    private sealed class RecordingLogger : ILogger
    {
      public object[] Arguments { get; private set; }

      public List<Level> Levels { get; } = new List<Level>();

      public List<string> Messages { get; } = new List<string>();

      public void Log(Level lvl, string msg, params object[] args)
      {
        Levels.Add(lvl);
        Messages.Add(msg);
        Arguments = args;
      }
    }
  }
}

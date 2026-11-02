namespace Xeno.ToolsHub.Services
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Microsoft.Win32;

  /// <summary>Manages Windows sign-in startup for the current user.</summary>
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public sealed class AutoStartService : IAutoStartService
  {
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private readonly string _registryPath;
    private readonly string _valueName;
    private readonly string _command;

    public AutoStartService()
      : this(Application.ExecutablePath, RunKeyPath, "ToolsHub")
    {
    }

    /// <summary>Allows an isolated registry location for testing.</summary>
    public AutoStartService(string executablePath, string registryPath, string valueName)
    {
      ArgumentException.ThrowIfNullOrWhiteSpace(executablePath);
      ArgumentException.ThrowIfNullOrWhiteSpace(registryPath);
      ArgumentException.ThrowIfNullOrWhiteSpace(valueName);
      _command = $"\"{Path.GetFullPath(executablePath)}\" --auto-start";
      _registryPath = registryPath;
      _valueName = valueName;
    }

    public bool IsEnabled
    {
      get
      {
        using var key = Registry.CurrentUser.OpenSubKey(_registryPath);
        return key?.GetValue(_valueName) is string command && !string.IsNullOrWhiteSpace(command);
      }
    }

    public void SetEnabled(bool enabled)
    {
      if (enabled)
      {
        using var key = Registry.CurrentUser.CreateSubKey(_registryPath, true);
        key.SetValue(_valueName, _command, RegistryValueKind.String);
      }
      else
      {
        using var key = Registry.CurrentUser.OpenSubKey(_registryPath, true);
        key?.DeleteValue(_valueName, false);
      }
    }
  }
}

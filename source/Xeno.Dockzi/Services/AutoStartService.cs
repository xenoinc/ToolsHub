namespace Xeno.Dockzi.Services
{
  using System;
  using System.IO;
  using System.Windows.Forms;
  using Microsoft.Win32;
  using Xeno.Dockzi.Services.Logging;

  /// <summary>Manages Windows sign-in startup for the current user.</summary>
  [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
  public sealed class AutoStartService : IAutoStartService
  {
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private readonly string _registryPath;
    private readonly string _valueName;
    private readonly string _command;
    private readonly string _executablePath;

    public AutoStartService()
      : this(ApplicationPaths.Current.LauncherPath, RunKeyPath, "Dockzi", "ToolsHub")
    {
    }

    /// <summary>Allows an isolated registry location for testing.</summary>
    public AutoStartService(string executablePath, string registryPath, string valueName)
      : this(executablePath, registryPath, valueName, null)
    {
    }

    public AutoStartService(string executablePath, string registryPath, string valueName, string legacyValueName)
    {
      ArgumentException.ThrowIfNullOrWhiteSpace(executablePath);
      ArgumentException.ThrowIfNullOrWhiteSpace(registryPath);
      ArgumentException.ThrowIfNullOrWhiteSpace(valueName);

      _executablePath = Path.GetFullPath(executablePath);
      _command = $"\"{_executablePath}\" --auto-start";
      _registryPath = registryPath;
      _valueName = valueName;

      if (!string.IsNullOrEmpty(legacyValueName))
        MigrateLegacyEntry(executablePath, legacyValueName);
    }

    public bool IsEnabled
    {
      get
      {
        using var key = Registry.CurrentUser.OpenSubKey(_registryPath);
        return key?.GetValue(_valueName) is string command && !string.IsNullOrWhiteSpace(command);
      }
    }

    /// <summary>Repairs an enabled entry left behind by a moved or removed installation.</summary>
    public void RepairRegistration()
    {
      if (!File.Exists(_executablePath))
        return;

      string command;
      using (var key = Registry.CurrentUser.OpenSubKey(_registryPath))
        command = key?.GetValue(_valueName) as string;

      if (string.IsNullOrWhiteSpace(command) ||
          string.Equals(command, _command, StringComparison.OrdinalIgnoreCase) ||
          !command.StartsWith('"'))
        return;

      int quote = command.IndexOf('"', 1);
      if (quote <= 1 ||
          !string.Equals(command[(quote + 1)..].Trim(), "--auto-start", StringComparison.OrdinalIgnoreCase))
        return;

      string registeredPath = command[1..quote];
      if (!Path.IsPathFullyQualified(registeredPath) ||
          !string.Equals(Path.GetFileName(registeredPath), Path.GetFileName(_executablePath), StringComparison.OrdinalIgnoreCase) ||
          File.Exists(registeredPath))
        return;

      using var writableKey = Registry.CurrentUser.OpenSubKey(_registryPath, true);

      // Preserve changes made by the user or another instance after the initial read.
      if (!string.Equals(writableKey?.GetValue(_valueName) as string, command, StringComparison.Ordinal))
        return;

      writableKey.SetValue(_valueName, _command, RegistryValueKind.String);
      Log.Info("Repaired Dockzi startup registration: the previous executable no longer exists.");
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

    private void MigrateLegacyEntry(string executablePath, string legacyValueName)
    {
      try
      {
        using var key = Registry.CurrentUser.OpenSubKey(_registryPath, true);
        if (key == null || key.GetValue(_valueName) != null)
          return;
        string legacyExecutable = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(executablePath)), "ToolsHub.exe");
        string expected = $"\"{legacyExecutable}\" --auto-start";
        if (!string.Equals(key.GetValue(legacyValueName) as string, expected, StringComparison.OrdinalIgnoreCase))
          return;
        key.SetValue(_valueName, _command, RegistryValueKind.String);
        key.DeleteValue(legacyValueName, false);
      }
      catch (Exception ex) when (ex is UnauthorizedAccessException || ex is IOException || ex is System.Security.SecurityException)
      {
        Log.Warn($"Unable to migrate the legacy startup registration: {ex.Message}");
      }
    }
  }
}

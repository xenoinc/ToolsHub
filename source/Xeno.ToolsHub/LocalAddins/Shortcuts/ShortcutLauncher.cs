namespace Xeno.ToolsHub.LocalAddins.Shortcuts
{
  using System;
  using System.Diagnostics;
  using System.IO;

  /// <summary>Builds shell launch options for applications, files, folders and URLs.</summary>
  public static class ShortcutLauncher
  {
    public static ProcessStartInfo CreateStartInfo(ShortcutItem shortcut)
    {
      ArgumentNullException.ThrowIfNull(shortcut);
      ArgumentException.ThrowIfNullOrWhiteSpace(shortcut.Target);

      string target = Environment.ExpandEnvironmentVariables(shortcut.Target.Trim());
      var startInfo = new ProcessStartInfo
      {
        FileName = target,
        Arguments = shortcut.Params ?? string.Empty,
        UseShellExecute = true,
      };

      // Portable applications often load resources relative to their working directory.
      // Leave URLs and commands resolved through PATH to the Windows shell.
      if (File.Exists(target))
      {
        startInfo.FileName = Path.GetFullPath(target);
        startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
      }
      else if (Directory.Exists(target))
      {
        startInfo.FileName = Path.GetFullPath(target);
        startInfo.WorkingDirectory = startInfo.FileName;
      }

      if (shortcut.RunAsAdmin)
        startInfo.Verb = "runas";

      return startInfo;
    }
  }
}

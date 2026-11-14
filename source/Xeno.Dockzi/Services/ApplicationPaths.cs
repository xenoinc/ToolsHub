namespace Xeno.Dockzi.Services;

using System;
using System.IO;

/// <summary>Separates replaceable application files from persistent user data.</summary>
public sealed class ApplicationPaths
{
  public ApplicationPaths(string applicationDirectory = null, string localAppData = null, string portableDirectory = null)
  {
    ApplicationDirectory = Path.GetFullPath(applicationDirectory ?? AppContext.BaseDirectory);
    IsPackaged = File.Exists(Path.Combine(ApplicationDirectory, "Dockzi.install.json"));
    IsPortable = !IsPackaged || File.Exists(Path.Combine(ApplicationDirectory, "Dockzi.portable"));
    DataDirectory = IsPortable
      ? Path.GetFullPath(portableDirectory ?? Directory.GetCurrentDirectory())
      : Path.Combine(localAppData ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "XI", "Dockzi");
  }

  public static ApplicationPaths Current { get; } = new();

  public string ApplicationDirectory { get; }

  public bool IsPackaged { get; }

  public bool IsPortable { get; }

  public string DataDirectory { get; }

  public string AddinRegistryDirectory => Path.Combine(DataDirectory, "AddinRegistry");

  public string UserAddinsDirectory => Path.Combine(DataDirectory, "Addins");

  public string AddinDataDirectory => Path.Combine(DataDirectory, "AddinData");

  public string DownloadsDirectory => Path.Combine(DataDirectory, "Downloads");

  public string LogsDirectory => Path.Combine(DataDirectory, "Logs");

  public string LauncherPath
  {
    get
    {
      string stub = Path.Combine(Path.GetDirectoryName(ApplicationDirectory.TrimEnd(Path.DirectorySeparatorChar)) ?? ApplicationDirectory, "Dockzi.exe");
      return IsPackaged && string.Equals(new DirectoryInfo(ApplicationDirectory).Name, "current", StringComparison.OrdinalIgnoreCase) && File.Exists(stub)
        ? stub : Path.Combine(ApplicationDirectory, "Dockzi.exe");
    }
  }
}

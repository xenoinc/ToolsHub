namespace Xeno.ToolsHub.SidebarAddin.Services;

using System.Collections.Generic;

public interface IRunningApplications
{
  IReadOnlySet<string> GetOpenExecutables();

  string ResolveExecutable(string target);
}

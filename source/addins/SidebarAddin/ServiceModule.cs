namespace Xeno.ToolsHub.SidebarAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.ToolsHub.Services.Addins;
using Xeno.ToolsHub.SidebarAddin.Managers;
using Xeno.ToolsHub.SidebarAddin.Services;
using Xeno.ToolsHub.SidebarAddin.Views;

public sealed class ServiceModule : IAddinServiceModule
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.TryAddSingleton<IRunningApplications, WindowsRunningApplications>();
    services.TryAddSingleton<ILauncherStore, SettingsLauncherStore>();
    services.TryAddSingleton<ILauncherShell, WindowsLauncherShell>();
    services.TryAddSingleton<ISidebarWindowFactory, SidebarWindowFactory>();
    services.TryAddSingleton<SidebarManager>();
    services.TryAddTransient<PreferencesPage>();
  }
}

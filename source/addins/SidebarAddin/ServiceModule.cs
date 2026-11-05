namespace Xeno.Dockzi.SidebarAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.Dockzi.Services.Addins;
using Xeno.Dockzi.SidebarAddin.Managers;
using Xeno.Dockzi.SidebarAddin.Services;
using Xeno.Dockzi.SidebarAddin.Views;

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

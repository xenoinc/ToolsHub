namespace Xeno.Dockzi.PomodoroAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.Dockzi.PomodoroAddin.Domain;
using Xeno.Dockzi.PomodoroAddin.Managers;
using Xeno.Dockzi.PomodoroAddin.Views;
using Xeno.Dockzi.Services.Addins;

public sealed class ServiceModule : IAddinServiceModule
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.TryAddSingleton<IPomodoroTimer, PomodoroTimer>();
    services.TryAddSingleton<PomodoroManager>();
    services.TryAddTransient<PreferencePage>();
  }
}

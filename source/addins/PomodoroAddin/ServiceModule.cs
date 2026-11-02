namespace PomodoroAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PomodoroAddin.Domain;
using PomodoroAddin.Managers;
using PomodoroAddin.Views;
using Xeno.ToolsHub.Services.Addins;

public sealed class ServiceModule : IAddinServiceModule
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.TryAddSingleton<IPomodoroTimer, PomodoroTimer>();
    services.TryAddSingleton<PomodoroManager>();
    services.TryAddTransient<PreferencePage>();
  }
}

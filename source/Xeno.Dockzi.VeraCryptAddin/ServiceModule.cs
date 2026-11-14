namespace Xeno.Dockzi.VeraCryptAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.Dockzi.Services.Addins;
using Xeno.Dockzi.VeraCryptAddin.Services;
using Xeno.Dockzi.VeraCryptAddin.Views;

public sealed class ServiceModule : IAddinServiceModule
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.TryAddSingleton<IVeraCryptOperations, VeraCryptOperations>();
    services.TryAddSingleton<ISecretProtector, SecretProtector>();
    services.TryAddSingleton<VeraCryptManager>();
    services.TryAddTransient<PreferencesPage>();
  }
}

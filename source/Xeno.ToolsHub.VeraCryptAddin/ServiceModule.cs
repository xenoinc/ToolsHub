namespace Xeno.ToolsHub.VeraCryptAddin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.ToolsHub.Services.Addins;
using Xeno.ToolsHub.VeraCryptAddin.Services;
using Xeno.ToolsHub.VeraCryptAddin.Views;

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

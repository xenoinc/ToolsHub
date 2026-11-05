namespace Xeno.Dockzi.Services.Addins;

using Microsoft.Extensions.DependencyInjection;

/// <summary>Registers add-in dependencies before the application provider is built.</summary>
public interface IAddinServiceModule
{
  void ConfigureServices(IServiceCollection services);
}

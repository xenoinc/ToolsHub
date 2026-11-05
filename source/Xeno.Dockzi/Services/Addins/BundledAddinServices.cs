namespace Xeno.Dockzi.Services.Addins;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

public static class BundledAddinServices
{
  public static IServiceCollection AddBundledAddinServices(this IServiceCollection services, string directory = null)
  {
    // Register disabled bundles too, so enabling an installed add-in does not require rebuilding the provider.
    foreach (string file in new[] { "PomodoroAddin.dll", "SidebarAddin.dll", "VeraCryptAddin.dll" })
    {
      string path = Path.Combine(directory ?? AppContext.BaseDirectory, file);
      if (!File.Exists(path))
        continue;
      var assembly = Assembly.LoadFrom(Path.GetFullPath(path));
      foreach (var type in assembly.GetTypes().Where(type => !type.IsAbstract && typeof(IAddinServiceModule).IsAssignableFrom(type)))
        ((IAddinServiceModule)Activator.CreateInstance(type)).ConfigureServices(services);
    }

    return services;
  }
}

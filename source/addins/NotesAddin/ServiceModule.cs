namespace Xeno.Dockzi.NotesAddin;

using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services.Addins;

public sealed class ServiceModule : IAddinServiceModule
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.TryAddSingleton(provider => new NoteStore(Path.Combine(
      Path.GetDirectoryName(provider.GetRequiredService<SettingsManager>().SettingsFilePath), "Notes")));

    services.TryAddSingleton<NotesManager>();
    services.TryAddTransient<PreferencesPage>();
  }
}

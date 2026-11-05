namespace Xeno.Dockzi.Services;

using Lite.EventIpc;
using Microsoft.Extensions.DependencyInjection;
using Xeno.Dockzi.LocalAddins.Shortcuts;
using Xeno.Dockzi.Managers;
using Xeno.Dockzi.Services.Addins;
using Xeno.Dockzi.Views;
using Xeno.Dockzi.Views.Preferences;

/// <summary>Composition root for the application. Resolve UI services on the main STA thread.</summary>
public static class ApplicationServices
{
  public static IServiceCollection AddDockzi(this IServiceCollection services)
  {
    services.AddSingleton<SettingsManager>(_ => SettingsLoader.Load());
    services.AddSingleton<IEventAggregator, EventAggregator>();
    services.AddSingleton<ISettingsService, ApplicationSettings>();
    services.AddSingleton<IAutoStartService, AutoStartService>();
    services.AddTransient<ShortcutsManager>(provider => new ShortcutsManager(
      provider.GetRequiredService<SettingsManager>(), provider.GetRequiredService<IEventAggregator>()));
    services.AddTransient<Xeno.Dockzi.LocalAddins.Shortcuts.Prefs.ShortcutsPreferences>();
    services.AddTransient<GeneralPreferencesPage>();
    services.AddTransient<AddinFactory>();
    services.AddSingleton<AddinsManager>(provider => new AddinsManager(
      provider.GetRequiredService<AddinFactory>().Create));
    services.AddTransient<PreferencesForm>(provider => new PreferencesForm(
      provider.GetRequiredService<AddinsManager>(),
      provider.GetRequiredService<SettingsManager>(),
      provider.GetRequiredService<GeneralPreferencesPage>(),
      provider.GetRequiredService<AddinFactory>()));
    services.AddSingleton<IPreferencesDialogService, PreferencesDialogService>();
    services.AddSingleton<SystemTrayManager>();
    services.AddSingleton(_ => new WndProcManager("Dockzi"));
    services.AddSingleton<MainHandler>();
    return services;
  }
}

namespace Xeno.ToolsHub.Services;

using Lite.EventIpc;
using Microsoft.Extensions.DependencyInjection;
using Xeno.ToolsHub.LocalAddins.Shortcuts;
using Xeno.ToolsHub.Managers;
using Xeno.ToolsHub.Services.Addins;
using Xeno.ToolsHub.Views;
using Xeno.ToolsHub.Views.Preferences;

/// <summary>Composition root for the application. Resolve UI services on the main STA thread.</summary>
public static class ApplicationServices
{
  public static IServiceCollection AddToolsHub(this IServiceCollection services)
  {
    services.AddSingleton<SettingsManager>(_ => SettingsLoader.Load());
    services.AddSingleton<IEventAggregator, EventAggregator>();
    services.AddSingleton<ISettingsService, ApplicationSettings>();
    services.AddSingleton<IAutoStartService, AutoStartService>();
    services.AddTransient<ShortcutsManager>(provider => new ShortcutsManager(
      provider.GetRequiredService<SettingsManager>(), provider.GetRequiredService<IEventAggregator>()));
    services.AddTransient<Xeno.ToolsHub.LocalAddins.Shortcuts.Prefs.ShortcutsPreferences>();
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
    services.AddSingleton(_ => new WndProcManager("ToolsHub"));
    services.AddSingleton<MainHandler>();
    return services;
  }
}

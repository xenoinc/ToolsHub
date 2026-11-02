namespace Xeno.ToolsHub.Services;

using Microsoft.Extensions.DependencyInjection;
using Xeno.ToolsHub.Views;

/// <summary>Creates a fresh service scope and page instances for each settings dialog.</summary>
public sealed class PreferencesDialogService : IPreferencesDialogService
{
  private readonly IServiceScopeFactory _scopes;

  public PreferencesDialogService(IServiceScopeFactory scopes)
  {
    _scopes = scopes;
  }

  public void Show()
  {
    using var scope = _scopes.CreateScope();
    scope.ServiceProvider.GetRequiredService<PreferencesForm>().ShowDialog();
  }
}

namespace Xeno.Dockzi.Services;

using Microsoft.Extensions.DependencyInjection;
using Xeno.Dockzi.Views;

/// <summary>Creates a fresh service scope and page instances for each settings dialog.</summary>
public sealed class PreferencesDialogService : IPreferencesDialogService
{
  private readonly IServiceScopeFactory _scopes;
  private PreferencesForm _activeDialog;

  public PreferencesDialogService(IServiceScopeFactory scopes)
  {
    _scopes = scopes;
  }

  public void Show() => Show(null);

  public void Show(string pageTitle)
  {
    if (_activeDialog != null && !_activeDialog.IsDisposed)
    {
      _activeDialog.SelectPreferencePage(pageTitle);
      _activeDialog.Activate();
      return;
    }

    using var scope = _scopes.CreateScope();
    var dialog = scope.ServiceProvider.GetRequiredService<PreferencesForm>();
    _activeDialog = dialog;
    try
    {
      dialog.SelectPreferencePage(pageTitle);
      dialog.ShowDialog();
    }
    finally
    {
      _activeDialog = null;
    }
  }
}

namespace Xeno.Dockzi.Services;

using System;
using Xeno.Dockzi.Managers;

/// <summary>User update preferences persisted in Dockzi.config.</summary>
public sealed class UpdatePreferences
{
  private const string Section = "General";
  private const string PreviewKey = "ReceivePreviewUpdates";
  private readonly SettingsManager _settings;

  public UpdatePreferences(SettingsManager settings) => _settings = settings ?? throw new ArgumentNullException(nameof(settings));

  public bool ReceivePreviewUpdates => bool.TryParse(_settings.GetValue(Section, PreviewKey), out bool enabled) && enabled;

  public void SaveReceivePreviewUpdates(bool enabled)
  {
    bool previous = ReceivePreviewUpdates;
    _settings.SetValue(Section, PreviewKey, enabled.ToString());
    try
    {
      _settings.SaveFileOrThrow();
    }
    catch
    {
      _settings.SetValue(Section, PreviewKey, previous.ToString());
      throw;
    }
  }
}

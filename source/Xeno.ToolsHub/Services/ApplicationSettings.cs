namespace Xeno.ToolsHub.Services;

using Xeno.ToolsHub.Managers;

public sealed class ApplicationSettings : ISettingsService
{
  private readonly SettingsManager _settings;

  public ApplicationSettings(SettingsManager settings) => _settings = settings;

  public bool GetBool(string propertyId, string key, bool defaultValue = false) =>
    bool.TryParse(GetString(propertyId, key), out bool value) ? value : defaultValue;

  public int GetInt(string propertyId, string key, int defaultValue = 0) =>
    int.TryParse(GetString(propertyId, key), out int value) ? value : defaultValue;

  public double GetDouble(string propertyId, string key, double defaultValue = 0) =>
    double.TryParse(GetString(propertyId, key), out double value) ? value : defaultValue;

  public string GetString(string propertyId, string key, string defaultValue = "") => _settings.GetValue(propertyId, key, defaultValue);

  public T GetObject<T>(string propertyId, string key) => _settings.GetObject<T>(propertyId, key);

  public void SetValue(string propertyId, string key, string value) => _settings.SetValue(propertyId, key, value);

  public void SetValue(string propertyId, string key, bool value) => SetValue(propertyId, key, value.ToString());

  public void SetValue(string propertyId, string key, int value) => SetValue(propertyId, key, value.ToString());
}

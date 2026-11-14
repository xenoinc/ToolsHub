namespace Xeno.Dockzi.Services;

public interface ISettingsService
{
  bool GetBool(string propertyId, string key, bool defaultValue = false);

  int GetInt(string propertyId, string key, int defaultValue = 0);

  double GetDouble(string propertyId, string key, double defaultValue = 0);

  string GetString(string propertyId, string key, string defaultValue = "");

  T GetObject<T>(string propertyId, string key);

  void SetValue(string propertyId, string key, string value);

  void SetValue(string propertyId, string key, bool value);

  void SetValue(string propertyId, string key, int value);
}

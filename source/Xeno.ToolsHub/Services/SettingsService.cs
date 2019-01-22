/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    SettingsService.cs
 * Description:
 *  Wrapper for Program.Settings so external add-ins access it
 *  Temporary workaround until we implement IoC or MessageSender
 */

namespace Xeno.ToolsHub.Services
{
  public static class SettingsService
  {
    public static string GetValue(string propertyId, string key, string defValue = "")
    {
      return Program.Settings.GetValue(propertyId, key, defValue);
    }

    public static int GetInt(string propertyId, string key, int defValue = 0)
    {
      if (int.TryParse(GetValue(propertyId, key, defValue.ToString()), out int value))
        return value;
      else
        return defValue;
    }

    public static double GetDouble(string propertyId, string key, double defValue = 0)
    {
      if (double.TryParse(GetValue(propertyId, key, defValue.ToString()), out double value))
        return value;
      else
        return defValue;
    }

    public static void SetValue(string propertyId, string key, string value)
    {
      Program.Settings.SetValue(propertyId, key, value);
    }

    public static void SetValue(string propertyId, string key, int value)
    {
      SetValue(propertyId, key, value.ToString());
    }

    public static void SetValue(string propertyId, string key, double value)
    {
      SetValue(propertyId, key, value.ToString());
    }
  }
}

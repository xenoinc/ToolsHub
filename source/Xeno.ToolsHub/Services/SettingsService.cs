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
    public static void SetValue(string propertyId, string key, string value)
    {
      Program.Settings.SetValue(propertyId, key, value);
    }

    public static string GetValue(string propertyId, string key, string defValue = "")
    {
      return Program.Settings.GetValue(propertyId, key, defValue);
    }
  }
}

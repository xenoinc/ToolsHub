/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-5
 * Author:  <Unknown>
 * File:    AppSettings.cs
 * Description:
 *  Application settings
 */

namespace Xeno.ToolsHub.Helpers
{
  public class AppSettings
  {
    public void InitializeDefaults()
    {
    }

    public AppSettings Load()
    {
      return new AppSettings();
    }

    /// <summary>Save settings to local file</summary>
    /// <returns>Returns false if issue saving</returns>
    public bool Save(bool useLocalDir = true)
    {
      return false;
    }
  }
}

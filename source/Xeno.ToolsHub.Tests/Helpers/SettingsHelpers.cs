/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-9
 * Author:  Damian Suess
 * File:    SettingsHelpers.cs
 * Description:
 *
 */

using Xeno.ToolsHub.LocalAddins.Shortcuts;

namespace Xeno.ToolsHub.Tests.Helpers
{
  public static class SettingsHelpers
  {
    public static ShortcutItems GenerateShortcutsMixed()
    {
      var items = new ShortcutItems
      {
         new ShortcutItem { Title = "FolderA", Target = @"C:\Test1" },
         new ShortcutItem { Title = "FolderB", Target = @"C:\Test2\Sub1" },
         new ShortcutItem { Title = "FolderC", Target = @"C:\Test3\Sub1\Sub2" },
         new ShortcutItem { Title = "Web1", Target = @"https://www.xenoinc.com/" },
         new ShortcutItem { Title = "-", Target = string.Empty },
         new ShortcutItem { Title = "Manage Shortcuts", Target = "SomePath" },
       };

      return items;
    }
  }
}

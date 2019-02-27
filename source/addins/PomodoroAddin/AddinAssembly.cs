/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our add-in extension
 */

// Define our add-in
[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "PomodoroAddin",
  Category = "Tools",
  CompatVersion = "0.3",
  EnabledByDefault = true,
  Version = "0.3",
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("XenoInnovations.ToolsHub", "0.3")]

namespace PomodoroAddin
{
  public static class Constants
  {
    public static string AddinId = "PomodoroTimer";

    public static string KeyDuration = "Duration";
    public static string KeyShortBreak = "BreakShort";
    public static string KeyLongBreak = "BreakLong";

    public static string KeyTrayShowDuration = "SysTrayShowDuration";
    public static string KeyAlertSound = "AlertPlaySound";
    public static string KeyAlertFlash = "AlertFlashScreen";
    public static string KeyAlertSysTrayBubble = "AlertSysTrayBubble";

    // public static string SysTrayDurations = "SysTrayDurations";
    // public static string FlashScreenEvents = "FlashEvents";
    // public static string SysTrayBubbles = "SysTrayBubbles";
  }
}
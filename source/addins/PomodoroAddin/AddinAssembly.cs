/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our add-in extension
 */
using Mono.Addins;

// Define our add-in
[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "PomodoroAddin",
  Version = "0.3",
  Category = "Tools",
  CompatVersion = "0.3",
  EnabledByDefault = true,
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

// Extended properties
[assembly: Mono.Addins.AddinName("Pomodoro Timer")]
[assembly: Mono.Addins.AddinDescription("Productivity time-boxing assistant.")]
[assembly: Mono.Addins.AddinAuthor("Damian Suess")]

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("Xeno.Dockzi", "0.3")]

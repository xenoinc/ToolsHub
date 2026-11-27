/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-6
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Definition of our Sidebar add-in extension
 */

// Define our add-in
using Mono.Addins;

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("Xeno.Dockzi", "0.3")]

[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "SidebarAddin",
  Version = "0.3",
  Category = "Tools",
  CompatVersion = "0.3",
  EnabledByDefault = true,
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

//// [assembly:AddinName("Sidebar Add-in")]
[assembly: AddinProperty("Name", "Sidebar Add-in")]
[assembly: AddinDescription("Desktop dockable sidebar")]
[assembly: Mono.Addins.AddinAuthor("Damian Suess")]

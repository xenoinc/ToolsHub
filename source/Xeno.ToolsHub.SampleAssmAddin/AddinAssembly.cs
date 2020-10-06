/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Add-in assembly definition
 */

// Define our add-in
using Mono.Addins;

[assembly: Addin(
  Namespace = "",
  Id = "ExternalSampleAssmblyAddin",
  Version = "0.3",
  Category = "Sample",
  CompatVersion = "0.3",
  EnabledByDefault = false,
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

// Manually set properties:
//// [assembly: AddinProperty("Name", "Sidebar Add-in")]
[assembly: AddinName("Sample Assembly Add-in")]
[assembly: AddinDescription("Sample add-in defined using assembly.")]
[assembly: AddinAuthor("Damian Suess")]

// Sample Properties
[assembly: AddinProperty("CustomProperty1", "Val1")]
[assembly: AddinProperty("CustomProperty2", "en-US", "Val2Cat")]
[assembly: AddinProperty("CustomProperty2", "ca-ES", "Val2Cat")]

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("XenoInnovations.ToolsHub", "0.3")]
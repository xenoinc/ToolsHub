/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinAssembly.cs
 * Description:
 *  Add-in assembly definition
 */

// Define our add-in
[assembly: Mono.Addins.Addin(
  Namespace = "",
  Id = "ExternalSampleAssmblyAddin",
  Category = "Sample",
  CompatVersion = "0.3",
  EnabledByDefault = true,
  Version = "0.3",
  Flags = Mono.Addins.Description.AddinFlags.None,
  Url = "https://github.com/xenoinc/ToolsHub")]

// Attach to add-in host
[assembly: Mono.Addins.AddinDependency("XenoInnovations.ToolsHub", "0.3")]
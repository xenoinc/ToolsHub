/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-5
 * Author:  Damian Suess
 * File:    AddinOnStartHandler.cs
 * Description:
 *  SampleAssembly OnStart extension
 */

using Mono.Addins;
using Xeno.ToolsHub.Config;
using Xeno.ToolsHub.ExtensionModel;

//[assembly: Addin(
//  Id = "",
//  Namespace = "",
//  Category = "Sample",
//  CompatVersion = "0.3",
//  EnabledByDefault = true,
//  Version = "0.3", Flags = Mono.Addins.Description.AddinFlags.None,
//  Url = "https://github.com/xenoinc/ToolsHub")]

[assembly: Addin]
[assembly: AddinDependency("XenoInnovations.ToolsHub", "0.3")]

namespace Xeno.ToolsHub.SampleAssmAddin
{
  [Extension(NodeName = ExtensionName.OnStartupAddin, Path = ExtensionPath.OnStartup)]
  public class AddinOnStartHandler : ExtensionModel.IOnStartupExtension
  {
    public AddinOnStartHandler()
    {
      this.Title = "SampleAssm external OnStartup add-in";
      Log.Debug("SampleAssm external OnStartup, initialized");

      System.Windows.Forms.MessageBox.Show("SampleAssm external OnStartup, initialized!");
    }

    public string Title { get; }

    public void Execute()
    {
      Log.Debug("SampleAssm external OnStartup, executed");

      System.Windows.Forms.MessageBox.Show("SampleAssm external OnStartup, executed!");
    }
  }
}

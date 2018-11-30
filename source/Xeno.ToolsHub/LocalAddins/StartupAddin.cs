/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-11-29
 * Author:  Damian Suess
 * File:    StartupAddin.cs
 * Description:
 *
 */

using System;

namespace Xeno.ToolsHub.LocalAddins
{
  public class StartupAddin : ExtensionModel.IStartupExtension
  {
    public StartupAddin()
    {
      this.Title = "Sample Startup Add-In";
    }

    public string Title { get; }

    public void Exexcute()
    {
      Console.WriteLine("Sample Startup :: Run");
      System.Windows.Forms.MessageBox.Show("Sample Startup Add-in!");
    }
  }
}

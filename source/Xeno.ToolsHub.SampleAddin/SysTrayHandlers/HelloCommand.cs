/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-10-24
 * Author:  Damian Suess
 * File:    HelloCommand.cs
 * Description:
 *
 */

using System.Windows.Forms;

namespace Xeno.ToolsHub.SampleAddin.SysTrayHandlers
{
  // [Command("Hello", Id = "SampleHelloCommand")]
  public class HelloCommand //: ICommand
  {
    public void Exec()
    {
      Xeno.ToolsHub.Helpers.Log.Debug("Hello command, clicked");
      MessageBox.Show("Hello World!", "BooOOOoring!");
    }
  }
}

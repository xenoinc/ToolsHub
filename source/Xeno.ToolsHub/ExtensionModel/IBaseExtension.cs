/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    IBaseExtension.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public interface IBaseExtension
  {
    string Title { get; }

    void Run();
  }
}

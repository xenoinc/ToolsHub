/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    IBaseExtension.cs
 * Description:
 *  Add-in Base extension
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  /// <summary>Add-in base extension.</summary>
  public interface IBaseExtension
  {
    string Title { get; }

    void Execute();
  }
}

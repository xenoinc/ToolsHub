﻿/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-17
 * Author:  Damian Suess
 * File:    ISetartupExtension.cs
 * Description:
 *  Unused, therefore obsolete for now
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  // [Mono.Addins.TypeExtensionPoint(Path = "/ToolsHub/OnStartup", NodeName = "OnStartup")]
  public interface IStartupExtension
  {
    string Title { get; }

    void Exexcute();
  }
}

/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    ILogger.cs
 * Description:
 *  Logger interface
 */

namespace Xeno.ToolsHub.Services.Logging
{
  using Xeno.ToolsHub.Config;

  public interface ILogger
  {
    void Log(Level lvl, string msg, params object[] args);
  }
}

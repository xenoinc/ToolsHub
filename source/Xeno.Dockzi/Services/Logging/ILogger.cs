/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-31
 * Author:  Damian Suess
 * File:    ILogger.cs
 * Description:
 *  Logger interface
 */

namespace Xeno.Dockzi.Services.Logging
{
  using Xeno.Dockzi.Config;

  public interface ILogger
  {
    void Log(Level lvl, string msg, params object[] args);
  }
}

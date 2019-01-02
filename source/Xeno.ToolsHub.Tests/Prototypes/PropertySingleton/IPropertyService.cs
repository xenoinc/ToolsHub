/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    IPropertyService.cs
 * Description:
 *
 */

namespace Xeno.ToolsHub.Tests.SystemTests.PropertySingleton
{
  public interface IPropertyService
  {
    void Set<T>(string key, T value);
  }
}

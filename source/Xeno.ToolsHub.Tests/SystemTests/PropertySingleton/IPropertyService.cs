/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    IPropertyService.cs
 * Description:
 *  
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xeno.ToolsHub.Tests.SystemTests.PropertySingleton
{
  public interface IPropertyService
  {
    void Set<T>(string key, T value);

  }
}

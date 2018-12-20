/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-12-19
 * Author:  Damian Suess
 * File:    PropertyService.cs
 * Description:
 *  Singleton property service (temporary)
 */

using System.Collections.Generic;

namespace Xeno.ToolsHub.Services.PropertyService
{
  public class PropertyBag
  {
    public string AddinGuid { get; set; }

    public Dictionary<string, string> Properties = new Dictionary<string, string>();

    // public Dictionary<string, object> Properties = new Dictionary<string, object>();
  }
}

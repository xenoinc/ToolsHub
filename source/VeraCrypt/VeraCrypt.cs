/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    VeraCrypt.cs
 * Description:
 *
 */

using System;
using Xeno.ToolsHub.ExtensionModel;

namespace Xeno.ToolsHub.VeraCrypt
{
  public class VeraCrypt : ApplicationAddin
  {
    private bool _initialized = false;

    public override bool Initialized => _initialized;

    public override void Initialize()
    {
      throw new NotImplementedException();
    }

    public override void Shutdown()
    {
      throw new NotImplementedException();
    }
  }
}

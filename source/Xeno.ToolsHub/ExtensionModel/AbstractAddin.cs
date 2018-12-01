/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    AbstractAddin.cs
 * Description:
 *  Base addin class
 */

using System;

namespace Xeno.ToolsHub.ExtensionModel
{
  public abstract class AbstractAddin : IDisposable
  {
    private bool _disposing = false;

    ~AbstractAddin()
    {
      Dispose(false);
    }

    public bool IsDisposing
    {
      get
      {
        return _disposing;
      }
    }

    public void Dispose()
    {
      _disposing = true;
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
  }
}

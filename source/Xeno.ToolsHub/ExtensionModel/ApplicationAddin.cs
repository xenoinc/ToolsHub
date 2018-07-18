/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    ApplicationAddin.cs
 * Description:
 *  Base application addin class
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  public abstract class ApplicationAddin : AbstractAddin
  {
    public abstract bool Initialized { get; }

    public abstract void Initialize();

    public abstract void Shutdown();
  }
}

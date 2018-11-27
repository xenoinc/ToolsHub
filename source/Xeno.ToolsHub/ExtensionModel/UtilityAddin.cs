/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    ApplicationAddin.cs
 * Description:
 *  Base application addin class
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  /// <summary>
  ///   Add notes as to what this extension does for your add-in
  /// </summary>
  public abstract class UtilityAddin : AbstractAddin
  {
    public abstract bool Initialized { get; }

    public abstract void Initialize();

    public abstract void Shutdown();
  }
}

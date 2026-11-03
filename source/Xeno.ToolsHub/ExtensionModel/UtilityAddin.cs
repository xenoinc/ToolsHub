/* Copyright Xeno Innovations, Inc. 2018
 * Date:    2018-7-18
 * Author:  Damian Suess
 * File:    UtilityAddin.cs
 * Description:
 *  Base application add-in class which is disposable
 */

namespace Xeno.ToolsHub.ExtensionModel
{
  /// <summary>
  ///   Add notes as to what this extension does for your add-in
  /// </summary>
  public abstract class UtilityAddin : AbstractAddin
  {
    public abstract bool IsInitialized { get; }

    public abstract void Execute();

    /// <summary>Called when the user enables this utility while the host is running.</summary>
    public virtual void OnEnabled() => Execute();

    public abstract void Shutdown();
  }
}

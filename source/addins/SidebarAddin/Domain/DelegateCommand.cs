/* Copyright Xeno Innovations, Inc. 2019
 * Date:    2019-4-21
 * Author:  Damian Suess
 * File:    DelegateCommand.cs
 * Description:
 *  DelegateCommand - MVVM helper
 *
 * Reference:
 *  - https://www.codeproject.com/Articles/275238/Skinnable-and-Animated-popup-window-in-WPF
 */

using System;
using System.Windows.Input;

namespace Xeno.ToolsHub.SidebarAddin.Domain
{
  /// <summary>A command that executes delegates to determine whether the command can execute, and to execute the command.</summary>
	/// <remarks>
	///   <para>
	///     This command implementation is useful when the command simply needs to execute a method on a view model. The delegate for
	///     determining whether the command can execute is optional. If it is not provided, the command is considered always eligible
	///     to execute.
	///   </para>
	/// </remarks>
	public class DelegateCommand : ICommand
  {
    private readonly Predicate<object> _canExecute;
    private readonly Action<object> _execute;

    /// <summary>Constructs an instance of <c>DelegateCommand</c>.</summary>
    /// <remarks>
    ///   This constructor creates the command without a delegate for determining
    ///   whether the command can execute. Therefore, the command will always be
    ///   eligible for execution.
    /// </remarks>
    /// <param name="execute">The delegate to invoke when the command is executed.</param>
    public DelegateCommand(Action<object> execute)
      : this(execute, null)
    {
    }

    /// <summary>Constructs an instance of <c>DelegateCommand</c>.</summary>
    /// <param name="execute">The delegate to invoke when the command is executed.</param>
    /// <param name="canExecute">The delegate to invoke to determine whether the command can execute.</param>
    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
      _execute = execute;
      _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    /// <summary>Determines whether this command can execute.</summary>
    /// <remarks>
    ///   If there is no delegate to determine whether the command can execute, this method
    ///   will return <see langword="true"/>. If a delegate was provided, this
    ///   method will invoke that delegate.
    /// </remarks>
    /// <param name="parameter">The command parameter.</param>
    /// <returns><see langword="true"/> if the command can execute, otherwise <see langword="false"/>.</returns>
    public bool CanExecute(object parameter)
    {
      if (_canExecute == null)
      {
        return true;
      }

      return _canExecute(parameter);
    }

    /// <summary>Executes this command.</summary>
    /// <remarks>This method invokes the provided delegate to execute the command.</remarks>
    /// <param name="parameter">The command parameter.</param>
    public void Execute(object parameter)
    {
      _execute(parameter);
    }
  }
}

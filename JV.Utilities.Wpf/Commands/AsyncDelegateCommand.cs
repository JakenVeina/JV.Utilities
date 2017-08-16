using System;
using System.Threading.Tasks;
using System.Windows.Input;

using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Commands
{
    /// <summary>
    /// Reusable implementation of <see cref="IAsyncCommand"/> via given <see cref="Func{T}"/> (of type <see cref="Task"/>) and <see cref="Func{T}"/> (of type <see cref="bool"/>) delegates.
    /// using the Task-based Asynchronous programming pattern. Note that <see cref="ICommand.Execute(object)"/> does not execute synchronously. It simply does not return the <see cref="Task"/>
    /// returned by the delegate, for the sake of WPF compatibility, and instead throws it away without awaiting it.
    /// </summary>
    public class AsyncDelegateCommand : DelegateCommand, IAsyncCommand
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new ICommand from the given Execute() delegate.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/> or <see cref="IAsyncCommand.ExecuteAsync(object)"/> is invoked.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<Task> executeAsync)
            : this(executeAsync, null, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/> or <see cref="IAsyncCommand.ExecuteAsync(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="ICommand.CanExecute(object)"/> is invoked. Can be null, in which case <see cref="ICommand.CanExecute(object)"/> will always return true.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<Task> executeAsync, Func<bool> canExecute)
            : this(executeAsync, canExecute, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/> or <see cref="IAsyncCommand.ExecuteAsync(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="ICommand.CanExecute(object)"/> is invoked. Can be null, in which case <see cref="ICommand.CanExecute(object)"/> will always return true.</param>
        /// <param name="commandManager">The <see cref="ICommandManager"/> to which the new command will be attached. Ignored if null.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<Task> executeAsync, Func<bool> canExecute, ICommandManager commandManager)
            : base(() => executeAsync.Invoke(), canExecute, commandManager)
        {
            if (executeAsync == null)
                throw new ArgumentNullException(nameof(executeAsync));

            _executeAsync = executeAsync;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IAsyncCommand

        /// <summary>
        /// See <see cref="IAsyncCommand.ExecuteAsync(object)"/>.
        /// </summary>
        public Task ExecuteAsync(object parameter)
            => _executeAsync.Invoke();

        #endregion IAsyncCommand

        /**********************************************************************/
        #region Private Fields

        private readonly Func<Task> _executeAsync;

        #endregion Private Fields
    }
}

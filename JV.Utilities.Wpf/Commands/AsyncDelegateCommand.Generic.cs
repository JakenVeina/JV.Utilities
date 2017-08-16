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
    /// <typeparam name="T">The type of <see cref="Func{T, Tout}"/> and <see cref="Predicate{T}"/> delegates used for <see cref="ICommand.Execute(object)"/>/<see cref="IAsyncCommand.ExecuteAsync(object)"/> and <see cref="ICommand.CanExecute(object)"/>, respectively.</typeparam>
    public class AsyncDelegateCommand<T> : DelegateCommand<T>, IAsyncCommand
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new ICommand from the given Execute() delegate.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/>is invoked.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<T, Task> executeAsync)
            : this(executeAsync, null, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/>is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="ICommand.CanExecute(object)"/> is invoked. Can be null, in which case <see cref="ICommand.CanExecute(object)"/> will always return true.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<T, Task> executeAsync, Predicate<T> canExecute)
            : this(executeAsync, canExecute, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="executeAsync">Delegate to execute when <see cref="ICommand.Execute(object)"/>is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="ICommand.CanExecute(object)"/> is invoked. Can be null, in which case <see cref="ICommand.CanExecute(object)"/> will always return true.</param>
        /// <param name="commandManager">The <see cref="ICommandManager"/> to which the new command will be attached. Ignored if null.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="executeAsync"/> is null.</exception>
        public AsyncDelegateCommand(Func<T, Task> executeAsync, Predicate<T> canExecute, ICommandManager commandManager)
            : base(p => executeAsync.Invoke(p), canExecute, commandManager)
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
        /// <exception cref="InvalidCastException">Throws if <paramref name="parameter"/> cannot be cast to <typeparamref name="T"/>.</exception>
        public Task ExecuteAsync(object parameter)
        {
            if (!ValidateParameterType(parameter))
                throw new ArgumentException($"Cannot convert {parameter?.GetType().Name ?? "null"} to {typeof(T).Name}", nameof(parameter));

            return _executeAsync.Invoke((T)parameter);
        }

        #endregion IAsyncCommand

        /**********************************************************************/
        #region Private Fields

        private readonly Func<T, Task> _executeAsync;

        #endregion Private Fields
    }
}

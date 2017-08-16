using System;
using System.Windows.Input;

using JV.Utilities.Wpf.Commands.Interfaces;

namespace JV.Utilities.Wpf.Commands
{
    /// <summary>
    /// Reusable implementation of <see cref="ICommand"/> via given <see cref="Action{T}"/> and <see cref="Predicate{T}"/> delegates.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Action{T}"/> and <see cref="Predicate{T}"/> delegates used for <see cref="ICommand.Execute(object)"/> and <see cref="ICommand.CanExecute(object)"/>, respectively.</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new ICommand from the given Execute() delegate.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action<T> execute)
            : this(execute, null, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="CanExecute(object)"/> is invoked. Can be null, in which case <see cref="CanExecute(object)"/> will always return true for valid parameters.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
            : this(execute, canExecute, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="CanExecute(object)"/> is invoked. Can be null, in which case <see cref="CanExecute(object)"/> will always return true for valid parameters.</param>
        /// <param name="commandManager">The <see cref="ICommandManager"/> to which the new command will be attached. Ignored if null.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute, ICommandManager commandManager)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
            _commandManager = commandManager;

            // Cache type reflection for performance.
            var parameterType = typeof(T);
            _parameterTypeIsNullable = !parameterType.IsValueType || (Nullable.GetUnderlyingType(parameterType) != null);
        }

        #endregion

        /**********************************************************************/
        #region Public Methods

        /// <summary>
        /// Raises the <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
            => _canExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion Public Methods

        /**********************************************************************/
        #region ICommand

        /// <summary>
        /// See <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        /// <exception cref="ArgumentException">Throws if <paramref name="parameter"/> cannot be cast to <typeparamref name="T"/>.</exception>
        public void Execute(object parameter)
        {
            AssertParameterType(parameter);

           _execute.Invoke((T)parameter);
        }
        private readonly Action<T> _execute;

        /// <summary>
        /// See <see cref="ICommand.CanExecute(object)"/>.
        /// Returns false if <paramref name="parameter"/> cannot be cast to <typeparamref name="T"/>.
        /// Returns true <paramref name="parameter"/> is valid for <typeparamref name="T"/>, but no canExecute delegate was supplied during construction.
        /// </summary>
        public bool CanExecute(object parameter)
            => ValidateParameterType(parameter) && (_canExecute?.Invoke((T)parameter) ?? true);

        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// See <see cref="ICommand.CanExecuteChanged"/>.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if ((_canExecute != null) && (_commandManager != null))
                    _commandManager.RequerySuggested += value;
                _canExecuteChanged += value;
            }
            remove
            {
                if ((_canExecute != null) && (_commandManager != null))
                    _commandManager.RequerySuggested -= value;
                _canExecuteChanged -= value;
            }
        }
        private event EventHandler _canExecuteChanged;

        #endregion ICommand

        /**********************************************************************/
        #region Protected Methods

        /// <summary>
        /// Checks whether or not a given object can be cast to the current type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="parameter">The object to be checked for castability.</param>
        /// <returns>True if <paramref name="parameter"/> can be cast to <typeparamref name="T"/> without causing an exception; False otherwise</returns>
        internal protected bool ValidateParameterType(object parameter)
        {
            if (parameter == null)
            {
                if (!_parameterTypeIsNullable)
                    return false;
            }
            else if (!(parameter is T))
                return false;

            return true;
        }

        /// <summary>
        /// Invokes <see cref="ValidateParameterType(object)"/> with the given <paramref name="parameter"/> value,
        /// and throws a relevant exception if the return value is false.
        /// </summary>
        /// <param name="parameter">The parameter to be passed to <see cref="ValidateParameterType(object)"/>.</param>
        internal protected void AssertParameterType(object parameter)
        {
            if (!ValidateParameterType(parameter))
                throw new ArgumentException($"Cannot convert {parameter?.GetType().Name ?? "null"} to {typeof(T).Name}", nameof(parameter));
        }

        #endregion Protected Properties

        /**********************************************************************/
        #region Private Fields

        private readonly ICommandManager _commandManager;

        private readonly bool _parameterTypeIsNullable;

        #endregion
    }
}

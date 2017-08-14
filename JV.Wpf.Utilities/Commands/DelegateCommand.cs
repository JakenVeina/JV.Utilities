using System;
using System.Windows.Input;

namespace JV.Wpf.Utilities.Commands
{
    /// <summary>
    /// Reusable implementation of <see cref="ICommand"/> via given <see cref="Action"/> and <see cref="Func{T}"/> (of type <see cref="bool"/>) delegates.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new ICommand from the given Execute() delegate.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action execute)
            : this(execute, null, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="CanExecute(object)"/> is invoked. Can be null, in which case <see cref="CanExecute(object)"/> will always return true.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(execute, canExecute, null) { }

        /// <summary>
        /// Creates a new ICommand from the given Execute() and CanExecute() delegates.
        /// </summary>
        /// <param name="execute">Delegate to execute when <see cref="Execute(object)"/> is invoked.</param>
        /// <param name="canExecute">Delegate to execute when <see cref="CanExecute(object)"/> is invoked. Can be null, in which case <see cref="CanExecute(object)"/> will always return true.</param>
        /// <param name="commandManager">The <see cref="ICommandManager"/> to which the new command will be attached. Ignored if null.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="execute"/> is null.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute, ICommandManager commandManager)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute;
            _commandManager = commandManager;
        }

        #endregion

        /**********************************************************************/
        #region Public Methods

        /// <summary>
        /// Raises the <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            _canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion Public Methods

        /**********************************************************************/
        #region ICommand

        /// <summary>
        /// See <see cref="ICommand.Execute(object)"/>.
        /// </summary>
        public void Execute(object parameter)
            => _execute.Invoke();
        private readonly Action _execute;

        /// <summary>
        /// See <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        public bool CanExecute(object parameter)
            => _canExecute?.Invoke() ?? true;
        private readonly Func<bool> _canExecute;

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
        #region Private Fields

        private readonly ICommandManager _commandManager;

        #endregion

    }
}

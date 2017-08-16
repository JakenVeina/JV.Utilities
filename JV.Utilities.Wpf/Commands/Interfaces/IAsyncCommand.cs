using System.Threading.Tasks;
using System.Windows.Input;

namespace JV.Utilities.Wpf.Commands.Interfaces
{
    /// <summary>
    /// Extends <see cref="ICommand"/> to support the Task-based Asynchronous programming pattern.
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /**********************************************************************/
        #region Methods

        /// <summary>
        /// Equivalent to <see cref="ICommand.Execute(object)"/>, except an awaitable Task is returned to support asynchronous execution.
        /// </summary>
        Task ExecuteAsync(object parameter);

        #endregion Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Observation
{
    /// <summary>
    /// A proxy object for invoking an <see cref="Action"/>, via the <see cref="IDisposable"/> interface,
    /// allowing the use of the C# using keyword to control an object's state.
    /// </summary>
    public class DisposeInvoker : IDisposable
    {
        /**********************************************************************/
        #region Constructors
            
        /// <summary>
        /// Creates a new <see cref="DisposeInvoker"/> from a given <see cref="Action"/>.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be invoked when <see cref="Dispose"/> is called.</param>
        /// <exception cref="ArgumentNullException">Throws if action is null.</exception>
        public DisposeInvoker(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            _action = action;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IDisposable

        /// <summary>
        /// Invokes the <see cref="Action"/> given on construction. The action is only invoked the first time <see cref="Dispose"/> is called.
        /// </summary>
        public void Dispose()
        {
            if (!_hasDisposed)
            {
                _action.Invoke();
                _hasDisposed = true;
            }
        }

        #endregion IDisposable

        /**********************************************************************/
        #region Private Fields

        private bool _hasDisposed;

        private readonly Action _action;

        #endregion Private Fields
    }
}

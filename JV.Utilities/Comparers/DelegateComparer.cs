using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Comparers
{
    /// <summary>
    /// An implementation of <see cref="IComparer{T}"/> via delegates.
    /// </summary>
    public class DelegateComparer<T> : IComparer<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new comparer which implements <see cref="IComparer{T}.Compare"/>
        /// with the given delegate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for compareDelegate.</exception>
        /// <param name="compareDelegate">The delegate to use for <see cref="IComparer{T}.Compare(T, T)"/></param>
        public DelegateComparer(Func<T, T, int> compareDelegate)
        {
            if (compareDelegate == null)
                throw new ArgumentNullException(nameof(compareDelegate));

            _compareDelegate = compareDelegate;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IComparer

        /// <summary>
        /// See <see cref="IComparer{T}.Compare(T, T)"/>.
        /// Invokes and returns the result of the delegate given on construction.
        /// </summary>
        public int Compare(T x, T y) => _compareDelegate.Invoke(x, y);

        #endregion IComparer

        /**********************************************************************/
        #region Private Fields

        private Func<T, T, int> _compareDelegate;

        #endregion Private Fields
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Comparers
{
    /// <summary>
    /// An implementation of <see cref="IEqualityComparer{T}"/> via delegates.
    /// </summary>
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new comparer which implements <see cref="IEqualityComparer{T}.Equals"/> and <see cref="IEqualityComparer{T}.GetHashCode(T)"/>
        /// with the given delegates.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for equalsDelegate and getHashCodeDelegate.</exception>
        /// <param name="equalsDelegate">The delegate to use for <see cref="Equals(T, T)"/>.</param>
        /// <param name="getHashCodeDelegate">The delegate to use for <see cref="GetHashCode(T)"/>.</param>
        public DelegateEqualityComparer(Func<T, T, bool> equalsDelegate, Func<T, int> getHashCodeDelegate)
        {
            if (equalsDelegate == null)
                throw new ArgumentNullException(nameof(equalsDelegate));

            if (getHashCodeDelegate == null)
                throw new ArgumentNullException(nameof(getHashCodeDelegate));

            _equalsDelegate = equalsDelegate;
            _getHashCodeDelegate = getHashCodeDelegate;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IEqualityComparer

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.Equals(T, T)"/>.
        /// Invokes and returns the result of the delegate given on construction.
        /// </summary>
        public bool Equals(T x, T y) => _equalsDelegate.Invoke(x, y);

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// Invokes and returns the result of the delegate given on construction.
        /// </summary>
        public int GetHashCode(T x) => _getHashCodeDelegate.Invoke(x);

        #endregion IEqualityComparer

        /**********************************************************************/
        #region Private Fields

        private Func<T, T, bool> _equalsDelegate;
        private Func<T, int> _getHashCodeDelegate;

        #endregion Private Fields
    }
}

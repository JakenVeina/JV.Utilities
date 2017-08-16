using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace JV.Utilities.Comparers
{
    /// <summary>
    /// Compares objects implementing <see cref="ISet{T}"/>, using <see cref="ISet{T}.SetEquals(IEnumerable{T})"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of items contained in the sets to be compared.</typeparam>
    public class SetEqualityComparer<TValue> : IEqualityComparer<ISet<TValue>> 
    {
        /**********************************************************************/
        #region Static Fields

        /// <summary>
        /// Provides a global instance of the comparer.
        /// </summary>
        public static readonly SetEqualityComparer<TValue> Default = new SetEqualityComparer<TValue>();

        #endregion Static Fields

        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="SetEqualityComparer{TValue}"/>.
        /// </summary>
        public SetEqualityComparer() { }

        #endregion Constructors

        /**********************************************************************/
        #region IEqualityComparer

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.Equals(T, T)"/>.
        /// </summary>
        public virtual bool Equals(ISet<TValue> x, ISet<TValue> y)
        {
            return EqualityComparer<object>.Default.Equals(x, y) || ((x != null) && (y != null) && x.SetEquals(y));
        }

        /// <summary>
        /// See <see cref="IEqualityComparer{T}.GetHashCode(T)"/>.
        /// </summary>
        public virtual int GetHashCode(ISet<TValue> set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            unchecked
            {
                int hash = (int)2166136261;
                int hashMod = 16777619;

                // ISet.SetEquals() doesn't care about order, so we need to do the same
                foreach (var value in set.Select(x => x.GetHashCode()).OrderBy(x => x))
                    hash = (hash * hashMod) ^ value;
                return hash;
            }
        }

        #endregion IEqualityComparer
    }
}

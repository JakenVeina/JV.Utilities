using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerator{T}"/>.
    /// </summary>
    public static class EnumeratorExtensions
    {
        /// <summary>
        /// Advances a given enumerator to its next item, and returns it, or returns the default value for the enumerated type, if no next item is available.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for @this.</exception>
        /// <typeparam name="T">The type of the items being enumerated.</typeparam>
        /// <param name="this">The enumerator whose next item is to be retrieved.</param>
        /// <returns><see cref="IEnumerator{T}.Current"/>, after advancing the enumerator, or the default value for the return type if no more items were available.</returns>
        public static T GetNext<T>(this IEnumerator<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (@this.MoveNext())
                return @this.Current;

            return default(T);
        }

        /// <summary>
        /// Advances a given enumerator to its next item, and returns it, or returns the default value for the enumerated type, if no next item is available.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for @this.</exception>
        /// <param name="this">The enumerator whose next item is to be retrieved.</param>
        /// <returns><see cref="IEnumerator.Current"/>, after advancing the enumerator, or null if no more items were available.</returns>
        public static object GetNext(this IEnumerator @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (@this.MoveNext())
                return @this.Current;

            return null;
        }

        /// <summary>
        /// Enumerates all remaining values from a given enumerator, as a new enumerable object.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for @this.</exception>
        /// <typeparam name="T">The type of the items being enumerated.</typeparam>
        /// <param name="this">The enumerator whose items are to be retrieved.</param>
        /// <returns>An enumerable object for the remaining items in the given enumerator.</returns>
        public static IEnumerable<T> GetRemaining<T>(this IEnumerator<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            // Run yielding in a separate method, so exceptions get thrown on call, not on iteration.
            return GetRemainingInternal(@this);
        }

        private static IEnumerable<T> GetRemainingInternal<T>(IEnumerator<T> @this)
        {
            while (@this.MoveNext())
                yield return @this.Current;
        }

        /// <summary>
        /// Enumerates all remaining values from a given enumerator, as a new enumerable object.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for @this.</exception>
        /// <param name="this">The enumerator whose items are to be retrieved.</param>
        /// <returns>An enumerable object for the remaining items in the given enumerator.</returns>
        public static IEnumerable GetRemaining(this IEnumerator @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            // Run yielding in a separate method, so exceptions get thrown on call, not on iteration.
            return GetRemainingInternal(@this);
        }

        private static IEnumerable GetRemainingInternal(IEnumerator @this)
        {
            while (@this.MoveNext())
                yield return @this.Current;
        }
    }
}

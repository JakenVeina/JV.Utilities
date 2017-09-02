using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Contains extension methods for operatin on <see cref="IList{T}"/> and <see cref="IReadOnlyList{T}"/> objects
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> object which enumerates the valid index values of a given <see cref="IReadOnlyList{T}"/>,
        /// in ascending order. Modification of the source <see cref="IReadOnlyList{T}"/> during enumeration of the index values is supported.
        /// However, the enumeration does not account for index positions that may have been added or removed. It merely ensures that the
        /// bounds of the source list are not exceeded.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="this"/>.</typeparam>
        /// <param name="this">The array whose index values are to be enumerated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object for enumerating the valid index values of <paramref name="this"/>.</returns>
        public static IEnumerable<int> IndexRange<T>(this IReadOnlyList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return IndexRangeInternal(() => @this.Count);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> object which enumerates the valid index values of a given <see cref="IReadOnlyList{T}"/>,
        /// in descending order. Modification of the source <see cref="IReadOnlyList{T}"/> during enumeration of the index values is supported.
        /// However, the enumeration does not account for index positions that may have been added or removed. It merely ensures that the
        /// bounds of the source list are not exceeded.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="this"/>.</typeparam>
        /// <param name="this">The array whose index values are to be enumerated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object for enumerating the valid index values of <paramref name="this"/>.</returns>
        public static IEnumerable<int> ReverseIndexRange<T>(this IReadOnlyList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return ReverseIndexRangeInternal(() => @this.Count);
        }

        // IReadOnlyList<T> is not native to .NET 4.0, so core classes don't implement it, and can't use the above methods.
        // The below methods are removed for .NET 4.5+ since they would otherwise generate ambiguous reference warnings.
#if NET40
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> object which enumerates the valid index values of a given <see cref="IList{T}"/>,
        /// in ascending order. Modification of the source <see cref="IList{T}"/> during enumeration of the index values is supported.
        /// However, the enumeration does not account for index positions that may have been added or removed. It merely ensures that the
        /// bounds of the source list are not exceeded.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="this"/>.</typeparam>
        /// <param name="this">The array whose index values are to be enumerated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object for enumerating the valid index values of <paramref name="this"/>.</returns>
        public static IEnumerable<int> IndexRange<T>(this IList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return IndexRangeInternal(() => @this.Count);
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> object which enumerates the valid index values of a given <see cref="IList{T}"/>,
        /// in descending order. Modification of the source <see cref="IList{T}"/> during enumeration of the index values is supported.
        /// However, the enumeration does not account for index positions that may have been added or removed. It merely ensures that the
        /// bounds of the source list are not exceeded.
        /// </summary>
        /// <typeparam name="T">The type of items in <paramref name="this"/>.</typeparam>
        /// <param name="this">The array whose index values are to be enumerated.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> object for enumerating the valid index values of <paramref name="this"/>.</returns>
        public static IEnumerable<int> ReverseIndexRange<T>(this IList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return ReverseIndexRangeInternal(() => @this.Count);
        }
#endif

        private static IEnumerable<int> IndexRangeInternal(Func<int> getCount)
        {
            for (var i = 0; i < getCount.Invoke(); ++i)
                yield return i;
        }

        private static IEnumerable<int> ReverseIndexRangeInternal(Func<int> getCount)
        {
            for (var i = getCount.Invoke() - 1; i >= 0; --i)
            {
                yield return i;

                while (i > getCount.Invoke())
                    --i;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        public static IEnumerable<int> IndexRange<T>(this T[] @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return Enumerable.Range(0, @this.Length);
        }

        public static IEnumerable<int> ReverseIndexRange<T>(this T[] @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return ReverseIndexRangeInternal(@this.Length);
        }

        private static IEnumerable<int> ReverseIndexRangeInternal(int length)
        {
            for (int i = (length - 1); i >= 0; --i)
                yield return i;
        }

        /// <summary>
        /// Returns an array containing the items in the given array, specified by the given index and length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if index or length is negative, or specifies an item outside of the bounds of this.</exception>
        /// <typeparam name="T">The type of items in the given array.</typeparam>
        /// <param name="this">The array to retrieve items from.</param>
        /// <param name="index">The 0-based index of the first item to be retrieved from the given array.</param>
        /// <param name="length">The number of items to be retrieved, sequentially.</param>
        /// <returns>An array containing the requested items from the given array.</returns>
        public static T[] SubArray<T>(this T[] @this, int index, int length)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Cannot be negative");

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), length, "Cannot be negative");

            if (index >= @this.Length)
                throw new ArgumentOutOfRangeException(nameof(index), index, $"Cannot exceed the length of {nameof(@this)}");

            if(length > (@this.Length - index))
                throw new ArgumentOutOfRangeException(nameof(length), length, $"Cannot exceed the length of {nameof(@this)}");

            T[] result = new T[length];
            Array.Copy(@this, index, result, 0, length);
            return result;
        }
    }
}

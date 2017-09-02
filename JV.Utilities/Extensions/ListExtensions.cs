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
        public static IEnumerable<int> IndexRange<T>(this IReadOnlyList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return IndexRangeInternal(() => @this.Count);
        }

        public static IEnumerable<int> ReverseIndexRange<T>(this IReadOnlyList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return ReverseIndexRangeInternal(() => @this.Count);
        }

// IReadOnlyList<T> is not native to .NET 4.0, so core classes don't implement it, and can't use the above methods.
// The below methods are removed for .NET 4.5+ since they would otherwise generate ambiguous reference warnings.
#if NET40
        public static IEnumerable<int> IndexRange<T>(this IList<T> @this)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return IndexRangeInternal(() => @this.Count);
        }

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

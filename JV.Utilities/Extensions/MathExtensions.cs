using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Math"/>.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Checks whether a given comparable value is within a range, defined by given minimum and maximum values.
        /// </summary>
        /// <typeparam name="T">The type of values to be compared.</typeparam>
        /// <param name="this">The value to check against min and max.</param>
        /// <param name="min">The minimum value of the range to check against.</param>
        /// <param name="max">The maximum value of the range to check against.</param>
        /// <param name="comparer">The comparer to be used for all value comparisons. Use null to specify <see cref="Comparer{T}.Default"/>.</param>
        /// <returns>
        /// True if value is greater than or equal to min and less than or equal to max,
        /// as determined by the given <see cref="IComparer{T}"/>; False otherwise.
        /// </returns>
        public static bool IsInRange<T>(this T @this, T min, T max, IComparer<T> comparer = null) where T : IComparable<T>
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            comparer = comparer ?? Comparer<T>.Default;

            if (comparer.Compare(min, max) > 0)
                throw new ArgumentOutOfRangeException(nameof(max), max, "Cannot be less than min");

            return (comparer.Compare(@this, min) >= 0) && (comparer.Compare(@this, max) <= 0);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for use of <see cref="Nullable{T}"/> types.
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// Casts a given value-type value as <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be cast.</typeparam>
        /// <param name="this">The value to be cast.</param>
        /// <returns>The newly-created <see cref="Nullable{T}"/> struct.</returns>
        public static T? AsNullable<T>(this T @this) where T : struct
            => new T?(@this);

        /// <summary>
        /// Casts each value-type value within an <see cref="IEnumerable{T}"/> as <see cref="Nullable{T}"/>.
        /// Does not enumerate the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of value to be cast.</typeparam>
        /// <param name="this">The <see cref="IEnumerable{T}"/> whose values are to be cast.</param>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="this"/> is null.</exception>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the new <see cref="Nullable{T}"/> structs.</returns>
        public static IEnumerable<T?> CastAsNullable<T>(this IEnumerable<T> @this) where T : struct
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            return @this.Select(x => new T?(x));
        }
    }
}

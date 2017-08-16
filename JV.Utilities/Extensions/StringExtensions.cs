using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Equivalent to <see cref="string.IndexOf(char)"/> for a given n'th occurrence of the search character, rather than just the first.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if occurrence is less than 1.</exception>
        /// <param name="this">The string to be searched</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <param name="occurrence">The 1-based index of the occurrence to search for</param>
        /// <returns>The 0-based index of value within this, if it was found; -1 otherwise</returns>
        public static int IndexOfOccurrence(this string @this, char value, int occurrence)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));
            
            if (occurrence < 1)
                throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence, "must be greater than 0");

            int result = -1;
            for (int i = 0; i < occurrence; ++i)
                if ((result = @this.IndexOf(value, (result + 1))) == -1)
                    break;

            return result;
        }

        /// <summary>
        /// Equivalent to <see cref="string.IndexOf(string)"/> for a given n'th occurrence of the search string, rather than just the first.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this and value.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if occurrence is less than 1.</exception>
        /// <param name="this">The string to be searched</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="comparisonType">The type of string comparison to be performed when searching for the given value.</param>
        /// <param name="occurrence">The 1-based index of the occurrence to search for.</param>
        /// <returns>The 0-based index of value within this, if it was found; -1 otherwise</returns>
        public static int IndexOfOccurrence(this string @this, string value, int occurrence, StringComparison comparisonType = default(StringComparison))
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (occurrence < 1)
                throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence, "must be greater than 0");

            if (occurrence > @this.Length)
                return -1;

            int result = -1;
            for (int i = 0; i < occurrence; ++i)
                if ((result = @this.IndexOf(value, (result + 1), comparisonType)) == -1)
                    break;

            return result;
        }

        /// <summary>
        /// Overload of <see cref="string.Replace(char, char)"/> which replaces only the n'th occurrence of oldChar with newChar, rather than all of them.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if occurrence is less than 1.</exception>
        /// <param name="this">The string to be operated upon.</param>
        /// <param name="oldChar">The Unicode character to be replaced.</param>
        /// <param name="newChar">The Unicode replacement character.</param>
        /// <param name="occurrence">The 1-based index of the occurrence to search for.</param>
        /// <returns>The value of the original string, with the requested character replaced, if it was found.</returns>
        public static string Replace(this string @this, char oldChar, char newChar, int occurrence)
        {
            if(@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (occurrence < 1)
                throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence, "must be greater than 0");

            var index = @this.IndexOfOccurrence(oldChar, occurrence);
            if (index == -1)
                return @this;

            return string.Join("", @this.Substring(0, index), newChar.ToString(), @this.Substring(index + 1));
        }

        /// <summary>
        /// Overload of <see cref="string.Replace(string, string)"/> which replaces only the n'th occurrence of oldChar with newChar, rather than all of them.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this, oldValue, and newValue.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws if occurrence is less than 1.</exception>
        /// <param name="this">The string to be operated upon.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The replacement string.</param>
        /// <param name="occurrence">The 1-based index of the occurrence to search for.</param>
        /// <param name="comparisonType">The type of string comparison to be performed when searching for the given value.</param>
        /// <returns>The value of the original string, with the requested string replaced, if it was found.</returns>
        public static string Replace(this string @this, string oldValue, string newValue, int occurrence, StringComparison comparisonType = default(StringComparison))
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            if (occurrence < 1)
                throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence, "must be greater than 0");

            var index = @this.IndexOfOccurrence(oldValue, occurrence, comparisonType);
            if (index == -1)
                return @this;

            return string.Join("", @this.Substring(0, index), newValue, @this.Substring(index + oldValue.Length));
        }

        /// <summary>
        /// Overload of <see cref="string.Contains(string)"/> which includes a StringComparison parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws for this and value.</exception>
        /// <param name="this">The string to be searched.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="comparisonType">The type of string comparison to be performed when searching for the given value.</param>
        /// <returns>True if the given string was found within the source string; False otherwise.</returns>
        public static bool Contains(this string @this, string value, StringComparison comparisonType)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return (@this.IndexOf(value, comparisonType) >= 0);
        }
    }
}

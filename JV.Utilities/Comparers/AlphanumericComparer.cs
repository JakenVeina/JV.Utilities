using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Comparers
{
    /// <summary>
    /// Performs intelligent alphanumeric comparison of strings.
    /// I.E. numeric segments within strings are sorted numerically, rather than alphabetically.
    /// </summary>
    public class AlphanumericComparer : IComparer<string>, IComparer
    {
        /**********************************************************************/
        #region Static Fields

        /// <summary>
        /// Provides a global instance of the comparer.
        /// </summary>
        public static readonly AlphanumericComparer Default = new AlphanumericComparer();

        #endregion Static Fields

        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Create a new <see cref="AlphanumericComparer"/> for the given <see cref="StringComparison"/> comparison type.
        /// </summary>
        /// <param name="comparisonType">The type of string comparison to be performed on each non-numeric chunk of the strings being compared.</param>
        public AlphanumericComparer(StringComparison comparisonType = StringComparison.CurrentCulture)
        {
            _comparisonType = comparisonType;
        }

        #endregion Constructors

        /**********************************************************************/
        #region IComparer

        /// <summary>
        /// See <see cref="IComparer{T}.Compare(T, T)"/>.
        /// Comparison is performed by splitting each string into numeric and non-numeric segments, and sorting by each of these, sequentially.
        /// </summary>
        public int Compare(string x, string y)
        {
            int result;
            
            // For null and empty strings, we can't do any numeric/alphabetic tokenization,
            // so we can just pass off the work to string's comparer (which means we need to clip the results to -1,0,1
            if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
            {
                result = string.Compare(x, y, _comparisonType);
                return (result <= -1) ? -1 : (result >= 1) ? 1 : 0;
            }
            
            // Temporary variables for analysis
            var xLen = x.Length;
            var yLen = y.Length;
            var xPos = 0;
            var yPos = 0;
            var xBuf = new char[xLen];
            var yBuf = new char[yLen];

            int xBufLen;
            int yBufLen;
            bool xCollectDigits;
            bool yCollectDigits;

            // Iterate through the two strings, in parallel
            while ((xPos < xLen) && (yPos < yLen))
            {
                // For each string, collect characters from the string into the buffer,
                // until we see the character type change, or we reach the end of the string.
                // This splits the strings up into separate numeric and alphabetic chunks.
                xCollectDigits = char.IsDigit(x[xPos]);
                xBufLen = 0;
                do xBuf[xBufLen++] = x[xPos++];
                while ((xPos < xLen) && (char.IsDigit(x[xPos]) == xCollectDigits));

                yCollectDigits = char.IsDigit(y[yPos]);
                yBufLen = 0;
                do yBuf[yBufLen++] = y[yPos++];
                while ((yPos < yLen) && (char.IsDigit(y[yPos]) == yCollectDigits));

                // Convert each chunk to its own string for compatibility with int.Parse and string.Compare()
                var xChunk = new string(xBuf, 0, xBufLen);
                var yChunk = new string(yBuf, 0, yBufLen);

                // If both chunks are numeric, convert them and compare them.
                if (xCollectDigits && yCollectDigits)
                    result = int.Parse(xChunk).CompareTo(int.Parse(yChunk));
                // Otherwise, just compare them as strings
                else
                {
                    result = string.Compare(xChunk, yChunk, _comparisonType);
                    result = (result <= -1) ? -1 : (result >= 1) ? 1 : 0;
                }

                // If the chunks arnen't equal, we're done.
                if (result != 0)
                    return result;
            }

            // If we've reached this point, the strings are equal, except that we might have reached the end of one string but not the other.
            // Sort the shorter one first.
            return xLen.CompareTo(yLen);
        }

        /// <summary>
        /// See <see cref="IComparer{T}.Compare(T, T)"/>.
        /// Comparison is performed by converting each object to its string representation, then sorting via <see cref="Compare(string, string)"/>.
        /// </summary>
        public int Compare(object x, object y)
        {
            // Convert each object to its string representation, then pass it to Compare().
            // If the object can't be directly converted to a string, just call its ToString() method.
            // Pass through null's.
            var xString = x as string;
            if ((xString == null) && (x != null))
                xString = x.ToString();

            var yString = y as string;
            if ((yString == null) && (y != null))
                yString = y.ToString();

            return Compare(xString, yString);
        }

        #endregion IComparer

        /*****************************************************************/
        #region Private Fields

        private StringComparison _comparisonType;

        #endregion Private Fields
    }
}

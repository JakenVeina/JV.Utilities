using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace JV.Utilities.Math
{
    /// <summary>
    /// Represents a range of values defined by a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public struct MinMaxPair<T> : IEquatable<MinMaxPair<T>>, IComparable<MinMaxPair<T>>, IComparable  where T : IComparable<T>
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Construct a new pair from the given min and max values.
        /// </summary>
        /// <param name="min">The minimum value of the new range.</param>
        /// <param name="max">The maximum value of the new range.</param>
        /// <param name="comparer">
        /// The comparer to be used for comparing the minimum and maximum values.
        /// <see cref="Comparer{T}.Default"/> is used if null is given.
        /// </param>
        /// <exception cref="ArgumentException">Throws if min is greater than max</exception>
        public MinMaxPair(T min, T max, IComparer<T> comparer = null)
        {
            var comparison = comparer?.Compare(min, max) ?? Comparer<T>.Default.Compare(min, max);
            if (comparison > 0)
                throw new ArgumentOutOfRangeException(nameof(max), max, $"Cannot be less than {nameof(min)}");

            Min = min;
            Max = max;
            IsRange = comparison != 0;

            _comparer = comparer;

            _hashCode = CalculateHashCode(min, max);
        }

        /// <summary>
        /// Construct a new pair from the given single value, using that value as both the <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="value">The value to use for <see cref="Min"/> and <see cref="Max"/>.</param>
        /// <param name="comparer">
        /// The comparer to be used for comparing the minimum and maximum values.
        /// <see cref="Comparer{T}.Default"/> is used if null is given.
        /// </param>
        public MinMaxPair(T value, IComparer<T> comparer = null)
        {
            Min = value;
            Max = value;
            IsRange = false;

            _comparer = comparer;

            _hashCode = CalculateHashCode(value, value);
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The minimum value of the range.
        /// </summary>
        [DataMember(Order = 0)]
        public T Min { get; private set; }

        /// <summary>
        /// The maximum value of the range.
        /// </summary>
        [DataMember(Order = 1)]
        public T Max { get; private set; }

        /// <summary>
        /// Flag indicating whether or not <see cref="Min"/> and <see cref="Max"/> actually define a range (are not equal).
        /// </summary>
        public bool IsRange { get; private set; }

        #endregion Properties

        /**********************************************************************/
        #region Public Methods

        /// <summary>
        /// Checks whether a given value is contained within the range defined by <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// True if value is greater than or equal to <see cref="Min"/> and less than or equal to <see cref="Max"/>,
        /// as determined by the <see cref="IComparer{T}"/> given upon construction; False otherwise.
        /// </returns>
        public bool Contains(T value) => (Comparer.Compare(value, Min) >= 0) && (Comparer.Compare(value, Max) <= 0);

        #endregion Public Methods

        /**********************************************************************/
        #region IEquatable

        /// <summary>
        /// Checks if the current pair is equal to another.
        /// </summary>
        /// <param name="pair">The <see cref="MinMaxPair{T}"/> to compare against</param>
        /// <returns>True of the two pairs are equal; False otherwise.</returns>
        public bool Equals(MinMaxPair<T> pair) =>
            (Comparer.Compare(Min, pair.Min) == 0) &&
            (Comparer.Compare(Max, pair.Max) == 0);

        /// <summary>
        /// Checks if the current pair is equal to an arbitrary object.
        /// </summary>
        /// <param name="obj">The object to compare against</param>
        /// <returns>True of the two objects are equal, False otherwise.</returns>
        public override bool Equals(object obj) => (obj is MinMaxPair<T>) ? Equals((MinMaxPair<T>)obj) : false;

        /// <summary>
        /// See <see cref="ValueType.GetHashCode"/>.
        /// </summary>
        public override int GetHashCode() => _hashCode;
        private int _hashCode;

        #endregion IEquatable

        /**********************************************************************/
        #region IComparable

        /// <summary>
        /// See <see cref="IComparable{T}.CompareTo(T)"/>
        /// </summary>
        public int CompareTo(MinMaxPair<T> pair)
        {
            var result = Comparer.Compare(Min, pair.Min);
            if (result == 0)
                result = Comparer.Compare(Max, pair.Max);

            return result;
        }

        /// <summary>
        /// See <see cref="IComparable.CompareTo(object)"/>
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is MinMaxPair<T>)
                return CompareTo((MinMaxPair<T>)obj);
            
            throw new ArgumentException($"Cannot compare {GetType()} to {obj.GetType()}", nameof(obj));
        }

        #endregion IComparable

        /**********************************************************************/
        #region Operators

        /// <summary>
        /// See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/index.
        /// Checks for equality between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public static bool operator == (MinMaxPair<T> x, MinMaxPair<T> y) =>
            (x.Comparer.Compare(x.Min, y.Min) == 0) &&
            (x.Comparer.Compare(x.Max, y.Max) == 0) &&
            (y.Comparer.Compare(x.Min, y.Min) == 0) && 
            (y.Comparer.Compare(x.Max, y.Max) == 0);

        /// <summary>
        /// See https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/index.
        /// Checks for equality between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public static bool operator != (MinMaxPair<T> x, MinMaxPair<T> y) =>
            (x.Comparer.Compare(x.Min, y.Min) != 0) ||
            (x.Comparer.Compare(x.Max, y.Max) != 0) ||
            (y.Comparer.Compare(x.Min, y.Min) != 0) ||
            (y.Comparer.Compare(x.Max, y.Max) != 0);

        /// <summary>
        /// Up-converts a value into a MinMaxPair of the same type. Min and Max are both set to the given Value.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        public static implicit operator MinMaxPair<T>(T value) => new MinMaxPair<T>(value, value);

        #endregion Operators

        /**********************************************************************/
        #region Value Type Methods

        /// <summary>
        /// <para>
        /// Returns the string representation of the current pair.
        /// </para>
        /// If the pair represents a range, the string representations of Min and Max are returned,
        /// using the standard mathematical notation for ranges (E.G. [1-5]). 
        /// Otherwise, the string representation of the underlying single value is returned, as-is.
        /// </summary>
        /// <returns>See <see cref="ValueType.ToString"/>.</returns>
        public override string ToString() => IsRange ? $"[{Min}-{Max}]" : Min.ToString();

        #endregion Value Type Methods

        /**********************************************************************/
        #region Private Properties

        private IComparer<T> Comparer
        {
            get
            {
                if (_comparer == null)
                    _comparer = Comparer<T>.Default;

                return _comparer;
            }
        }
        private IComparer<T> _comparer;

        #endregion Private Properties

        /**********************************************************************/
        #region Private Methods

        private static int CalculateHashCode(T min, T max)
        {
            unchecked
            {
                int hash = (int)2166136261;
                int hashMod = 16777619;
                hash = (hash * hashMod) ^ min.GetHashCode();
                hash = (hash * hashMod) ^ max.GetHashCode();
                return hash;
            }
        }

        #endregion Private Methods

        /**********************************************************************/
        #region DataContract

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            IsRange = Comparer.Compare(Min, Max) != 0;

            _hashCode = CalculateHashCode(Min, Max);
        }

        #endregion DataContract
    }
}

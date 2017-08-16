using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JV.Utilities.Wpf.ValueConverters
{
    /// <summary>
    /// Provides methods for conversion between <see cref="Boolean"/> values and <see cref="Visibility"/> values.
    /// </summary>
    public class BooleanToVisibilityValueConverter : IValueConverter
    {
        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The <see cref="Visibility"/> value to be equated with a <see cref="Boolean"/> value of false,
        /// for <see cref="Boolean"/> to <see cref="Visibility"/> conversions.
        /// </summary>
        public Visibility WhenFalse { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region IValueConverter

        /// <summary>
        /// <para>
        /// See <see cref="IValueConverter.Convert"/>.
        /// Performs <see cref="Boolean"/> to <see cref="Visibility"/> and <see cref="Visibility"/> to <see cref="Boolean"/> conversions.
        /// </para>
        /// <para>
        /// <see cref="Boolean"/> to <see cref="Visibility"/> conversions equate true to <see cref="Visibility.Visible"/>,
        /// and false to <see cref="WhenFalse"/>.
        /// </para>
        /// <para>
        /// <see cref="Visibility"/> to <see cref="Boolean"/> conversions equate <see cref="Visibility.Visible"/> to true,
        /// and otherwise return false.
        /// </para>
        /// <para>
        /// <paramref name="parameter"/> and <paramref name="culture"/> are unused.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws if targetType is null.</exception>
        /// <exception cref="NotSupportedException">
        /// Throws if the requested conversion (as defined by value and targetType)
        /// is not a <see cref="Boolean"/> to <see cref="Visibility"/> or <see cref="Visibility"/> to <see cref="Boolean"/> conversion.
        /// </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if ((value is bool) && targetType.IsAssignableFrom(typeof(Visibility)))
                return ConvertBooleanToVisibility((bool)value);

            else if ((value is Visibility) && targetType.IsAssignableFrom(typeof(bool)))
                return ConvertVisibilityToBoolean((Visibility)value);

            throw new NotSupportedException($"Cannot convert {value?.GetType().Name ?? "null"} to {targetType.Name}");
        }

        /// <summary>
        /// Same as <see cref="Convert(object, Type, object, CultureInfo)"/>.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);

        #endregion IValueConverter

        /**********************************************************************/
        #region Private Methods

        private Visibility ConvertBooleanToVisibility(bool value)
            => value ? Visibility.Visible : WhenFalse;

        private bool ConvertVisibilityToBoolean(Visibility value)
            => (value == Visibility.Visible);

        #endregion Private Methods
    }
}

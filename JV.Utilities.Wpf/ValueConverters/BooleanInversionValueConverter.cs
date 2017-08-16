using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JV.Utilities.Wpf.ValueConverters
{
    /// <summary>
    /// Provides methods for inverting <see cref="Boolean"/> values in the WPF View Layer.
    /// </summary>
    public class BooleanInversionValueConverter : IValueConverter
    {
        /**********************************************************************/
        #region IValueConverter

        /// <summary>
        /// <para>
        /// See <see cref="IValueConverter.Convert"/>.
        /// Inverts <see cref="Boolean"/> values.
        /// </para>
        /// <para>
        /// <paramref name="parameter"/> and <paramref name="culture"/> are unused.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws if targetType is null.</exception>
        /// <exception cref="NotSupportedException">
        /// Throws if the requested conversion (as defined by value and targetType
        /// is not a <see cref="Boolean"/> to <see cref="Boolean"/> conversion.
        /// </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if ((value is bool) && targetType.IsAssignableFrom(typeof(Boolean)))
                return !(bool)value;            

            throw new NotSupportedException($"Cannot convert {value?.GetType().Name ?? "null"} to {targetType.Name}");
        }

        /// <summary>
        /// Same as <see cref="Convert(object, Type, object, CultureInfo)"/>.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);

        #endregion IValueConverter
    }
}

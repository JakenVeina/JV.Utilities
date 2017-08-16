using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace JV.Utilities.Wpf.ValueConverters
{
    /// <summary>
    /// Provides methods for conversion and validation between <see cref="Boolean"/> values and <see cref="String"/> values,
    /// using string values "Yes" and "No" for boolean true and false, respectively.
    /// </summary>
    public class BooleanToStringValueConverterAndValidationRule : ValidationRule, IValueConverter
    {
        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The string value to equate with a boolean value of true.
        /// This value is produced by a <see cref="Boolean"/> to <see cref="String"/> conversion,
        /// and is used (case-insensitively) to validate and perform <see cref="String"/> to <see cref="Boolean"/> conversions.
        /// </summary>
        public string WhenTrue { get; set; }

        /// <summary>
        /// The string value to equate with a boolean value of false.
        /// This value is produced by a <see cref="Boolean"/> to <see cref="String"/> conversion,
        /// and is used (case-insensitively) to validate and perform <see cref="String"/> to <see cref="Boolean"/> conversions.
        /// </summary>
        public string WhenFalse { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region IValueConverter

        /// <summary>
        /// <para>
        /// See <see cref="IValueConverter.Convert"/>.
        /// Performs <see cref="Boolean"/> to <see cref="String"/> and <see cref="String"/> to <see cref="Boolean"/> conversions.
        /// </para>
        /// <para>
        /// <see cref="Boolean"/> to <see cref="String"/> conversions are generated based on the values of <see cref="WhenTrue"/> and <see cref="WhenFalse"/>.
        /// </para>
        /// <para>
        /// <see cref="String"/> to <see cref="Boolean"/> conversions are generated based on the values of <see cref="WhenTrue"/> and <see cref="WhenFalse"/>,
        /// insensitive to case and extraneous whitespace.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws if targetType is null.</exception>
        /// <exception cref="ArgumentException">
        /// Throws if a <see cref="String"/> to <see cref="Boolean"/> conversion is requested
        /// and the string representation of value does not match <see cref="WhenTrue"/> or <see cref="WhenFalse"/> (ignoring case and whitespace).</exception>
        /// <exception cref="NotSupportedException">
        /// Throws if the requested conversion (as defined by value and targetType)
        /// is not a <see cref="Boolean"/> to <see cref="String"/> or <see cref="String"/> to <see cref="Boolean"/> conversion.
        /// </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolResult;

            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            if ((value is bool) && targetType.IsAssignableFrom(typeof(string)))
                return ConvertBoolToString((bool)value);
            else if (targetType.IsAssignableFrom(typeof(bool)))
            {
                var str = (value as string) ?? value?.ToString();

                if (!TryConvertStringToBool(str, out boolResult))
                    throw new ArgumentException(MakeConversionErrorMessage(), nameof(value));

                return boolResult;
            }

            throw new NotSupportedException($"Cannot convert {value?.GetType().Name ?? "null"} to {targetType.Name}");
        }

        /// <summary>
        /// Same as <see cref="Convert(object, Type, object, CultureInfo)"/>.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);

        #endregion IValueConverter

        /**********************************************************************/
        #region ValidationRule Overrides

        /// <summary>
        /// See <see cref="ValidationRule.Validate(object, CultureInfo)"/> and <see cref="Convert"/>.
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool temp;

            if (value is bool)
                return new ValidationResult(true, null);

            string str = (value as string) ?? value?.ToString();

            if (!TryConvertStringToBool(str, out temp))
                return new ValidationResult(false, MakeConversionErrorMessage());

            return new ValidationResult(true, null);
        }

        #endregion ValidationRule Overrides

        /**********************************************************************/
        #region Private Methods

        private string ConvertBoolToString(bool value)
            => value ? WhenTrue : WhenFalse;

        private bool TryConvertStringToBool(string value, out bool result)
        {
            var str = value?.Trim().ToLower();

            if (string.Equals(str, WhenTrue, StringComparison.CurrentCultureIgnoreCase))
            {
                result = true;
                return true;
            }
            else if (string.Equals(str, WhenFalse, StringComparison.CurrentCultureIgnoreCase))
            {
                result = false;
                return true;
            }
            else
            {
                result = default(bool);
                return false;
            }
        }

        private string MakeConversionErrorMessage()
            => $"Value must be \"{WhenTrue}\" or \"{WhenFalse}\" (ignoring case and whitespace)";

        #endregion Private Methods
    }
}

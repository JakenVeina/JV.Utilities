using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Exceptions;

using JV.Utilities.Wpf.ValueConverters;

namespace JV.Utilities.Wpf.Tests.ValueConverters
{
    [TestFixture]
    public class Int32ToStringValueConverterAndValidationRuleTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly CultureInfo TestCultureInfo =
            new CultureInfo("en-US")
            {
                NumberFormat = new NumberFormatInfo()
                {
                    NegativeSign = "!"
                }
            };

        private static readonly object[] TestCases_InvalidConversions =
        {
            new object[] { null,         typeof(string) },
            new object[] { true,         typeof(string) },
            new object[] { 0,            typeof(bool) },
            new object[] { string.Empty, typeof(bool) },
            new object[] { string.Empty, typeof(string) }
        };

        private static readonly object[] TestCases_ValidInt32ToStringConversions =
        {
            new object[] {  1, CultureInfo.CurrentCulture, "1",  int.MinValue, int.MaxValue },
            new object[] {  1, CultureInfo.CurrentCulture, "1",  -2,           2 },
            new object[] {  1, CultureInfo.CurrentCulture, "1",   1,           1 },
            new object[] { -1, CultureInfo.CurrentCulture, "-1", int.MinValue, int.MaxValue },
            new object[] { -1, null,                       "-1", int.MinValue, int.MaxValue },
            new object[] { -1, TestCultureInfo,            "!1", int.MinValue, int.MaxValue },
        };

        private static readonly object[] TestCases_ValidInt32ToStringValidations =
            TestCases_ValidInt32ToStringConversions
                .Select(x => (object[])x)
                .Select(x => new[] { x[0], x[1], x[3], x[4] })
                .ToArray();

        private static readonly object[] TestCases_InvalidInt32ToStringConversions =
        {
            new object[] { -2, CultureInfo.CurrentCulture, -1, 1 },
            new object[] {  2, CultureInfo.CurrentCulture, -1, 1 }
        };

        private static readonly object[] TestCases_InvalidInt32ToStringValidations =
            TestCases_InvalidInt32ToStringConversions;

        private static readonly object[] TestCases_ValidStringToInt32Conversions =
        {
            new object[] {  "1", CultureInfo.CurrentCulture,  1, int.MinValue, int.MaxValue },
            new object[] {  "1", CultureInfo.CurrentCulture,  1, -2,           2 },
            new object[] {  "1", CultureInfo.CurrentCulture,  1,  1,           1 },
            new object[] {   1,  CultureInfo.CurrentCulture,  1,  1,           1 },

            new object[] {  "1", CultureInfo.CurrentCulture,  1, int.MinValue, int.MaxValue },
            new object[] {  "1", CultureInfo.CurrentCulture,  1, -2,           2 },
            new object[] {  "1", CultureInfo.CurrentCulture,  1,  1,           1 },
            new object[] { "-1", CultureInfo.CurrentCulture, -1, int.MinValue, int.MaxValue },
            new object[] { "-1", null,                       -1, int.MinValue, int.MaxValue },
            new object[] { "!1", TestCultureInfo,            -1, int.MinValue, int.MaxValue },
        };

        private static readonly object[] TestCases_ValidStringToInt32Validations =
            TestCases_ValidStringToInt32Conversions
                .Select(x => (object[])x)
                .Select(x => new[] { x[0], x[1], x[3], x[4] })
                .ToArray();

        private static readonly object[] TestCases_InvalidStringToInt32Conversions =
        {
            new object[] {  null,         CultureInfo.CurrentCulture,  int.MinValue, int.MaxValue },
            new object[] {  new object(), CultureInfo.CurrentCulture,  int.MinValue, int.MaxValue },
            new object[] {  "a",          CultureInfo.CurrentCulture,  -2,           2 },
            new object[] {  "1",          CultureInfo.CurrentCulture,   2,           2 },
        };

        private static readonly object[] TestCases_InvalidStringToInt32Validations =
            TestCases_InvalidStringToInt32Conversions;

        #endregion Test Data

        /**********************************************************************/
        #region Minimum Tests

        [Test]
        public void Minimum_ByDefault_IsMinValue()
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            uut.Minimum.ShouldBe(int.MinValue);
        }

        [Test]
        public void Minimum_GivenIsGreaterThanMaximum_ThrowsException()
        {
            var minimum = 1;
            var maximum = 0;

            var uut = new Int32ToStringValueConverterAndValidationRule() { Maximum = maximum };

            var result = Should.Throw<PropertyValidationException>(() =>
            {
                uut.Minimum = minimum;
            });

            result.ShouldSatisfyAllConditions(
                () => result.PropertyName.ShouldBe(nameof(uut.Minimum)),
                () => result.InvalidValue.ShouldBe(minimum),
                () => result.Message.ShouldContain(nameof(uut.Maximum)),
                () => result.Message.ShouldContain(maximum.ToString()));
        }

        [TestCase(1)]
        public void Minimum_Otherwise_SetsToGiven(int minimum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum
            };

            uut.Minimum.ShouldBe(minimum);
        }

        #endregion Minimum Tests

        /**********************************************************************/
        #region Maximum Tests

        [Test]
        public void Maximum_ByDefault_IsMaxValue()
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            uut.Maximum.ShouldBe(int.MaxValue);
        }

        [Test]
        public void Maximum_GivenIsLessThanMinimum_ThrowsException()
        {
            var minimum = 0;
            var maximum = -1;

            var uut = new Int32ToStringValueConverterAndValidationRule() { Minimum = minimum };

            var result = Should.Throw<PropertyValidationException>(() =>
            {
                uut.Maximum = maximum;
            });

            result.ShouldSatisfyAllConditions(
                () => result.PropertyName.ShouldBe(nameof(uut.Maximum)),
                () => result.InvalidValue.ShouldBe(maximum),
                () => result.Message.ShouldContain(nameof(uut.Minimum)),
                () => result.Message.ShouldContain(minimum.ToString()));
        }

        [TestCase(1)]
        public void Maximum_Otherwise_SetsToGiven(int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Maximum = maximum
            };

            uut.Maximum.ShouldBe(maximum);
        }

        #endregion Maximum Tests

        /**********************************************************************/
        #region Validate Tests

        [TestCaseSource(nameof(TestCases_ValidInt32ToStringValidations))]
        [TestCaseSource(nameof(TestCases_ValidStringToInt32Validations))]
        public void Validate_ValueIsValid_ReturnsIsValidTrue(object value, CultureInfo cultureInfo, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            uut.Validate(value, cultureInfo).IsValid.ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_InvalidInt32ToStringValidations))]
        [TestCaseSource(nameof(TestCases_InvalidStringToInt32Validations))]
        public void Validate_ValueIsNotValid_ReturnsIsValidFalse(object value, CultureInfo cultureInfo, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            uut.Validate(value, cultureInfo).IsValid.ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToInt32Validations))]
        public void Validate_ValueIsNotValid_ReturnsErrorContentAsExpected(object value, CultureInfo cultureInfo, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var result = uut.Validate(value, cultureInfo).ErrorContent;

            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<string>(),
                () => result.ShouldSatisfyAnyCondition(
                    () => result.ShouldSatisfyAllConditions(
                        () => (result as string)?.ShouldContain(minimum.ToString()),
                        () => (result as string)?.ShouldContain(maximum.ToString())),
                    () => (result as string)?.ShouldContain(value?.ToString() ?? "null")));
        }

        #endregion Validate Tests

        /**********************************************************************/
        #region Convert Tests

        [Test]
        public void Convert_TargetTypeIsNull_ThrowsException()
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            var value = 0;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<ArgumentNullException>(() => uut.Convert(value, targetType, parameter, culture));

            result.ParamName.ShouldBe(nameof(targetType));
        }

        [TestCaseSource(nameof(TestCases_InvalidConversions))]
        public void Convert_ConversionIsInvalid_ThrowsException(object value, Type targetType)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(value?.GetType().Name ?? "null"),
                () => result.Message.ShouldContain(targetType.Name));
        }

        [TestCaseSource(nameof(TestCases_InvalidInt32ToStringConversions))]
        public void Convert_InvalidInt32ToString_ReturnsExpected(int value, CultureInfo culture, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(string);
            var parameter = (object)null;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(value)),
                () => result.ActualValue.ShouldBe(value),
                () => result.Message.ShouldContain(minimum.ToString()),
                () => result.Message.ShouldContain(maximum.ToString()));
        }

        [TestCaseSource(nameof(TestCases_ValidInt32ToStringConversions))]
        public void Convert_ValidInt32ToString_ReturnsExpected(int value, CultureInfo culture, string expectedResult, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(string);
            var parameter = (object)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToInt32Conversions))]
        public void Convert_InvalidStringToInt32_ThrowsException(object value, CultureInfo culture, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(int);
            var parameter = (object)null;

            var result = Should.Throw<Exception>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAnyCondition(
                () => result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeOfType<ArgumentException>(),
                    () => (result as ArgumentException)?.ParamName.ShouldBe(nameof(value)),
                    () => (result as ArgumentException)?.Message.ShouldContain(value?.ToString() ?? "null")),
                () => result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeOfType<ArgumentOutOfRangeException>(),
                    () => (result as ArgumentOutOfRangeException)?.ParamName.ShouldBe(nameof(value)),
                    () => (result as ArgumentOutOfRangeException)?.ActualValue.ShouldBe(value),
                    () => (result as ArgumentOutOfRangeException)?.Message.ShouldContain(minimum.ToString()),
                    () => (result as ArgumentOutOfRangeException)?.Message.ShouldContain(maximum.ToString())));
        }

        [TestCaseSource(nameof(TestCases_ValidStringToInt32Conversions))]
        public void Convert_ValidStringToInt32_ReturnsExpected(object value, CultureInfo culture, int expectedResult, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(int);
            var parameter = (object)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        #endregion Convert Tests

        /**********************************************************************/
        #region ConvertBack Tests

        [Test]
        public void ConvertBack_TargetTypeIsNull_ThrowsException()
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            var value = 0;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ParamName.ShouldBe(nameof(targetType));
        }

        [TestCaseSource(nameof(TestCases_InvalidConversions))]
        public void ConvertBack_ConversionIsInvalid_ThrowsException(object value, Type targetType)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule();

            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(value?.GetType().Name ?? "null"),
                () => result.Message.ShouldContain(targetType.Name));
        }

        [TestCaseSource(nameof(TestCases_InvalidInt32ToStringConversions))]
        public void ConvertBack_InvalidInt32ToString_ReturnsExpected(int value, CultureInfo culture, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(string);
            var parameter = (object)null;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(value)),
                () => result.ActualValue.ShouldBe(value),
                () => result.Message.ShouldContain(minimum.ToString()),
                () => result.Message.ShouldContain(maximum.ToString()));
        }

        [TestCaseSource(nameof(TestCases_ValidInt32ToStringConversions))]
        public void ConvertBack_ValidInt32ToString_ReturnsExpected(int value, CultureInfo culture, string expectedResult, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(string);
            var parameter = (object)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToInt32Conversions))]
        public void ConvertBack_InvalidStringToInt32_ThrowsException(object value, CultureInfo culture, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(int);
            var parameter = (object)null;

            var result = Should.Throw<Exception>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAnyCondition(
                () => result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeOfType<ArgumentException>(),
                    () => (result as ArgumentException)?.ParamName.ShouldBe(nameof(value)),
                    () => (result as ArgumentException)?.Message.ShouldContain(value?.ToString() ?? "null")),
                () => result.ShouldSatisfyAllConditions(
                    () => result.ShouldBeOfType<ArgumentOutOfRangeException>(),
                    () => (result as ArgumentOutOfRangeException)?.ParamName.ShouldBe(nameof(value)),
                    () => (result as ArgumentOutOfRangeException)?.ActualValue.ShouldBe(value),
                    () => (result as ArgumentOutOfRangeException)?.Message.ShouldContain(minimum.ToString()),
                    () => (result as ArgumentOutOfRangeException)?.Message.ShouldContain(maximum.ToString())));
        }

        [TestCaseSource(nameof(TestCases_ValidStringToInt32Conversions))]
        public void ConvertBack_ValidStringToInt32_ReturnsExpected(object value, CultureInfo culture, int expectedResult, int minimum, int maximum)
        {
            var uut = new Int32ToStringValueConverterAndValidationRule()
            {
                Minimum = minimum,
                Maximum = maximum
            };

            var targetType = typeof(int);
            var parameter = (object)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        #endregion ConvertBack Tests
    }
}

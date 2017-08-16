using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.ValueConverters;

namespace JV.Utilities.Wpf.Tests.ValueConverters
{
    [TestFixture]
    public class BooleanToStringValueConverterAndValidationRuleTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_InvalidConversions =
        {
            new object[] { null,         typeof(string) },
            new object[] { 0,            typeof(string) },
            new object[] { true,         typeof(int) },
            new object[] { string.Empty, typeof(int) },
            new object[] { string.Empty, typeof(string) }
        };

        private static readonly object[] TestCases_ValidBooleanToStringConversions =
        {
            new object[] { true,  "WhenTrue",  "WhenTrue", "WhenFalse" },
            new object[] { false, "WhenFalse", "WhenTrue", "WhenFalse" },
            new object[] { true,  "Yes",       "Yes",      "No" },
            new object[] { false, "No",        "Yes",      "No" }
        };

        private static readonly object[] TestCases_ValidBooleanToStringValidations =
            TestCases_ValidBooleanToStringConversions
                .Select(x => (object[])x)
                .Select(x => new[] { x[0], x[2], x[3] })
                .ToArray();

        private static readonly object[] TestCases_ValidStringToBooleanConversions =
        {
            new object[] { "WhenTrue",  true,  "WhenTrue", "WhenFalse" },
            new object[] { "whenTrue",  true,  "WhenTrue", "WhenFalse" },
            new object[] { "WhenFalse", false, "WhenTrue", "WhenFalse" },
            new object[] { "whenFalse", false, "WhenTrue", "WhenFalse" },
            new object[] { "Yes",       true,  "Yes",      "No" },
            new object[] { "No",        false, "Yes",      "No" },
            new object[] { 1,           true,  "1",        "0" },
            new object[] { 0,           false, "1",        "0" }
        };

        private static readonly object[] TestCases_ValidStringToBooleanValidations =
            TestCases_ValidStringToBooleanConversions
                .Select(x => (object[])x)
                .Select(x => new[] { x[0], x[2], x[3] })
                .ToArray();

        private static readonly object[] TestCases_InvalidStringToBooleanConversions =
        {
            new object[] { null,       "WhenTrue", "WhenFalse" },
            new object[] { 1,          "WhenTrue", "WhenFalse" },
            new object[] { "",         "WhenTrue", "WhenFalse" },
            new object[] { "Yes",      "WhenTrue", "WhenFalse" },
            new object[] { "WhenTrue", "Yes",        "No" }
        };

        private static readonly object[] TestCases_InvalidStringToBooleanValidations =
            TestCases_InvalidStringToBooleanConversions;

        #endregion Test Data

        /**********************************************************************/
        #region WhenTrue Tests

        [TestCase("WhenTrue")]
        public void WhenTrue_Always_SetsToGiven(string whenTrue)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule();

            uut.WhenTrue = whenTrue;

            uut.WhenTrue.ShouldBe(whenTrue);
        }

        #endregion WhenTrue Tests

        /**********************************************************************/
        #region WhenFalse Tests

        [TestCase("WhenFalse")]
        public void WhenFalse_Always_SetsToGiven(string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule();

            uut.WhenFalse = whenFalse;

            uut.WhenFalse.ShouldBe(whenFalse);
        }

        #endregion WhenFalse Tests

        /**********************************************************************/
        #region Validate Tests

        [TestCaseSource(nameof(TestCases_ValidBooleanToStringValidations))]
        [TestCaseSource(nameof(TestCases_ValidStringToBooleanValidations))]
        public void Validate_ValueIsValid_ReturnsIsValidTrue(object value, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            uut.Validate(value, null).IsValid.ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToBooleanValidations))]
        public void Validate_ValueIsNotValid_ReturnsIsValidFalse(object value, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            uut.Validate(value, null).IsValid.ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToBooleanValidations))]
        public void Validate_ValueIsNotValid_ReturnsErrorContentAsExpected(object value, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var result = uut.Validate(value, null).ErrorContent;

            result.ShouldSatisfyAllConditions(
                () => result.ShouldBeOfType<string>(),
                () => (result as string)?.ShouldContain(whenTrue),
                () => (result as string)?.ShouldContain(whenFalse));
        }

        #endregion Validate Tests

        /**********************************************************************/
        #region Convert Tests

        [Test]
        public void Convert_TargetTypeIsNull_ThrowsException()
        {
            var uut = new BooleanToStringValueConverterAndValidationRule();

            var value = false;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ParamName.ShouldBe(nameof(targetType));
        }

        [TestCaseSource(nameof(TestCases_InvalidConversions))]
        public void Convert_ConversionIsInvalid_ThrowsException(object value, Type targetType)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule();

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

        [TestCaseSource(nameof(TestCases_ValidBooleanToStringConversions))]
        public void Convert_BooleanToString_ReturnsExpected(bool value, string expectedResult, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(string);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToBooleanConversions))]
        public void Convert_InvalidStringToBoolean_ThrowsException(object value, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(bool);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(value)),
                () => result.Message.ShouldContain(whenTrue),
                () => result.Message.ShouldContain(whenFalse));
        }

        [TestCaseSource(nameof(TestCases_ValidStringToBooleanConversions))]
        public void Convert_ValidStringToBoolean_ReturnsExpected(object value, bool expectedResult, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(bool);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            uut.Convert(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        #endregion Convert Tests

        /**********************************************************************/
        #region ConvertBack Tests

        [Test]
        public void ConvertBack_TargetTypeIsNull_ThrowsException()
        {
            var uut = new BooleanToStringValueConverterAndValidationRule();

            var value = false;
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
            var uut = new BooleanToStringValueConverterAndValidationRule();

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

        [TestCaseSource(nameof(TestCases_ValidBooleanToStringConversions))]
        public void ConvertBack_BooleanToString_ReturnsExpected(bool value, string expectedResult, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(string);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            uut.ConvertBack(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_InvalidStringToBooleanConversions))]
        public void ConvertBack_InvalidStringToBoolean_ThrowsException(object value, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(bool);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(value)),
                () => result.Message.ShouldContain(whenTrue),
                () => result.Message.ShouldContain(whenFalse));
        }

        [TestCaseSource(nameof(TestCases_ValidStringToBooleanConversions))]
        public void ConvertBack_ValidStringToBoolean_ReturnsExpected(object value, bool expectedResult, string whenTrue, string whenFalse)
        {
            var uut = new BooleanToStringValueConverterAndValidationRule()
            {
                WhenTrue = whenTrue,
                WhenFalse = whenFalse
            };

            var targetType = typeof(bool);
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            uut.ConvertBack(value, targetType, parameter, culture).ShouldBe(expectedResult);
        }

        #endregion ConvertBack Tests
    }
}

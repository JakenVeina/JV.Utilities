using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.ValueConverters;

namespace JV.Utilities.Wpf.Tests.ValueConverters
{
    [TestFixture]
    public class BooleanToVisibilityValueConverterTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_InvalidConversions =
        {
            new object[] { null,               typeof(bool) },
            new object[] { null,               typeof(Visibility) },
            new object[] { 0,                  typeof(bool) },
            new object[] { 0,                  typeof(Visibility) },
            new object[] { true,               typeof(int) },
            new object[] { Visibility.Visible, typeof(int) },
            new object[] { true,               typeof(bool) },
            new object[] { Visibility.Visible, typeof(Visibility) }
        };

        private static readonly object[] TestCases_ValidBooleanToVisibilityConversions =
        {
            new object[] { true,  Visibility.Visible,   Visibility.Hidden },
            new object[] { false, Visibility.Hidden,    Visibility.Hidden },
            new object[] { false, Visibility.Collapsed, Visibility.Collapsed }
        };

        private static readonly object[] TestCases_ValidVisibilityToBooleanConversions =
        {
            new object[] { Visibility.Visible,   true },
            new object[] { Visibility.Hidden,    false },
            new object[] { Visibility.Collapsed, false }
        };

        #endregion Test Data

        /**********************************************************************/
        #region WhenFalse Tests

        [TestCase(Visibility.Visible)]
        [TestCase(Visibility.Hidden)]
        [TestCase(Visibility.Collapsed)]
        public void WhenFalse_Always_SetsToGiven(Visibility whenFalse)
        {
            var uut = new BooleanToVisibilityValueConverter();

            uut.WhenFalse = whenFalse;

            uut.WhenFalse.ShouldBe(whenFalse);
        }

        #endregion WhenFalse Tests

        /**********************************************************************/
        #region Convert Tests

        [Test]
        public void Convert_TargetTypeIsNull_ThrowsException()
        {
            var value = false;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ParamName.ShouldBe(nameof(targetType));
        }

        [TestCaseSource(nameof(TestCases_InvalidConversions))]
        public void Convert_ConversionIsInvalid_ThrowsException(object value, Type targetType)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(value?.GetType().Name ?? "null"),
                () => result.Message.ShouldContain(targetType.Name));
        }

        [TestCaseSource(nameof(TestCases_ValidBooleanToVisibilityConversions))]
        public void Convert_BooleanToVisibility_ReturnsExpected(bool value, Visibility expectedResult, Visibility whenFalse)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter() { WhenFalse = whenFalse };

            uut.Convert(value, typeof(Visibility), parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_ValidVisibilityToBooleanConversions))]
        public void Convert_VisibilityToBoolean_ReturnsExpected(Visibility value, bool expectedResult)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            uut.Convert(value, typeof(bool), parameter, culture).ShouldBe(expectedResult);
        }

        #endregion Convert Tests

        /**********************************************************************/
        #region ConvertBack Tests

        [Test]
        public void ConvertBack_TargetTypeIsNull_ThrowsException()
        {
            var value = false;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ParamName.ShouldBe(nameof(targetType));
        }

        [TestCaseSource(nameof(TestCases_InvalidConversions))]
        public void ConvertBack_ConversionIsInvalid_ThrowsException(object value, Type targetType)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(value?.GetType().Name ?? "null"),
                () => result.Message.ShouldContain(targetType.Name));
        }

        [TestCaseSource(nameof(TestCases_ValidBooleanToVisibilityConversions))]
        public void ConvertBack_BooleanToVisibility_ReturnsExpected(bool value, Visibility expectedResult, Visibility whenFalse)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter() { WhenFalse = whenFalse };

            uut.ConvertBack(value, typeof(Visibility), parameter, culture).ShouldBe(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_ValidVisibilityToBooleanConversions))]
        public void ConvertBack_VisibilityToBoolean_ReturnsExpected(Visibility value, bool expectedResult)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanToVisibilityValueConverter();

            uut.ConvertBack(value, typeof(bool), parameter, culture).ShouldBe(expectedResult);
        }

        #endregion ConvertBack Tests
    }
}

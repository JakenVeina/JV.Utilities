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
    public class BooleanInversionValueConverterTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_InvalidConversions =
        {
            new object[] { null,               typeof(bool) },
            new object[] { 0,                  typeof(bool) },
            new object[] { true,               typeof(int) },
        };

        private static readonly object[] TestCases_ValidConversions =
        {
            new object[] { true,  false },
            new object[] { false, true },
        };

        #endregion Test Data

        /**********************************************************************/
        #region Convert Tests

        [Test]
        public void Convert_TargetTypeIsNull_ThrowsException()
        {
            var value = false;
            var targetType = (Type)null;
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanInversionValueConverter();

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

            var uut = new BooleanInversionValueConverter();

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.Convert(value, targetType, parameter, culture);
            });

            result.Message.ShouldContain(value?.GetType().Name ?? "null");
            result.Message.ShouldContain(targetType.Name);
        }

        [TestCaseSource(nameof(TestCases_ValidConversions))]
        public void Convert_Otherwise_ReturnsExpected(bool value, bool expectedResult)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanInversionValueConverter();

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

            var uut = new BooleanInversionValueConverter();

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

            var uut = new BooleanInversionValueConverter();

            var result = Should.Throw<NotSupportedException>(() =>
            {
                uut.ConvertBack(value, targetType, parameter, culture);
            });

            result.ShouldSatisfyAllConditions(
                () => result.Message.ShouldContain(value?.GetType().Name ?? "null"),
                () => result.Message.ShouldContain(targetType.Name));
        }

        [TestCaseSource(nameof(TestCases_ValidConversions))]
        public void ConvertBack_Otherwise_ReturnsExpected(bool value, bool expectedResult)
        {
            var parameter = (object)null;
            var culture = (CultureInfo)null;

            var uut = new BooleanInversionValueConverter();

            uut.ConvertBack(value, typeof(bool), parameter, culture).ShouldBe(expectedResult);
        }

        #endregion ConvertBack Tests
    }
}

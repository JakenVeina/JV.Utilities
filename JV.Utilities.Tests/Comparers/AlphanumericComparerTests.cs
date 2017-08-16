using System;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Comparers;

namespace JV.Utilities.Tests.Comparers
{
    [TestFixture]
    public class AlphanumericComparerTests
    {
        /**********************************************************************/
        #region Test Cases

        private static readonly string[][] Compare_TestCases_XIsLessThanY =
        {
            new string[] { null, "test" },
            new string[] { "", "test" },
            new string[] { null, "" },
            new string[] { "This is a test", "This is only a test" },
            new string[] { "9", "10" },
            new string[] { "10", "100" },
            new string[] { "A9", "A10" },
            new string[] { "A10", "A100" },
            new string[] { "A1", "AB1" },
            new string[] { "9A", "10A" },
            new string[] { "10A", "100A" },
            new string[] { "1A", "1AB" },
            new string[] { "A1A2A3", "A1A2A10" },
            new string[] { "A1A2", "A1A2A3" }
        };

        private static readonly string[][] Compare_TestCases_XIsGreaterThanY =
            Compare_TestCases_XIsLessThanY.Select(x => x.Reverse().ToArray()).ToArray();

        #endregion Test Cases

        /**********************************************************************/
        #region Default Tests

        [Test]
        public void Default_Always_IsNotNull()
        {
            var uut = AlphanumericComparer.Default;

            uut.ShouldNotBeNull();
        }

        [TestCase("A1A2A3", "a1a2a3", 1)]
        public void Default_Always_IsCaseSensitive(string x, string y, int expected)
        {
            var uut = AlphanumericComparer.Default;

            uut.Compare(x, y).ShouldBe(expected);
        }
        
        #endregion Default Tests

        /**********************************************************************/
        #region Compare Tests

        [TestCaseSource(nameof(Compare_TestCases_XIsLessThanY))]
        public void Compare_Generic_XIsLessThanY_ReturnsNegative1(string x, string y)
        {
            var uut = new AlphanumericComparer(StringComparison.Ordinal);

            uut.Compare(x, y).ShouldBe(-1);
        }

        [TestCaseSource(nameof(Compare_TestCases_XIsGreaterThanY))]
        public void Compare_Generic_XIsGreaterThanY_Returns1(string x, string y)
        {
            var uut = new AlphanumericComparer(StringComparison.Ordinal);

            uut.Compare(x, y).ShouldBe(1);
        }

        [TestCase(null, null)]
        [TestCase("test", "test")]
        [TestCase("10", "10")]
        [TestCase("A10", "A10")]
        [TestCase("10A", "10A")]
        public void Compare_Generic_XIsEqualToY_Returns0(string x, string y)
        {
            var uut = new AlphanumericComparer(StringComparison.Ordinal);

            uut.Compare(x, y).ShouldBe(0);
        }

        [TestCase("A1A2A3", "a1a2a3", 0)]
        [TestCase("A1", "a1a2", -1)]
        [TestCase("a1a2", "A1", 1)]
        public void Compare_Generic_ComparisonTypeIsIgnoreCase_IsCaseInsensitive(string x, string y, int expectedResult)
        {
            var uut = new AlphanumericComparer(StringComparison.OrdinalIgnoreCase);

            uut.Compare(x, y).ShouldBe(expectedResult);
        }

        [TestCase("A9", "A10")]
        [TestCase("A9", "A9")]
        [TestCase("A10", "A9")]
        public void Compare_NonGeneric_ForStrings_MatchesGeneric(object x, object y)
        {
            var uut = new AlphanumericComparer(StringComparison.Ordinal);
            var expectedResult = uut.Compare(x, y);

            uut.Compare(x, y).ShouldBe(expectedResult);
        }

        [TestCase(1, 10)]
        [TestCase(1, 1)]
        [TestCase(10, 1)]
        public void Compare_NonGeneric_ForNonStrings_UsesToString(object x, object y)
        {
            var uut = new AlphanumericComparer(StringComparison.Ordinal);
            var expectedResult = uut.Compare(x.ToString(), y.ToString());

            uut.Compare(x, y).ShouldBe(expectedResult);
        }

        #endregion Compare Tests
    }
}

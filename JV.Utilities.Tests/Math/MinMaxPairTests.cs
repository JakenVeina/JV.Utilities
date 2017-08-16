using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Comparers;
using JV.Utilities.Math;

namespace JV.Utilities.Tests.Math
{
    [TestFixture]
    public class MinMaxPairTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_Default =
        {
            new object[] { 2, 4 },
            new object[] { 3, 3 }
        };

        private static readonly object[] TestCases_PairsEqual =
        {
            new object[] { 2, 4,
                           2, 4 },
            new object[] { 3, 3,
                           3, 3 }
        };

        private static readonly object[] TestCases_PairsNotEqual =
        {
            new object[] { 2, 4,
                           3, 4 },
            new object[] { 2, 4,
                           2, 3 },
        };

        private static readonly object[] TestCases_FirstPairIsLessThanSecondPair =
        {
            new object[] { 2, 5,
                           3, 4 },
            new object[] { 2, 4,
                           2, 5 },
        };

        private static readonly object[] TestCases_FirstPairIsGreaterThanSecondPair =
        {
            new object[] { 3, 4,
                           2, 5 },
            new object[] { 2, 5,
                           2, 4 },
        };

        #endregion Test Data

        /**********************************************************************/
        #region Constructor(min, max) Tests

        [TestCase(4, 2)]
        public void Constructor_MinMax_MaxIsLessThanMin_ThrowsException(int min, int max)
        {
            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                new MinMaxPair<int>(min, max);
            });

            result.ParamName.ShouldBe(nameof(max));
            result.ActualValue.ShouldBe(max);
        }

        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void Constructor_MinMax_MinIsLessThanOrEqualToMax_SetsMin(int min, int max)
        {
            new MinMaxPair<int>(min, max).Min.ShouldBe(min);
        }

        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void Constructor_MinMax_MinIsLessThanOrEqualToMax_SetsMax(int min, int max)
        {
            new MinMaxPair<int>(min, max).Max.ShouldBe(max);
        }

        [TestCase(2, 4, true)]
        [TestCase(3, 3, false)]
        public void Constructor_MinMax_MinIsLessThanOrEqualToMax_SetsIsRangeAsExpected(int min, int max, bool expectedIsRange)
        {
            new MinMaxPair<int>(min, max).IsRange.ShouldBe(expectedIsRange);
        }

        [TestCase(2, 4, true)]
        [TestCase(3, 3, false)]
        public void Constructor_MinMax_ComparerIsNull_UsesDefault(int min, int max, bool expectedIsRange)
        {
            new MinMaxPair<int>(min, max, null).IsRange.ShouldBe(expectedIsRange);
        }

        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void Constructor_MinMax_ComparerAlwaysReturns1_ThrowsException(int min, int max)
        {
            var comparer = new DelegateComparer<int>((x, y) => 1);

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                new MinMaxPair<int>(min, max, comparer);
            });

            result.ParamName.ShouldBe(nameof(max));
            result.ActualValue.ShouldBe(max);
        }

        #endregion Constructor(min, max) Tests

        /**********************************************************************/
        #region Constructor(value) Tests

        [TestCase(1)]
        public void Constructor_Value_Always_SetsMinToValue(int value)
        {
            new MinMaxPair<int>(value).Min.ShouldBe(value);
        }

        [TestCase(1)]
        public void Constructor_Value_Always_SetsMaxToValue(int value)
        {
            new MinMaxPair<int>(value).Max.ShouldBe(value);
        }

        [TestCase(1)]
        public void Constructor_Value_Always_SetsIsRangeToFalse(int value)
        {
            new MinMaxPair<int>(value).IsRange.ShouldBeFalse();
        }

        [TestCase(1)]
        public void Constructor_Value_ComparerIsNull_UsesDefault(int value)
        {
            var uut1 = new MinMaxPair<int>(value);
            var uut2 = new MinMaxPair<int>(value + 1);

            uut1.CompareTo(uut2).ShouldBe(-1);
        }

        #endregion Constructor(value) Tests

        /**********************************************************************/
        #region Contains Tests

        [TestCase(2, 4, 1)]
        [TestCase(3, 3, 1)]
        [TestCase(3, 3, 2)]
        public void Contains_ValueIsLessThanMin_ReturnsFalse(int min, int max, int value)
        {
            new MinMaxPair<int>(min, max).Contains(value).ShouldBeFalse();
        }

        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void Contains_ValueIsEqualToMin_ReturnsTrue(int min, int max)
        {
            new MinMaxPair<int>(min, max).Contains(min).ShouldBeTrue();
        }

        [TestCase(2, 4, 3)]
        public void Contains_ValueIsGreaterThanMinAndLessThanMax_ReturnsTrue(int min, int max, int value)
        {
            new MinMaxPair<int>(min, max).Contains(value).ShouldBeTrue();
        }

        [TestCase(2, 4)]
        [TestCase(3, 3)]
        public void Contains_ValueIsEqualToMax_ReturnsTrue(int min, int max)
        {
            new MinMaxPair<int>(min, max).Contains(max).ShouldBeTrue();
        }

        [TestCase(2, 4, 5)]
        [TestCase(3, 3, 4)]
        [TestCase(3, 3, 5)]
        public void Contains_ValueIsGreaterThanMax_ReturnsFalse(int min, int max, int value)
        {
            new MinMaxPair<int>(min, max).Contains(value).ShouldBeFalse();
        }

        [TestCase(2, 4, 1)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 4, 3)]
        [TestCase(2, 4, 4)]
        [TestCase(2, 4, 5)]
        public void Contains_ComparerAlwaysReturnsEqual_ReturnsTrue(int min, int max, int value)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut = new MinMaxPair<int>(min, max, comparer);

            uut.Contains(value).ShouldBeTrue();
        }

        [TestCase(2, 4, 1)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 4, 3)]
        [TestCase(2, 4, 4)]
        [TestCase(2, 4, 5)]
        public void Contains_ComparerAlwaysReturnsLessThan_ReturnsFalse(int min, int max, int value)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut = new MinMaxPair<int>(min, max, comparer);

            uut.Contains(value).ShouldBeFalse();
        }

        #endregion Contains Tests

        /**********************************************************************/
        #region Equals Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void Equals_Generic_ThisIsSameAsPair_ReturnsTrue(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.Equals(uut).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void Equals_Generic_ThisIsEqualToPair_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.Equals(uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void Equals_Generic_ThisIsNotEqualToPair_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.Equals(uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void Equals_Generic_ComparerAlwaysReturns0_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.Equals(uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void Equals_Generic_ComparerAlwaysReturnsNegative1_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.Equals(uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void Equals_NonGeneric_ThisIsSameAsObj_ReturnsTrue(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.Equals((object)uut).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void Equals_NonGeneric_ThisIsEqualToObj_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.Equals((object)uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void Equals_NonGeneric_ThisIsNotEqualToObj_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.Equals((object)uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void Equals_NonGeneric_ComparerAlwaysReturns0_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.Equals((object)uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void Equals_NonGeneric_ComparerAlwaysReturnsNegative1_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.Equals((object)uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void Equals_NonGeneric_ObjIsNull_ReturnsFalse(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.Equals(null).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void Equals_ObjTypeIsWrong_ReturnsFalse(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.Equals(new object()).ShouldBeFalse();
        }

        #endregion Equals Tests

        /**********************************************************************/
        #region GetHashCode Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void GetHashCode_ForSamePAir_ResultsAreEqual(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.GetHashCode().ShouldBe(uut.GetHashCode());
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void GetHashCode_ForPairsEqual_ResultsAreEqual(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.GetHashCode().ShouldBe(uut1.GetHashCode());
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void GetHashCode_ForPairsNotEqual_ResultsAreNotEqual(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.GetHashCode().ShouldNotBe(uut2.GetHashCode());
        }

        [Test]
        public void GetHashCode_ForRandomData_NoCollisions()
        {
            var rng = new Random(0);

            var uutsAndHashCodes = Enumerable.Range(0, 1000).Select(i => rng.Next(50)).Select(min => new MinMaxPair<int>(min, rng.Next(min, 50)))
                                                            .Select(uut => new object[] { uut, uut.GetHashCode() });

            var collisionCount = 1000 - uutsAndHashCodes.Distinct(new DelegateEqualityComparer<object[]>(
                    (x, y) => (!x[0].Equals(y[0]) && x[1].Equals(y[1])),
                    (x) => x.GetHashCode()
                )).Count();

            collisionCount.ShouldBe(0);
        }

        #endregion GetHashCode Tests

        /**********************************************************************/
        #region CompareTo Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void CompareTo_Generic_ThisIsSameAsPair_Returns0(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.CompareTo(uut).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void CompareTo_Generic_ThisIsEqualToPair_Returns0(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo(uut2).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_FirstPairIsLessThanSecondPair))]
        public void CompareTo_Generic_ThisIsLessThanPair_ReturnsNegative1(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo(uut2).ShouldBe(-1);
        }

        [TestCaseSource(nameof(TestCases_FirstPairIsGreaterThanSecondPair))]
        public void CompareTo_Generic_ThisIsGreaterThanPair_Returns1(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo(uut2).ShouldBe(1);
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void CompareTo_Generic_ComparerAlwaysReturns0_Returns0(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.CompareTo(uut2).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void CompareTo_Generic_ComparerAlwaysReturnsNegative1_ReturnsNegative1(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.CompareTo(uut2).ShouldBe(-1);
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void CompareTo_NonGeneric_ThisIsSameAsObj_Returns0(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            uut.CompareTo((object)uut).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void CompareTo_NonGeneric_ThisIsEqualToObj_Returns0(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo((object)uut2).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_FirstPairIsLessThanSecondPair))]
        public void CompareTo_NonGeneric_ThisIsLessThanObj_ReturnsNegative1(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo((object)uut2).ShouldBe(-1);
        }

        [TestCaseSource(nameof(TestCases_FirstPairIsGreaterThanSecondPair))]
        public void CompareTo_NonGeneric_ThisIsGreaterThanObj_Returns1(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            uut1.CompareTo((object)uut2).ShouldBe(1);
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void CompareTo_NonGeneric_ComparerAlwaysReturns0_Returns0(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.CompareTo((object)uut2).ShouldBe(0);
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void Equals_NonGeneric_ComparerAlwaysReturnsNegative1_ReturnsNegative1(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            uut1.CompareTo((object)uut2).ShouldBe(-1);
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void CompareTo_NonGeneric_ObjIsNull_Returns1(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);
            var obj = (object)null;

            uut.CompareTo(obj).ShouldBe(1);
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void CompareTo_ObjTypeIsWrong_ThrowsException(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);
            var obj = new object();

            var result = Should.Throw<ArgumentException>(() =>
            {
                uut.CompareTo(obj);
            });

            result.ParamName.ShouldBe(nameof(obj));
        }

        #endregion CompareTo Tests

        /**********************************************************************/
        #region Operator Equals Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void OperatorEquals_XIsSameAsY_ReturnsTrue(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            #pragma warning disable CS1718 // Comparison made to same variable
            (uut == uut).ShouldBeTrue();
            #pragma warning restore CS1718 // Comparison made to same variable
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void OperatorEquals_XIsEqualToY_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            (uut1 == uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void OperatorEquals_XIsNotEqualToY_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            (uut1 == uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void OperatorEquals_ComparerAlwaysReturns0_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            (uut1 == uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void OperatorEquals_ComparerAlwaysReturnsNegative1_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            (uut1 == uut2).ShouldBeFalse();
        }

        #endregion Operator Equals Tests

        /**********************************************************************/
        #region Operator Not Equals Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void OperatorNotEquals_XIsSameAsY_ReturnsFalse(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            #pragma warning disable CS1718 // Comparison made to same variable
            (uut != uut).ShouldBeFalse();
            #pragma warning restore CS1718 // Comparison made to same variable
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void OperatorNotEquals_XIsEqualToY_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            (uut1 != uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void OperatorNotEquals_XIsNotEqualToY_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var uut1 = new MinMaxPair<int>(xMin, xMax);
            var uut2 = new MinMaxPair<int>(yMin, yMax);

            (uut1 != uut2).ShouldBeTrue();
        }

        [TestCaseSource(nameof(TestCases_PairsNotEqual))]
        public void OperatorNotEquals_ComparerAlwaysReturns0_ReturnsFalse(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            (uut1 != uut2).ShouldBeFalse();
        }

        [TestCaseSource(nameof(TestCases_PairsEqual))]
        public void OperatorNotEquals_ComparerAlwaysReturnsNegative1_ReturnsTrue(int xMin, int xMax, int yMin, int yMax)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);
            var uut1 = new MinMaxPair<int>(xMin, xMax, comparer);
            var uut2 = new MinMaxPair<int>(yMin, yMax, comparer);

            (uut1 != uut2).ShouldBeTrue();
        }

        #endregion Operator Not Equals Tests

        /**********************************************************************/
        #region Operator Cast From Value Tests

        [TestCase(1)]
        public void OperatorCastFromT_Always_MinEqualsValue(int value)
        {
            ((MinMaxPair<int>)value).Min.ShouldBe(value);
        }

        [TestCase(1)]
        public void OperatorCastFromT_Always_MaxEqualsValue(int value)
        {
            ((MinMaxPair<int>)value).Max.ShouldBe(value);
        }

        [TestCase(1)]
        public void OperatorCastFromT_Always_SetsIsRangeFalse(int value)
        {
            ((MinMaxPair<int>)value).IsRange.ShouldBeFalse();
        }

        #endregion Operator Cast From Value Tests

        /**********************************************************************/
        #region ToString Tests

        [TestCaseSource(nameof(TestCases_Default))]
        public void ToString_Always_ContainsMinToStringAndMaxToString(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);

            var result = uut.ToString();

            result.ShouldSatisfyAllConditions(
                () => result.ShouldContain(min.ToString()),
                () => result.ShouldContain(max.ToString()));
        }

        #endregion ToString Tests

        /**********************************************************************/
        #region DataContract Tests

        [Test]
        public void DataContract_ByDefinition_DataContractIsDefined()
        {
            var uut = typeof(MinMaxPair<int>);

            uut.GetCustomAttributes(typeof(DataContractAttribute), false).ShouldNotBeEmpty();
        }

        [Test]
        public void DataContract_ByDefinition_DataContractIsReferenceIsFalse()
        {
            var uut = typeof(MinMaxPair<int>);

            (uut.GetCustomAttributes(typeof(DataContractAttribute), false).First() as DataContractAttribute).IsReference.ShouldBeFalse();
        }

        [TestCase(nameof(MinMaxPair<int>.Min))]
        [TestCase(nameof(MinMaxPair<int>.Max))]
        public void DataContract_ByDefinition_DataMemberIsDefinedForProperty(string propertyName)
        {
            var uut = typeof(MinMaxPair<int>).GetProperty(propertyName);

            uut.GetCustomAttributes(typeof(DataMemberAttribute), false).ShouldNotBeEmpty();
        }

        [Test]
        public void DataContract_ByDefinition_OnDeserializedIsDefined()
        {
            var uut = typeof(MinMaxPair<int>).GetMethod(nameof(MinMaxPair<int>.OnDeserialized), BindingFlags.Instance | BindingFlags.NonPublic);

            uut.GetCustomAttributes(typeof(OnDeserializedAttribute), false).ShouldNotBeEmpty();
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void DataContract_Always_OnDeserializedSetsIsRangeToExpected(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);
            var expected = uut.IsRange;

            uut.OnDeserialized(new StreamingContext());

            uut.IsRange.ShouldBe(expected);
        }

        [TestCaseSource(nameof(TestCases_Default))]
        public void DataContract_Always_OnDeserializedSetsGetHashCodeToExpected(int min, int max)
        {
            var uut = new MinMaxPair<int>(min, max);
            var expected = uut.GetHashCode();

            uut.OnDeserialized(new StreamingContext());

            uut.GetHashCode().ShouldBe(expected);
        }

        #endregion DataContract Tests
    }
}

using System;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Comparers;
using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class MathExtensionsTests
    {
        /**********************************************************************/
        #region IsInRange Tests

        [Test]
        public void IsInRange_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var min = "1";
            var max = "2";

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IsInRange(min, max);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(0, -1)]
        [TestCase(1, 0)]
        [TestCase(2, 1)]
        public void IsInRange_MaxIsLessThanMin_ThrowsException(int min, int max)
        {
            var @this = 0;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.IsInRange(min, max);
            });

            result.ParamName.ShouldBe(nameof(max));
            result.ActualValue.ShouldBe(max);
        }

        [TestCase(-1, 0, 1)]
        [TestCase(0,  1, 2)]
        [TestCase(1,  2, 3)]
        public void IsInRange_ThisIsLessThanMin_ReturnsFalse(int @this, int min, int max)
        {
            @this.IsInRange(min, max).ShouldBeFalse();
        }

        [TestCase(-1, -1, 0)]
        [TestCase( 0,  0, 1)]
        [TestCase( 1,  1, 2)]
        public void IsInRange_ThisIsEqualToMin_ReturnsTrue(int @this, int min, int max)
        {
            @this.IsInRange(min, max).ShouldBeTrue();
        }

        [TestCase(-1, -2, 0)]
        [TestCase( 0, -1, 1)]
        [TestCase( 1,  0, 2)]
        [TestCase( 2,  1, 5)]
        [TestCase( 3,  1, 5)]
        [TestCase( 4,  1, 5)]
        public void IsInRange_ThisIsGreaterThanMinAndLessThanMax_ReturnsTrue(int @this, int min, int max)
        {
            @this.IsInRange(min, max).ShouldBeTrue();
        }

        [TestCase(-1, -2, -1)]
        [TestCase( 0, -1,  0)]
        [TestCase( 1,  0,  1)]
        public void IsInRange_ThisIsEqualToMax_ReturnsTrue(int @this, int min, int max)
        {
            @this.IsInRange(min, max).ShouldBeTrue();
        }

        [TestCase(-1, -3, -2)]
        [TestCase( 0, -2, -1)]
        [TestCase( 1, -1,  0)]
        public void IsInRange_ThisIsGreaterThanMax_ReturnsFalse(int @this, int min, int max)
        {
            @this.IsInRange(min, max).ShouldBeFalse();
        }

        [TestCase(1, 2, 4)]
        [TestCase(2, 2, 4)]
        [TestCase(3, 2, 4)]
        [TestCase(4, 2, 4)]
        [TestCase(5, 2, 4)]
        public void IsInRange_ComparerAlwaysReturnsEqual_ReturnsTrue(int @this, int min, int max)
        {
            var comparer = new DelegateComparer<int>((x, y) => 0);

            @this.IsInRange(min, max, comparer).ShouldBeTrue();
        }

        [TestCase(1, 2, 4)]
        [TestCase(2, 2, 4)]
        [TestCase(3, 2, 4)]
        [TestCase(4, 2, 4)]
        [TestCase(5, 2, 4)]
        public void IsInRange_ComparerAlwaysReturnsLessThan_ReturnsFalse(int @this, int min, int max)
        {
            var comparer = new DelegateComparer<int>((x, y) => -1);

            @this.IsInRange(min, max, comparer).ShouldBeFalse();
        }

        [TestCase(-2, -1)]
        [TestCase(-1, 0)]
        [TestCase(0, 1)]
        public void IsInRange_ComparerAlwaysReturnsGreaterThan_ThrowsException(int min, int max)
        {
            var comparer = new DelegateComparer<int>((x, y) => 1);

            var @this = 0;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.IsInRange(min, max, comparer);
            });

            result.ParamName.ShouldBe(nameof(max));
            result.ActualValue.ShouldBe(max);
        }

        #endregion IsInRange Tests
    }
}

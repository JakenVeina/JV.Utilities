using System;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class ArrayExtensionsTests
    {
        /**********************************************************************/
        #region IndexRange Tests

        [Test]
        public void IndexRange_ThisIsNull_ThrowsException()
        {
            var @this = null as int[];

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void IndexRange_Otherwise_ResultEachIsExpected(int thisLength)
        {
            var @this = new int[thisLength];

            @this.IndexRange().ShouldBeOrderedEquivalentTo(Enumerable.Range(0, @this.Length));
        }

        #endregion IndexRange Tests

        /**********************************************************************/
        #region ReverseIndexRange Tests

        [Test]
        public void ReverseIndexRange_ThisIsNull_ThrowsException()
        {
            var @this = null as int[];

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ReverseIndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ReverseIndexRange_Otherwise_ResultEachIsExpected(int thisLength)
        {
            var @this = new int[thisLength];

            @this.ReverseIndexRange().ShouldBeOrderedEquivalentTo(Enumerable.Range(0, @this.Length).Reverse());
        }

        #endregion ReverseIndexRange Tests

        /**********************************************************************/
        #region SubArray Tests

        [Test]
        public void SubArray_ThisIsNull_ThrowsException()
        {
            var @this = (string[])null;
            var index = 0;
            var length = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.SubArray(index, length);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void SubArray_IndexIsNegative_ThrowsException()
        {
            var @this = new string[0];
            var index = -1;
            var length = 1;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.SubArray(index, length);
            });

            result.ParamName.ShouldBe(nameof(index));
            result.ActualValue.ShouldBe(index);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void SubArray_IndexIsTooLarge_ThrowsException(int index)
        {
            var @this = new string[index];
            var length = 1;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.SubArray(index, length);
            });

            result.ParamName.ShouldBe(nameof(index));
            result.ActualValue.ShouldBe(index);
        }

        [Test]
        public void SubArray_LengthIsNegative_ThrowsException()
        {
            var @this = new string[0];
            var index = 0;
            var length = -1;

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.SubArray(index, length);
            });

            result.ParamName.ShouldBe(nameof(length));
            result.ActualValue.ShouldBe(length);
        }

        [TestCase(1, 0, 2)]
        [TestCase(3, 0, 4)]
        [TestCase(3, 1, 3)]
        [TestCase(3, 2, 2)]
        public void SubArray_LengthIsTooLarge_ThrowsException(int thisLength, int index, int length)
        {
            var @this = new string[thisLength];

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.SubArray(index, length);
            });

            result.ParamName.ShouldBe(nameof(length));
            result.ActualValue.ShouldBe(length);
        }

        [TestCase(1, 0, 1)]
        [TestCase(3, 0, 3)]
        [TestCase(3, 1, 2)]
        [TestCase(3, 2, 1)]
        [TestCase(3, 2, 0)]
        [TestCase(10, 3, 4)]
        public void SubArray_Otherwise_ResultMatchesArraySlice(int thisLength, int index, int length)
        {
            var @this = Enumerable.Range(0, thisLength).ToArray();

            var expected = @this.Skip(index).Take(length);

            @this.SubArray(index, length).ShouldBeOrderedEquivalentTo(@this.Skip(index).Take(length));
        }

        #endregion SubArray Tests
    }
}

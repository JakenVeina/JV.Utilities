using System;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Comparers;

namespace JV.Utilities.Tests.Comparers
{
    [TestFixture]
    public class DelegateEqualityComparerTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T>
        {
            public TestContext()
            {
                equalsDelegate = Substitute.For<Func<T, T, bool>>();
                getHashCodeDelegate = Substitute.For<Func<T, int>>();
            }

            public Func<T, T, bool> equalsDelegate;
            public Func<T, int> getHashCodeDelegate;

            public DelegateEqualityComparer<T> ConstructUUT()
                => new DelegateEqualityComparer<T>(equalsDelegate, getHashCodeDelegate);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_EqualsDelegateIsNull_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                equalsDelegate = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT();
            });

            result.ParamName.ShouldBe(nameof(context.equalsDelegate));
        }

        [Test]
        public void Constructor_GetHashCodeDelegateIsNull_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                getHashCodeDelegate = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT();
            });

            result.ParamName.ShouldBe(nameof(context.getHashCodeDelegate));
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region Equals Tests

        [TestCase(1, 2, true)]
        public void Equals_Always_InvokesEqualsDelegate(int x, int y, bool expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.Equals(x, y);

            context.equalsDelegate.Received(1).Invoke(x, y);
        }

        [TestCase(1, 2, true)]
        public void Equals_Always_ReturnsEqualsDelegate(int x, int y, bool expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            context.equalsDelegate.Invoke(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedResult);

            uut.Equals(x, y).ShouldBe(expectedResult);
        }

        #endregion Equals Tests

        /**********************************************************************/
        #region GetHashCode Tests

        [TestCase(1, 2)]
        public void GetHashCode_Always_InvokesGetHashCodeDelegate(int x, int expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.GetHashCode(x);

            context.getHashCodeDelegate.Received(1).Invoke(x);
        }

        [TestCase(1, 2)]
        public void GetHashCode_Always_ReturnsGetHashCodeDelegate(int x, int expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            context.getHashCodeDelegate.Invoke(Arg.Any<int>()).Returns(expectedResult);

            uut.GetHashCode(x).ShouldBe(expectedResult);
        }

        #endregion GetHashCode Tests
    }
}

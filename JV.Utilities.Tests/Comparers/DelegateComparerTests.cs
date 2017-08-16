using System;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Comparers;

namespace JV.Utilities.Tests.Comparers
{
    [TestFixture]
    public class DelegateComparerTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T>
        {
            public TestContext()
            {
                compareDelegate = Substitute.For<Func<T, T, int>>();
            }

            public Func<T, T, int> compareDelegate;

            public DelegateComparer<T> ConstructUUT()
                => new DelegateComparer<T>(compareDelegate);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_CompareDelegateIsNull_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                compareDelegate = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT();
            });

            result.ParamName.ShouldBe(nameof(context.compareDelegate));
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region Compare Tests

        [TestCase(1, 2, 3)]
        public void Compare_Always_InvokesCompareDelegate(int x, int y, int expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.Compare(x, y);

            context.compareDelegate.Received(1).Invoke(x, y);
        }

        [TestCase(1, 2, 3)]
        public void Compare_Always_ReturnsCompareDelegate(int x, int y, int expectedResult)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            context.compareDelegate.Invoke(Arg.Any<int>(), Arg.Any<int>()).Returns(expectedResult);

            uut.Compare(x, y).ShouldBe(expectedResult);
        }

        #endregion Compare Tests
    }
}

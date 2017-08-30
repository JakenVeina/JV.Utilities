using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        /**********************************************************************/
        #region Do Tests

        [Test]
        public void Do_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerable<int>)null;
            var action = Substitute.For<Action<int>>();

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Do(action);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void Do_ActionIsNull_ThrowsException()
        {
            var @this = Enumerable.Empty<int>();
            var action = (Action<int>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Do(action);
            });

            result.ParamName.ShouldBe(nameof(action));
        }

        [TestCase(0)]
        [TestCase(5)]
        public void Do_ResultIsNotEnumerated_DoesNotInvokeAction(int itemCount)
        {
            var @this = Enumerable.Range(1, itemCount);
            var action = Substitute.For<Action<int>>();

            @this.Do(action);

            action.DidNotReceive().Invoke(Arg.Any<int>());
        }

        [TestCase(0)]
        [TestCase(5)]
        public void Do_ResultIsEnumerated_InvokesActionForEachItem(int itemCount)
        {
            var @this = Enumerable.Range(1, itemCount);
            var action = Substitute.For<Action<int>>();

            @this.Do(action).ForEach();

            action.Received(itemCount);
            Received.InOrder(() =>
            {
                foreach (var item in @this)
                    action.Received().Invoke(item);
            });
        }

        #endregion Do Tests

        /**********************************************************************/
        #region ForEach(action) Tests

        [Test]
        public void ForEach_Action_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerable<int>)null;
            var action = Substitute.For<Action<int>>();

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ForEach(action);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void ForEach_Action_ActionIsNull_ThrowsException()
        {
            var @this = Enumerable.Empty<int>();
            var action = (Action<int>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ForEach(action);
            });

            result.ParamName.ShouldBe(nameof(action));
        }

        [TestCase(0)]
        [TestCase(5)]
        public void ForEach_Action_Otherwise_InvokesActionForEachItem(int itemCount)
        {
            var @this = Enumerable.Range(1, itemCount);
            var action = Substitute.For<Action<int>>();

            @this.ForEach(action);

            action.Received(itemCount);
            Received.InOrder(() =>
            {
                foreach (var item in @this)
                    action.Received().Invoke(item);
            });
        }

        #endregion ForEach Tests

        /**********************************************************************/
        #region ForEach() Tests

        [Test]
        public void ForEach_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerable<int>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ForEach();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(0)]
        [TestCase(5)]
        public void ForEach_Otherwise_EnumeratesThis(int itemCount)
        {
            var @this = Substitute.For<IEnumerable<int>>();
            var enumerator = Substitute.For<IEnumerator<int>>();

            var moveNextCount = 0;
            enumerator.MoveNext().Returns(x => ++moveNextCount <= itemCount);
            @this.GetEnumerator().Returns(enumerator);

            @this.ForEach();

            @this.Received(1).GetEnumerator();
            enumerator.Received(itemCount + 1).MoveNext();
        }

        #endregion ForEach() Tests

        /**********************************************************************/
        #region AsNewEnumerable Tests

        [Test]
        public void AsNewEnumerable_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerable<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.AsNewEnumerable();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void AsNewEnumerable_ThisIsNotNull_ReturnsNotThis()
        {
            var @this = new List<string>();

            @this.AsNewEnumerable().ShouldNotBeSameAs(@this);
        }

        [TestCase("A")]
        [TestCase("A", "B")]
        [TestCase("A", "B", "C")]
        public void AsNewEnumerable_ThisIsNotNull_EnumerationMatchesThis(params string[] @this)
        {
            @this.AsNewEnumerable().ShouldBeOrderedEquivalentTo(@this);
        }

        #endregion AsNewEnumerable Tests

        /**********************************************************************/
        #region MakeEnumerable Tests

        [TestCase(null)]
        [TestCase("This is a test")]
        [TestCase("This is only a test")]
        public void MakeEnumerable_Always_EnumerationMatchesThis(string @this)
        {
            @this.MakeEnumerable().ShouldBeOrderedEquivalentTo(new[] { @this });
        }

        #endregion MakeEnumerable Tests

        /**********************************************************************/
        #region CartesianProduct Tests

        [Test]
        public void CartesianProduct_SequencesIsNull_ThrowsException()
        {
            var sequences = (IEnumerable<IEnumerable<string>>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                EnumerableExtensions.CartesianProduct(sequences);
            });

            result.ParamName.ShouldBe(nameof(sequences));
        }

        [TestCase(null, null)]
        [TestCase("A,B", null, "C,D")]
        public void CartesianProduct_SequencesContainsNull_ThrowsException(params string[] sequenceStrings)
        {
            var sequences = sequenceStrings.Select(x => x?.Split(',').Select(y => y.Trim()));

            var result = Should.Throw<ArgumentException>(() =>
            {
                sequences.CartesianProduct();
            });

            result.ParamName.ShouldBe(nameof(sequences));
        }
        
        [TestCase("A : 1", "A,1")]
        [TestCase("A : 1,2", "A,1 : A,2")]
        [TestCase("A : 1,2,3", "A,1 : A,2 : A,3")]
        [TestCase("A,B : 1", "A,1 : B,1")]
        [TestCase("A,B : 1,2", "A,1 : A,2 : B,1 : B,2")]
        [TestCase("A,B : 1,2,3", "A,1 : A,2 : A,3 : B,1 : B,2 : B,3")]
        [TestCase("A,B,C : 1", "A,1 : B,1 : C,1")]
        [TestCase("A,B,C : 1,2", "A,1 : A,2 : B,1 : B,2 : C,1 : C,2")]
        [TestCase("A,B,C : 1,2,3", "A,1 : A,2 : A,3 : B,1 : B,2 : B,3 : C,1 : C,2 : C,3")]
        [TestCase("A : 1 : a", "A,1,a")]
        [TestCase("A,B : 1,2 : a,b", "A,1,a : A,1,b : A,2,a : A,2,b : B,1,a : B,1,b : B,2,a : B,2,b")]
        public void CartesianProduct_SequencesAreNotNull_ResultMatchesExpected(string sequencesString, string expectedString)
        {
            var sequences = sequencesString.Split(':').Select(sequenceString => sequenceString.Trim().Split(',').Select(x => x.Trim()).ToArray()).ToArray();

            var expected = expectedString.Split(':').Select(sequenceString => sequenceString.Trim().Split(',').Select(x => x.Trim()).ToArray()).ToArray();
            var result = sequences.CartesianProduct().Select(x => x.ToArray()).ToArray();

            result.ShouldSatisfyAllConditions(
                () => result.Length.ShouldBe(expected.Length),
                () =>
                {
                    foreach (var i in Enumerable.Range(0, result.Length))
                        result[i].ShouldBeOrderedEquivalentTo(expected[i]);
                });
        }

        #endregion CartesianProduct Tests

        /**********************************************************************/
        #region Partition(size) Tests

        [Test]
        public void Partition_Size_ThisIsNull_ThrowsException()
        {
            var @this = null as IEnumerable<string>;
            var size = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Partition(size);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Partition_Size_SizeIsLessThan1_ThrowsException(int size)
        {
            var @this = "this".MakeEnumerable();

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.Partition(size);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(size)),
                () => result.ActualValue.ShouldBe(size));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Partition_Size_ThisIsEmpty_ResultIsEmpty(int size)
        {
            var @this = Enumerable.Empty<string>();

            @this.Partition(size).ShouldBeEmpty();
        }

        [TestCase("1",           1, "1")]
        [TestCase("1",           2, "1")]
        [TestCase("1,2",         1, "1;2")]
        [TestCase("1,2",         2, "1,2")]
        [TestCase("1,2",         3, "1,2")]
        [TestCase("1,2,3",       1, "1;2;3")]
        [TestCase("1,2,3",       2, "1,2;3")]
        [TestCase("1,2,3",       3, "1,2,3")]
        [TestCase("1,2,3",       4, "1,2,3")]
        [TestCase("1,2,3,4",     1, "1;2;3;4")]
        [TestCase("1,2,3,4",     2, "1,2;3,4")]
        [TestCase("1,2,3,4",     3, "1,2,3;4")]
        [TestCase("1,2,3,4",     4, "1,2,3,4")]
        [TestCase("1,2,3,4",     5, "1,2,3,4")]
        [TestCase("1,2,3,4,5",   1, "1;2;3;4;5")]
        [TestCase("1,2,3,4,5",   2, "1,2;3,4;5")]
        [TestCase("1,2,3,4,5",   3, "1,2,3;4,5")]
        [TestCase("1,2,3,4,5",   4, "1,2,3,4;5")]
        [TestCase("1,2,3,4,5",   5, "1,2,3,4,5")]
        [TestCase("1,2,3,4,5",   6, "1,2,3,4,5")]
        [TestCase("1,2,3,4,5,6", 1, "1;2;3;4;5;6")]
        [TestCase("1,2,3,4,5,6", 2, "1,2;3,4;5,6")]
        [TestCase("1,2,3,4,5,6", 3, "1,2,3;4,5,6")]
        [TestCase("1,2,3,4,5,6", 4, "1,2,3,4;5,6")]
        [TestCase("1,2,3,4,5,6", 5, "1,2,3,4,5;6")]
        [TestCase("1,2,3,4,5,6", 6, "1,2,3,4,5,6")]
        [TestCase("1,2,3,4,5,6", 7, "1,2,3,4,5,6")]
        public void Partition_Size_Otherwise_EnumerationMatchesExpected(string thisString, int size, string expectedResultString)
        {
            var @this = thisString.Split(',').ToArray();

            var expectedResult = expectedResultString.Split(';').Select(x => x.Split(',')).ToArray();

            var result = @this.Partition(size).ToArray();

            result.ShouldSatisfyAllConditions(
                () => result.Length.ShouldBe(expectedResult.Length),
                () => result.Zip(expectedResult, (x, y) => new { Actual = x, Expected = y }).ForEach(x => x.Actual.ShouldBeOrderedEquivalentTo(x.Expected)));
        }

        #endregion Partition(size) Tests

        /**********************************************************************/
        #region Partition(splitSelector) Tests

        [Test]
        public void Partition_SplitSelector_ThisIsNull_ThrowsException()
        {
            var @this = null as IEnumerable<string>;
            var splitSelector = new Predicate<string>(x => true);

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Partition(splitSelector);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Partition_SplitSelector_SplitSelectorIsNull_ThrowsException(int size)
        {
            var @this = "this".MakeEnumerable();
            var splitSelector = null as Predicate<string>;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Partition(splitSelector);
            });

            result.ParamName.ShouldBe(nameof(splitSelector));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Partition_Size_ThisIsEmpty_ResultIsEmpty(bool splitSelectorResult)
        {
            var @this = Enumerable.Empty<string>();
            var splitSelector = new Predicate<string>(x => splitSelectorResult);

            @this.Partition(splitSelector).ShouldBeEmpty();
        }

        [TestCase("1",           "1", "1")]
        [TestCase("1",           "2", "1")]
        [TestCase("1,2",         "1", "1;2")]
        [TestCase("1,2",         "2", "1,2")]
        [TestCase("1,2",         "3", "1,2")]
        [TestCase("1,2,3",       "1", "1;2,3")]
        [TestCase("1,2,3",       "2", "1,2;3")]
        [TestCase("1,2,3",       "3", "1,2,3")]
        [TestCase("1,2,3",       "4", "1,2,3")]
        [TestCase("1,2,3,4",     "1", "1;2,3,4")]
        [TestCase("1,2,3,4",     "2", "1,2;3,4")]
        [TestCase("1,2,3,4",     "3", "1,2,3;4")]
        [TestCase("1,2,3,4",     "4", "1,2,3,4")]
        [TestCase("1,2,3,4",     "5", "1,2,3,4")]
        [TestCase("1,2,3,4,5",   "1", "1;2,3,4,5")]
        [TestCase("1,2,3,4,5",   "2", "1,2;3,4,5")]
        [TestCase("1,2,3,4,5",   "3", "1,2,3;4,5")]
        [TestCase("1,2,3,4,5",   "4", "1,2,3,4;5")]
        [TestCase("1,2,3,4,5",   "5", "1,2,3,4,5")]
        [TestCase("1,2,3,4,5",   "6", "1,2,3,4,5")]
        [TestCase("1,2,3,4,5,6", "1", "1;2,3,4,5,6")]
        [TestCase("1,2,3,4,5,6", "2", "1,2;3,4,5,6")]
        [TestCase("1,2,3,4,5,6", "3", "1,2,3;4,5,6")]
        [TestCase("1,2,3,4,5,6", "4", "1,2,3,4;5,6")]
        [TestCase("1,2,3,4,5,6", "5", "1,2,3,4,5;6")]
        [TestCase("1,2,3,4,5,6", "6", "1,2,3,4,5,6")]
        [TestCase("1,2,3,4,5,6", "7", "1,2,3,4,5,6")]
        public void Partition_SplitSelector_Otherwise_EnumerationMatchesExpected(string thisString, string splitItem, string expectedResultString)
        {
            var @this = thisString.Split(',').ToArray();
            var splitSelector = new Predicate<string>(x => (x == splitItem));

            var expectedResult = expectedResultString.Split(';').Select(x => x.Split(',')).ToArray();

            var result = @this.Partition(splitSelector).ToArray();

            result.ShouldSatisfyAllConditions(
                () => result.Length.ShouldBe(expectedResult.Length),
                () => result.Zip(expectedResult, (x, y) => new { Actual = x, Expected = y }).ForEach(x => x.Actual.ShouldBeOrderedEquivalentTo(x.Expected)));
        }

        #endregion Partition(splitSelector) Tests
    }
}

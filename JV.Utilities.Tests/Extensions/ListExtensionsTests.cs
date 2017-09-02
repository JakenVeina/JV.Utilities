using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

#if NET40
using ReadOnlyCollectionsExtensions;
#endif

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class ListExtensionsTests
    {
#if NET40
        /**********************************************************************/
        #region IndexRange(IList<T>) Tests

        [Test]
        public void IndexRange_IList_ThisIsNull_ThrowsException()
        {
            var @this = null as IList<int>;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void IndexRange_IList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultIsExpected(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            var result = @this.IndexRange();

            @this.RemoveAt(removeAtIndex);
                
            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount - 1));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void IndexRange_IList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultEachDoesNotExceedThisBounds(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            foreach (var index in @this.IndexRange())
            {
                index.ShouldSatisfyAllConditions(
                    () => index.ShouldBeGreaterThanOrEqualTo(0),
                    () => index.ShouldBeLessThan(@this.Count));

                if (index == removeAtIndex)
                    @this.RemoveAt(index);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void IndexRange_IList_Otherwise_ResultIsExpected(int thisCount)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            @this.IndexRange().ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount));
        }

#endregion IndexRange(IList<T>) Tests

        /**********************************************************************/
        #region ReverseIndexRange(IList<T>) Tests

        [Test]
        public void ReverseIndexRange_IList_ThisIsNull_ThrowsException()
        {
            var @this = null as IList<int>;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ReverseIndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultIsExpected(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            var result = @this.ReverseIndexRange();

            @this.RemoveAt(removeAtIndex);

            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount - 1).Reverse());
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultEachDoesNotExceedThisBounds(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            foreach (var index in @this.ReverseIndexRange())
            {
                index.ShouldSatisfyAllConditions(
                    () => index.ShouldBeGreaterThanOrEqualTo(0),
                    () => index.ShouldBeLessThan(@this.Count));

                if (index == removeAtIndex)
                    @this.RemoveAt(index);
            }
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IList_ThisAllItemsAreRemovedDuringIndexRangeEnumeration_EnumerationTerminates(int thisCount, int clearIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            var enumerator = @this.ReverseIndexRange().GetEnumerator();

            while(enumerator.MoveNext())
            {
                if (enumerator.Current == clearIndex)
                {
                    @this.Clear();
                    break;
                }
            }

            enumerator.MoveNext().ShouldBeFalse();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ReverseIndexRange_IList_Otherwise_ResultIsExpected(int thisCount)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

            @this.ReverseIndexRange().ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount).Reverse());
        }

        #endregion IndexRange(IList<T>) Tests
#endif

        /**********************************************************************/
        #region IndexRange(IReadOnlyList<T>) Tests

        [Test]
        public void IndexRange_IReadOnlyList_ThisIsNull_ThrowsException()
        {
            var @this = null as IReadOnlyList<int>;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void IndexRange_IReadOnlyList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultIsExpected(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();
            
#if NET40
            var result = @this.AsReadOnlyList().IndexRange();
#else
            var result = @this.IndexRange();
#endif

            @this.RemoveAt(removeAtIndex);

            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount - 1));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void IndexRange_IReadOnlyList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultEachDoesNotExceedThisBounds(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var result = @this.AsReadOnlyList().IndexRange();
#else
            var result = @this.IndexRange();
#endif

            foreach (var index in result)
            {
                index.ShouldSatisfyAllConditions(
                    () => index.ShouldBeGreaterThanOrEqualTo(0),
                    () => index.ShouldBeLessThan(@this.Count));

                if (index == removeAtIndex)
                    @this.RemoveAt(index);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void IndexRange_IReadOnlyList_Otherwise_ResultIsExpected(int thisCount)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var result = @this.AsReadOnlyList().IndexRange();
#else
            var result = @this.IndexRange();
#endif

            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount));
        }

#endregion IndexRange(IReadOnlyList<T>) Tests

        /**********************************************************************/
        #region ReverseIndexRange(IReadOnlyList<T>) Tests

        [Test]
        public void ReverseIndexRange_IReadOnlyList_ThisIsNull_ThrowsException()
        {
            var @this = null as IReadOnlyList<int>;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ReverseIndexRange();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IReadOnlyList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultIsExpected(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var result = @this.AsReadOnlyList().ReverseIndexRange();
#else
            var result = @this.ReverseIndexRange();
#endif

            @this.RemoveAt(removeAtIndex);

            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount - 1).Reverse());
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IReadOnlyList_ThisItemIsRemovedDuringIndexRangeEnumeration_ResultEachDoesNotExceedThisBounds(int thisCount, int removeAtIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var result = @this.AsReadOnlyList().ReverseIndexRange();
#else
            var result = @this.ReverseIndexRange();
#endif

            foreach (var index in result)
            {
                index.ShouldSatisfyAllConditions(
                    () => index.ShouldBeGreaterThanOrEqualTo(0),
                    () => index.ShouldBeLessThan(@this.Count));

                if (index == removeAtIndex)
                    @this.RemoveAt(index);
            }
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void ReverseIndexRange_IReadOnlyList_ThisAllItemsAreRemovedDuringIndexRangeEnumeration_EnumerationTerminates(int thisCount, int clearIndex)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var enumerator = @this.AsReadOnlyList().ReverseIndexRange().GetEnumerator();
#else
            var enumerator = @this.ReverseIndexRange().GetEnumerator();
#endif

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == clearIndex)
                {
                    @this.Clear();
                    break;
                }
            }

            enumerator.MoveNext().ShouldBeFalse();
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ReverseIndexRange_IReadOnlyList_Otherwise_ResultIsExpected(int thisCount)
        {
            var @this = Enumerable.Repeat(0, thisCount).ToList();

#if NET40
            var result = @this.AsReadOnlyList().ReverseIndexRange();
#else
            var result = @this.ReverseIndexRange();
#endif

            result.ShouldBeOrderedEquivalentTo(Enumerable.Range(0, thisCount).Reverse());
        }

        #endregion IndexRange(IReadOnlyList<T>) Tests
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class EnumeratorExtensionsTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly string[] TestCases_Sequences =
        {
            "A",
            "A, B, C",
        };

        private static readonly object[] TestCases_ItemsRemain =
        {
            new object[] { "A", 0 },
            new object[] { "A, B, C", 0 },
            new object[] { "A, B, C", 1 },
            new object[] { "A, B, C", 2 }
        };

        #endregion Test Data

        /**********************************************************************/
        #region GetNext Tests

        [Test]
        public void GetNext_Generic_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerator<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.GetNext();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCaseSource(nameof(TestCases_ItemsRemain))]
        public void GetNext_Generic_ItemsRemain_ReturnsNextItem(string sequenceString, int skipCount)
        {
            var @this = sequenceString.Split(',').Select(x => x.Trim()).ToArray();
            using (var enumerator = ((IEnumerable<string>)@this).GetEnumerator())
            {
                var expectedResult = @this.Skip(skipCount).ToArray();
                foreach (var i in Enumerable.Range(0, skipCount))
                    enumerator.MoveNext();

                var result = Enumerable.Range(0, (@this.Length - skipCount)).Select(x => enumerator.GetNext()).ToArray();

                result.ShouldBeOrderedEquivalentTo(expectedResult);
            }
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void GetNext_Generic_NoItemsRemain_ReturnsDefault(string sequenceString)
        {
            var @this = sequenceString.Split(',').Select(x => x.Trim()).ToArray();
            using (var enumerator = ((IEnumerable<string>)@this).GetEnumerator())
            {
                foreach (var i in Enumerable.Range(0, @this.Length))
                    enumerator.MoveNext();

                enumerator.GetNext().ShouldBeNull();
            }
        }

        [Test]
        public void GetNext_NonGeneric_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerator)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.GetNext();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCaseSource(nameof(TestCases_ItemsRemain))]
        public void GetNext_NonGeneric_ItemsRemain_ReturnsNextItem(string thisString, int skipCount)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();

            var enumerator = @this.GetEnumerator();

            var expectedResult = @this.Skip(skipCount).ToArray();
            foreach (var i in Enumerable.Range(0, skipCount))
                enumerator.MoveNext();

            var result = Enumerable.Range(0, (@this.Length - skipCount)).Select(x => enumerator.GetNext()).ToArray();

            result.ShouldBeOrderedEquivalentTo(expectedResult);
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void GetNext_NonGeneric_NoItemsRemain_ReturnsDefault(string thisString)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();

            var enumerator = @this.GetEnumerator();

            foreach (var i in Enumerable.Range(0, @this.Length))
                enumerator.MoveNext();

            enumerator.GetNext().ShouldBeNull();
        }

        #endregion GetNext Tests

        /**********************************************************************/
        #region GetRemaining Tests

        [Test]
        public void GetRemaining_Generic_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerator<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.GetRemaining();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCaseSource(nameof(TestCases_ItemsRemain))]
        public void GetRemaining_Generic_ItemsRemain_ReturnsRemainingItems(string thisString, int skipCount)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();
            using (var enumerator = @this.AsEnumerable().GetEnumerator())
            {
                foreach (var i in Enumerable.Range(0, skipCount))
                    enumerator.MoveNext();

                enumerator.GetRemaining().ShouldBeOrderedEquivalentTo(@this.Skip(skipCount));
            }
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void GetRemaining_Generic_NoItemsRemain_ReturnsRemainingItems(string thisString)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();
            using (var enumerator = @this.AsEnumerable().GetEnumerator())
            {
                foreach (var i in Enumerable.Range(0, @this.Length))
                    enumerator.MoveNext();

                enumerator.GetRemaining().ShouldBeEmpty();
            }
        }

        [Test]
        public void GetRemaining_NonGeneric_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerator)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.GetRemaining();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCaseSource(nameof(TestCases_ItemsRemain))]
        public void GetRemaining_NonGeneric_ItemsRemain_ReturnsRemainingItems(string thisString, int skipCount)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();

            var enumerator = @this.GetEnumerator();

            var expectedResult = @this.Skip(skipCount).ToArray();

            foreach (var i in Enumerable.Range(0, skipCount))
                enumerator.MoveNext();

            enumerator.GetRemaining().Cast<object>().ShouldBeOrderedEquivalentTo(@this.Skip(skipCount).Cast<object>());
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void GetRemaining_NonGeneric_NoItemsRemain_ReturnsRemainingItems(string thisString)
        {
            var @this = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var enumerator = @this.GetEnumerator();
            foreach (var i in Enumerable.Range(0, @this.Length))
                enumerator.MoveNext();

            enumerator.GetRemaining().Cast<object>().ShouldBeEmpty();
        }

        #endregion GetRemaining Tests
    }
}

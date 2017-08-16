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
    public class LinkedListExtensionsTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly string[] TestCases_Sequences =
        {
            "A",
            "A, B, C",
        };

        private static readonly object[] TestCases_CriteriaIdentifiesSingle =
        {
            new object[] { "A", "A" },
            new object[] { "A, B, C", "A" },
            new object[] { "A, B, C", "B" },
            new object[] { "A, B, C", "C" }
        };

        #endregion Test Data

        /**********************************************************************/
        #region RemoveWhere Tests

        [Test]
        public void RemoveWhere_ThisIsNull_ThrowsException()
        {
            var @this = (LinkedList<string>)null;
            var criteria = Substitute.For<Predicate<string>>();

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.RemoveWhere(criteria);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void RemoveWhere_CriteriaIsNull_ThrowsException()
        {
            var @this = new LinkedList<string>();
            var criteria = (Predicate<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.RemoveWhere(criteria);
            });

            result.ParamName.ShouldBe(nameof(criteria));
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void RemoveWhere_CriteriaIdentifiesAll_RemovesAll(string thisString)
        {
            var @this = new LinkedList<string>(thisString.Split(',').Select(x => x.Trim()).ToArray());

            @this.RemoveWhere(x => true);

            @this.ShouldBeEmpty();
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void RemoveWhere_CriteriaIdentifiesNone_RemovesNone(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.RemoveWhere(x => false);

            @this.ShouldBeOrderedEquivalentTo(source);
        }

        [TestCaseSource(nameof(TestCases_CriteriaIdentifiesSingle))]
        public void RemoveWhere_CriteriaIdentifiesSingle_RemovesSingle(string thisString, string itemToRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.RemoveWhere(x => (x == itemToRemove));

            @this.ShouldBeOrderedEquivalentTo(source.Where(x => x != itemToRemove));
        }

        [TestCaseSource(nameof(TestCases_CriteriaIdentifiesSingle))]
        public void RemoveWhere_CriteriaIdentifiesAllButSingle_RemovesAllButSingle(string thisString, string itemToNotRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.RemoveWhere(x => (x != itemToNotRemove));

            @this.ShouldBeOrderedEquivalentTo(source.Where(x => (x == itemToNotRemove)));
        }

        [TestCase("A, B, C, D, E")]
        public void RemoveWhere_CriteriaIdentifiesEven_RemovesEven(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var itemsToRemove = source.Where((x, i) => ((i % 2) == 0)).ToList();
            var @this = new LinkedList<string>(source);

            @this.RemoveWhere(x => itemsToRemove.Contains(x));

            @this.ShouldBeOrderedEquivalentTo(source.Except(itemsToRemove));
        }

        [TestCase("A, B, C, D, E")]
        public void RemoveWhere_CriteriaIdentifiesOdd_RemovesOdd(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var itemsToRemove = source.Where((x, i) => ((i % 2) == 1)).ToList();
            var @this = new LinkedList<string>(source);

            @this.RemoveWhere(x => itemsToRemove.Contains(x));

            @this.ShouldBeOrderedEquivalentTo(source.Except(itemsToRemove));
        }

        [TestCase("A, C, B, C, D", "C")]
        public void RemoveWhere_CriteriaIdentifiesFirstOccurrance_RemovesFirst(string thisString, string itemToRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            var hasRemoved = false;
            @this.RemoveWhere(x =>
            {
                if (!hasRemoved && (x == itemToRemove))
                {
                    hasRemoved = true;
                    return true;
                }
                return false;
            });

            @this.ShouldBeOrderedEquivalentTo(source.Where((x, i) => (i != Array.IndexOf(source, itemToRemove))));
        }

        #endregion RemoveWhere Tests

        /**********************************************************************/
        #region ReverseRemoveWhere Tests

        [Test]
        public void ReverseRemoveWhere_ThisIsNull_ThrowsException()
        {
            var @this = (LinkedList<string>)null;
            var criteria = (Predicate<string>)(x => true);

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ReverseRemoveWhere(criteria);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [Test]
        public void ReverseRemoveWhere_CriteriaIsNull_ThrowsException()
        {
            var @this = new LinkedList<string>();
            var criteria = (Predicate<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.ReverseRemoveWhere(criteria);
            });

            result.ParamName.ShouldBe(nameof(criteria));
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void ReverseRemoveWhere_CriteriaIdentifiesAll_RemovesAll(string thisString)
        {
            var @this = new LinkedList<string>(thisString.Split(',').Select(x => x.Trim()).ToArray());

            @this.ReverseRemoveWhere(x => true);

            @this.ShouldBeEmpty();
        }

        [TestCaseSource(nameof(TestCases_Sequences))]
        public void ReverseRemoveWhere_CriteriaIdentifiesNone_RemovesNone(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.ReverseRemoveWhere(x => false);

            @this.ShouldBeOrderedEquivalentTo(source);
        }

        [TestCaseSource(nameof(TestCases_CriteriaIdentifiesSingle))]
        public void ReverseRemoveWhere_CriteriaIdentifiesSingle_RemovesSingle(string thisString, string itemToRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.ReverseRemoveWhere(x => (x == itemToRemove));

            @this.ShouldBeOrderedEquivalentTo(source.Where(x => (x != itemToRemove)));
        }

        [TestCaseSource(nameof(TestCases_CriteriaIdentifiesSingle))]
        public void ReverseRemoveWhere_CriteriaIdentifiesAllButSingle_RemovesAllButSingle(string thisString, string itemToNotRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var @this = new LinkedList<string>(source);

            @this.ReverseRemoveWhere(x => (x != itemToNotRemove));

            @this.ShouldBeOrderedEquivalentTo(source.Where(x => (x == itemToNotRemove)));
        }

        [TestCase("A, B, C, D, E")]
        public void ReverseRemoveWhere_CriteriaIdentifiesEven_RemovesEven(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var itemsToRemove = source.Where((x, i) => ((i % 2) == 0)).ToList();
            var @this = new LinkedList<string>(source);

            @this.ReverseRemoveWhere(x => itemsToRemove.Contains(x));

            @this.ShouldBeOrderedEquivalentTo(source.Except(itemsToRemove));
        }

        [TestCase("A, B, C, D, E")]
        public void ReverseRemoveWhere_CriteriaIdentifiesOdd_RemovesOdd(string thisString)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var itemsToRemove = source.Where((x, i) => ((i % 2) == 1)).ToList();
            var @this = new LinkedList<string>(source);

            @this.ReverseRemoveWhere(x => itemsToRemove.Contains(x));

            @this.ShouldBeOrderedEquivalentTo(source.Except(itemsToRemove));
        }

        [TestCase("A, C, B, C, D", "C")]
        public void ReverseRemoveWhere_CriteriaIdentifiesFirstOccurrance_RemovesLast(string thisString, string itemToRemove)
        {
            var source = thisString.Split(',').Select(x => x.Trim()).ToArray();
            var reverseSource = source.Reverse().ToArray();
            var @this = new LinkedList<string>(source);

            var hasRemoved = false;
            @this.ReverseRemoveWhere(x =>
            {
                if (!hasRemoved && (x == itemToRemove))
                {
                    hasRemoved = true;
                    return true;
                }
                return false;
            });

            @this.ShouldBeOrderedEquivalentTo(reverseSource.Where((x, i) => (i != Array.IndexOf(reverseSource, itemToRemove))).Reverse());
        }

        #endregion ReverseRemoveWhere Tests
    }
}

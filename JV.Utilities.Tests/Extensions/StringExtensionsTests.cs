using System;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        /**********************************************************************/
        #region IndexOfOccurrence Tests

        [Test]
        public void IndexOfOccurrence_Char_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var value = 'x';
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexOfOccurrence(value, occurrence);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase('A')]
        [TestCase('a')]
        public void IndexOfOccurrence_Char_ThisIsEmpty_ReturnsNegative1(char value)
        {
            var @this = string.Empty;
            var occurrence = 1;

            @this.IndexOfOccurrence(value, occurrence).ShouldBe(-1);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IndexOfOccurrence_Char_OccurrenceIsLessThan1_ThrowsException(int occurrence)
        {
            var @this = "This is only a test";
            var value = 'x';

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.IndexOfOccurrence(value, occurrence);
            });

            result.ParamName.ShouldBe(nameof(occurrence));
            result.ActualValue.ShouldBe(occurrence);
        }

        [TestCase("aaa", 'A', 1, -1)]
        [TestCase("aaa", 'a', 1, 0)]
        [TestCase("aaa", 'a', 2, 1)]
        [TestCase("aaa", 'a', 3, 2)]
        [TestCase("aaa", 'a', 4, -1)]
        [TestCase("aaa", 'b', 1, -1)]
        [TestCase("baa", 'b', 1, 0)]
        [TestCase("baa", 'b', 2, -1)]
        [TestCase("aba", 'b', 1, 1)]
        [TestCase("aba", 'b', 2, -1)]
        [TestCase("aab", 'b', 1, 2)]
        [TestCase("aab", 'b', 2, -1)]
        [TestCase("ababa", 'a', 1, 0)]
        [TestCase("ababa", 'a', 2, 2)]
        [TestCase("ababa", 'a', 3, 4)]
        public void IndexOfOccurrence_Char_ThisContainsValueOccurrence_ReturnsExpectedIndex(string @this, char value, int occurrence, int expectedIndex)
        {
            @this.IndexOfOccurrence(value, occurrence).ShouldBe(expectedIndex);
        }

        [Test]
        public void IndexOfOccurrence_String_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var value = "this";
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexOfOccurrence(value, occurrence);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase("A")]
        [TestCase("a")]
        [TestCase("test")]
        public void IndexOfOccurrence_String_ThisIsEmpty_ReturnsNegative1(string value)
        {
            var @this = string.Empty;
            var occurrence = 1;

            @this.IndexOfOccurrence(value, occurrence).ShouldBe(-1);
        }

        [Test]
        public void IndexOfOccurrence_String_ValueIsNull_ThrowsException()
        {
            var @this = "test";
            var value = (string)null;
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.IndexOfOccurrence(value, occurrence);
            });

            result.ParamName.ShouldBe(nameof(value));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void IndexOfOccurrence_String_OccurrenceIsLessThan1_ThrowsException(int occurrence)
        {
            var @this = "This is only a test";
            var value = "x";

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.IndexOfOccurrence(value, occurrence);
            });

            result.ParamName.ShouldBe(nameof(occurrence));
            result.ActualValue.ShouldBe(occurrence);
        }

        [TestCase("aaa", "", 1, 0)]
        [TestCase("aaa", "", 2, 1)]
        [TestCase("aaa", "", 3, 2)]
        [TestCase("aaa", "", 4, -1)]
        [TestCase("aaa", "A", 1, -1)]
        [TestCase("aaa", "a", 1, 0)]
        [TestCase("aaa", "a", 2, 1)]
        [TestCase("aaa", "a", 3, 2)]
        [TestCase("aaa", "a", 4, -1)]
        [TestCase("aaa", "b", 1, -1)]
        [TestCase("baa", "b", 1, 0)]
        [TestCase("baa", "b", 2, -1)]
        [TestCase("aba", "b", 1, 1)]
        [TestCase("aba", "b", 2, -1)]
        [TestCase("aab", "b", 1, 2)]
        [TestCase("aab", "b", 2, -1)]
        [TestCase("ababa", "a", 1, 0)]
        [TestCase("ababa", "a", 2, 2)]
        [TestCase("ababa", "a", 3, 4)]
        [TestCase("This is a test.", "test", 1, 10)]
        [TestCase("This is a test.", "test", 2, -1)]
        [TestCase("This is a test.", "is", 1, 2)]
        [TestCase("This is a test.", "is", 2, 5)]
        [TestCase("This is a test.", "is", 3, -1)]
        [TestCase("This is a test.", "nope", 1, -1)]
        public void IndexOfOccurrence_String_ThisContainsValueOccurrence_ReturnsExpectedIndex(string @this, string value, int occurrence, int expectedIndex)
        {
            @this.IndexOfOccurrence(value, occurrence).ShouldBe(expectedIndex);
        }

        [TestCase("AABABCABCD", "ab", 1, 1)]
        [TestCase("AABABCABCD", "AB", 1, 1)]
        [TestCase("AABABCABCD", "ab", 2, 3)]
        [TestCase("AABABCABCD", "AB", 2, 3)]
        [TestCase("AABABCABCD", "ab", 3, 6)]
        [TestCase("AABABCABCD", "AB", 3, 6)]
        public void IndexOfOccurrence_String_ComparisonTypeIsIgnoreCase_IsCaseInsensitive(string @this, string value, int occurrence, int expectedIndex)
        {
            @this.IndexOfOccurrence(value, occurrence, StringComparison.OrdinalIgnoreCase).ShouldBe(expectedIndex);
        }

        #endregion IndexOfOccurrence Tests

        /**********************************************************************/
        #region Replace Tests

        [Test]
        public void Replace_Char_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var oldChar = 'x';
            var newChar = 'y';
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Replace(oldChar, newChar, occurrence);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase('A')]
        [TestCase('a')]
        public void Replace_Char_ThisIsEmpty_ReturnsEmpty(char oldChar)
        {
            var @this = string.Empty;
            var newChar = ';';
            var occurrence = 1;

            @this.Replace(oldChar, newChar, occurrence).ShouldBeEmpty();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Replace_Char_OccurrenceIsLessThan1_ThrowsException(int occurrence)
        {
            var @this = "This is only a test";
            var oldChar = 'x';
            var newChar = 'y';

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.Replace(oldChar, newChar, occurrence);
            });

            result.ParamName.ShouldBe(nameof(occurrence));
            result.ActualValue.ShouldBe(occurrence);
        }

        [TestCase("aaa", 'A', ';', 1, "aaa")]
        [TestCase("aaa", 'a', ';', 1, ";aa")]
        [TestCase("aaa", 'a', ';', 2, "a;a")]
        [TestCase("aaa", 'a', ';', 3, "aa;")]
        [TestCase("aaa", 'a', ';', 4, "aaa")]
        [TestCase("aaa", 'b', ';', 1, "aaa")]
        [TestCase("baa", 'b', ';', 1, ";aa")]
        [TestCase("baa", 'b', ';', 2, "baa")]
        [TestCase("aba", 'b', ';', 1, "a;a")]
        [TestCase("aba", 'b', ';', 2, "aba")]
        [TestCase("aab", 'b', ';', 1, "aa;")]
        [TestCase("aab", 'b', ';', 2, "aab")]
        [TestCase("ababa", 'a', ';', 1, ";baba")]
        [TestCase("ababa", 'a', ';', 2, "ab;ba")]
        [TestCase("ababa", 'a', ';', 3, "abab;")]
        [TestCase("ababa", 'a', ';', 4, "ababa")]
        [TestCase("aaaaa", 'a', '1', 1, "1aaaa")]
        [TestCase("aaaaa", 'a', '\0', 1, "\0aaaa")]
        public void Replace_Char_ThisContainsOldCharOccurrence_ReplacesSpecifiedOccurrenceOfOldCharWithNewChar(string @this, char oldChar, char newChar, int occurrence, string expectedResult)
        {
            @this.Replace(oldChar, newChar, occurrence).ShouldBe(expectedResult);
        }

        [Test]
        public void Replace_String_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var oldValue = "X";
            var newValue = "Y";
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Replace(oldValue, newValue, occurrence);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase("A")]
        [TestCase("a")]
        [TestCase("test")]
        public void Replace_String_ThisIsEmpty_ReturnsEmpty(string oldValue)
        {
            var @this = string.Empty;
            var newValue = ";";
            var occurrence = 1;

            @this.Replace(oldValue, newValue, occurrence).ShouldBeEmpty();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Replace_String_OccurrenceIsLessThan1_ThrowsException(int occurrence)
        {
            var @this = "This is only a test";
            var oldValue = "x";
            var newValue = "y";

            var result = Should.Throw<ArgumentOutOfRangeException>(() =>
            {
                @this.Replace(oldValue, newValue, occurrence);
            });

            result.ParamName.ShouldBe(nameof(occurrence));
            result.ActualValue.ShouldBe(occurrence);
        }

        [Test]
        public void Replace_String_OldValueIsNull_ThrowsException()
        {
            var @this = "This is a test";
            var oldValue = (string)null;
            var newValue = "Y";
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Replace(oldValue, newValue, occurrence);
            });

            result.ParamName.ShouldBe(nameof(oldValue));
        }

        [Test]
        public void Replace_String_NewValueIsNull_ThrowsException()
        {
            var @this = "This is a test";
            var oldValue = "X";
            var newValue = (string)null;
            var occurrence = 1;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Replace(oldValue, newValue, occurrence);
            });

            result.ParamName.ShouldBe(nameof(newValue));
        }

        [TestCase("aaa", "", ";", 1, ";aaa")]
        [TestCase("aaa", "", ";", 2, "a;aa")]
        [TestCase("aaa", "", ";", 3, "aa;a")]
        [TestCase("aaa", "", ";", 4, "aaa")]
        [TestCase("aaa", "A", ";", 1, "aaa")]
        [TestCase("aaa", "a", ";", 1, ";aa")]
        [TestCase("aaa", "a", ";", 2, "a;a")]
        [TestCase("aaa", "a", ";", 3, "aa;")]
        [TestCase("aaa", "a", ";", 4, "aaa")]
        [TestCase("aaa", "b", ";", 1, "aaa")]
        [TestCase("baa", "b", ";", 1, ";aa")]
        [TestCase("baa", "b", ";", 2, "baa")]
        [TestCase("aba", "b", ";", 1, "a;a")]
        [TestCase("aba", "b", ";", 2, "aba")]
        [TestCase("aab", "b", ";", 1, "aa;")]
        [TestCase("aab", "b", ";", 2, "aab")]
        [TestCase("ababa", "a", ";", 1, ";baba")]
        [TestCase("ababa", "a", ";", 2, "ab;ba")]
        [TestCase("ababa", "a", ";", 3, "abab;")]
        [TestCase("ababa", "a", ";", 4, "ababa")]
        [TestCase("aaa", "a", "replacement", 1, "replacementaa")]
        [TestCase("aaa", "a", "", 1, "aa")]
        [TestCase("aaa", "a", "", 2, "aa")]
        [TestCase("aaa", "a", "", 3, "aa")]
        [TestCase("aaa", "a", "", 4, "aaa")]
        public void Replace_Char_ThisContainsOldValueOccurrence_ReplacesSpecifiedOccurrenceOfOldValueWithNewValue(string @this, string oldValue, string newValue, int occurrence, string expectedResult)
        {
            @this.Replace(oldValue, newValue, occurrence).ShouldBe(expectedResult);
        }

        [TestCase("AABABCABCD", "ab", ";", 1, "A;ABCABCD")]
        [TestCase("AABABCABCD", "AB", ";", 1, "A;ABCABCD")]
        [TestCase("AABABCABCD", "ab", ";", 2, "AAB;CABCD")]
        [TestCase("AABABCABCD", "AB", ";", 2, "AAB;CABCD")]
        [TestCase("AABABCABCD", "ab", ";", 3, "AABABC;CD")]
        [TestCase("AABABCABCD", "AB", ";", 3, "AABABC;CD")]
        public void Replace_String_ComparisonTypeIsIgnoreCase_IsCaseInsensitive(string @this, string oldValue, string newValue, int occurrence, string expectedResult)
        {
            @this.Replace(oldValue, newValue, occurrence, StringComparison.OrdinalIgnoreCase).ShouldBe(expectedResult);
        }

        #endregion Replace Tests

        /**********************************************************************/
        #region Contains Tests

        [Test]
        public void Contains_ThisIsNull_ThrowsException()
        {
            var @this = (string)null;
            var value = "A";
            var comparisonType = StringComparison.Ordinal;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Contains(value, comparisonType);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase("A")]
        [TestCase("a")]
        [TestCase("test")]
        public void Contains_ThisIsEmpty_ReturnsFalse(string value)
        {
            var @this = string.Empty;
            var comparisonType = StringComparison.Ordinal;

            @this.Contains(value, comparisonType).ShouldBeFalse();
        }

        [Test]
        public void Contains_ValueIsNull_ThrowsException()
        {
            var @this = "This is a tesT";
            var value = (string)null;
            var comparisonType = StringComparison.Ordinal;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.Contains(value, comparisonType);
            });

            result.ParamName.ShouldBe(nameof(value));
        }

        [TestCase("A")]
        [TestCase("a")]
        [TestCase("test")]
        public void Contains_ValueIsEmpty_ReturnsTrue(string @this)
        {
            var value = string.Empty;
            var comparisonType = StringComparison.Ordinal;

            @this.Contains(value, comparisonType).ShouldBeTrue();
        }

        [TestCase("aaa", "a")]
        [TestCase("baa", "b")]
        [TestCase("aba", "b")]
        [TestCase("aab", "b")]
        [TestCase("This is a test.", "test")]
        [TestCase("This is a test.", "is")]
        public void Contains_ThisContainsValue_ReturnsTrue(string @this, string value)
        {
            var comparisonType = StringComparison.Ordinal;

            @this.Contains(value, comparisonType).ShouldBeTrue();
        }

        [TestCase("aaa", "A")]
        [TestCase("aaa", "b")]
        [TestCase("This is a test.", "not")]
        public void Contains_ThisDoesNotContainValue_ReturnsFalse(string @this, string value)
        {
            var comparisonType = StringComparison.Ordinal;

            @this.Contains(value, comparisonType).ShouldBeFalse();
        }

        [TestCase("aaaaa", "A", true)]
        [TestCase("aaaaa", "a", true)]
        public void Contains_String_ComparisonTypeIsIgnoreCase_IsCaseInsensitive(string @this, string value, bool expectedResult)
        {
            var comparisonType = StringComparison.OrdinalIgnoreCase;

            @this.Contains(value, comparisonType).ShouldBe(expectedResult);
        }

        #endregion Contains Tests
    }
}

using System.ComponentModel;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Collections;
using JV.Utilities.Wpf.Mvvm;

namespace JV.Utilities.Wpf.Tests.Mvvm
{
    [TestFixture]
    public class StringCollectionVMTests
    {
        /**********************************************************************/
        #region Test Data

        private static readonly object[] TestCases_CsvTextIsNotNullOrEmpty =
        {
            " ",
            "A",
            "A, B, C"
        };

        #endregion Test Data

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_Always_StringsIsEmptyObservableCollection()
        {
            var uut = new StringCollectionVM();

            uut.Strings.ShouldNotBeNull();
            uut.Strings.ShouldBeOfType<ObservableCollection<string>>();
            uut.Strings.ShouldBeEmpty();
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region CsvText Tests

        [TestCase(null)]
        [TestCase("")]
        [TestCaseSource(nameof(TestCases_CsvTextIsNotNullOrEmpty))]
        public void CsvText_Always_CsvTextEqualsGiven(string csvText)
        {
            var uut = new StringCollectionVM();

            uut.CsvText = csvText;

            uut.CsvText.ShouldBe(csvText);
        }

        [Test]
        public void CsvText_ValueIsNull_StringsIsEmpty()
        {
            var uut = new StringCollectionVM();

            uut.CsvText = "";

            uut.CsvText = null;

            uut.Strings.ShouldBeEmpty();
        }

        [TestCase("A",       ""         )]
        [TestCase("",        "A",       "A")]
        [TestCase("A",       " "        )]
        [TestCase(" ",       "A",       "A")]
        [TestCase("A",       "B",       "B")]
        [TestCase("A",       "B, C, D", "B", "C", "D")]
        [TestCase("A, B, C", "D",       "D")]
        [TestCase("A, B, C", "D, E, F", "D", "E", "F")]
        public void CsvText_Always_StringsMatchesExpected(string previousCsvText, string csvText, params string[] strings)
        {
            var uut = new StringCollectionVM();

            uut.CsvText = previousCsvText;

            uut.CsvText = csvText;

            uut.Strings.ShouldBeOrderedEquivalentTo(strings);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCaseSource(nameof(TestCases_CsvTextIsNotNullOrEmpty))]
        public void CsvText_EqualsValue_DoesNotInvokeRaisePropertyChangedWithCsvText(string csvText)
        {
            var uut = new StringCollectionVM();

            uut.CsvText = csvText;

            var handler = Substitute.For<PropertyChangedEventHandler>();
            uut.PropertyChanged += handler;

            uut.CsvText = csvText;

            handler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs>());
        }

        [TestCase("A", "B")]
        public void CsvText_DoesNotEqualValue_InvokesRaisePropertyChangedWithCsvText(string previousCsvText, string csvText)
        {
            var uut = new StringCollectionVM();

            uut.CsvText = previousCsvText;

            var handler = Substitute.For<PropertyChangedEventHandler>();
            uut.PropertyChanged += handler;

            uut.CsvText = csvText;

            handler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.CsvText)));
        }

        #endregion CsvText Tests

        /**********************************************************************/
        #region Strings Tests

        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void StringsCollectionChanged_Add_CsvTextMatchesStringsJoin(params string[] strings)
        {
            var uut = new StringCollectionVM();

            foreach (var str in strings)
                uut.Strings.Add(str);

            uut.CsvText.ShouldBe(string.Join(", ", uut.Strings));
        }

        [TestCase(0, 1, "A", "B", "C")]
        [TestCase(1, 0, "A", "B", "C")]
        [TestCase(1, 2, "A", "B", "C")]
        public void StringsCollectionChanged_Move_CsvTextMatchesStringsJoin(int oldIndex, int newIndex, params string[] strings)
        {
            var uut = new StringCollectionVM();

            foreach (var str in strings)
                uut.Strings.Add(str);

            uut.Strings.Move(oldIndex, newIndex);

            uut.CsvText.ShouldBe(string.Join(", ", uut.Strings));
        }

        [TestCase(0, "D", "A", "B", "C")]
        [TestCase(1, "D", "A", "B", "C")]
        [TestCase(2, "D", "A", "B", "C")]
        public void StringsCollectionChanged_Replace_CsvTextMatchesStringsJoin(int index, string newString, params string[] strings)
        {
            var uut = new StringCollectionVM();

            foreach (var str in strings)
                uut.Strings.Add(str);

            uut.Strings[index] = newString;

            uut.CsvText.ShouldBe(string.Join(", ", uut.Strings));
        }

        [TestCase(0, "A", "B", "C")]
        [TestCase(1, "A", "B", "C")]
        [TestCase(2, "A", "B", "C")]
        public void StringsCollectionChanged_Remove_CsvTextMatchesStringsJoin(int index, params string[] strings)
        {
            var uut = new StringCollectionVM();

            foreach (var str in strings)
                uut.Strings.Add(str);

            uut.Strings.RemoveAt(index);

            uut.CsvText.ShouldBe(string.Join(", ", uut.Strings));
        }

        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void StringsCollectionChanged_Clear_CsvTextIsEmpty(params string[] strings)
        {
            var uut = new StringCollectionVM();

            foreach (var str in strings)
                uut.Strings.Add(str);

            uut.Strings.Clear();

            uut.CsvText.ShouldBeEmpty();
        }

        #endregion Strings Tests
    }
}

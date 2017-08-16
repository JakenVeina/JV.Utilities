using System.ComponentModel;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Mvvm;

namespace JV.Utilities.Wpf.Tests.Mvvm
{
    [TestFixture]
    public class ViewModelBaseTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public ViewModelBase ConstructUUT()
                => Substitute.ForPartsOf<ViewModelBase>();
        }

        public class MockViewModelBase : ViewModelBase { }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_Always_SetsErrorToEmpty()
        {
            var context = new TestContext();

            var uut = context.ConstructUUT();

            uut.Error.ShouldBeEmpty();
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region RaisePropertyChanged Tests

        [TestCase("propertyName")]
        public void RaisePropertyChanged_PropertyChangedIsNull_DoesNotThrowException(string propertyName)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            Should.NotThrow(() =>
            {
                uut.RaisePropertyChanged(propertyName);
            });
        }

        [TestCase("propertyName")]
        public void RaisePropertyChanged_Otherwise_RaisesPropertyChanged(string propertyName)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            var handler = Substitute.For<PropertyChangedEventHandler>();
            uut.PropertyChanged += handler;

            uut.RaisePropertyChanged(propertyName);

            handler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == propertyName));
        }

        #endregion RaisePropertyChanged Tests

        /**********************************************************************/
        #region this[] Tests

        [TestCase("value")]
        public void ThisSet_ColumnNameIsNull_ThisGetEmptyEqualsValue(string value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[null] = value;

            uut[string.Empty].ShouldBe(value);
        }

        [TestCase("value")]
        public void ThisGet_ColumnNameIsNull_ReturnsThisGetEmpty(string value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[string.Empty] = value;

            uut[null].ShouldBe(value);
        }

        [TestCase("columnName")]
        public void ThisSet_ValueIsNull_ThisGetEqualsEmpty(string columnName)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[columnName] = null;

            uut[columnName].ShouldBeEmpty();
        }

        [TestCase("columnName", "previousValue", "otherColumnName", "otherValue")]
        public void ThisSet_ValueIsEmptyAndThisGetOtherColumnNameIsNotEmpty_ErrorEqualsThisGetOtherColumnName(string columnName, string previousValue, string otherColumnName, string otherValue)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[columnName] = previousValue;
            uut[otherColumnName] = otherValue;

            uut[columnName] = string.Empty;

            uut.Error.ShouldBe(otherValue);
        }

        [TestCase("columnName", "previousValue")]
        public void ThisSet_ValueIsEmptyAndThisGetOtherColumnNamesAreEmpty_ErrorIsEmpty(string columnName, string previousValue)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[columnName] = previousValue;
            uut[columnName] = string.Empty;

            uut.Error.ShouldBeEmpty();
        }

        [TestCase("columnName", "value")]
        public void ThisSet_ValueIsNotEmpty_ThisGetEqualsValue(string columnName, string value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[columnName] = value;

            uut[columnName].ShouldBe(value);
        }

        [TestCase("columnName", "value")]
        public void ThisSet_ValueIsNotEmpty_ErrorEqualsValue(string columnName, string value)
        {
            var context = new TestContext();
            var uut = context.ConstructUUT();

            uut[columnName] = value;

            uut.Error.ShouldBe(value);
        }

        #endregion this[] Tests
    }
}

using System;
using System.ComponentModel;
using System.Reflection;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Mvvm;

namespace JV.Utilities.Wpf.Tests.Mvvm
{
    [TestFixture]
    public class BooleanOptionVMTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T>
        {
            public T value;
            public bool initialize = true;

            public PropertyChangedEventHandler propertyChangedHandler = Substitute.For<PropertyChangedEventHandler>();

            public BooleanOptionVM<T> ConstructUUT()
            {
                try
                {
                    var uut = Substitute.ForPartsOf<BooleanOptionVM<T>>();

                    if (initialize)
                        uut.Init(value);

                    uut.PropertyChanged += propertyChangedHandler;

                    return uut;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }

            public void ClearReceivedCalls()
            {
                propertyChangedHandler.ClearReceivedCalls();
            }
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_Always_ValueEqualsDefault()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.Value.ShouldBe(default(int));
        }

        [Test]
        public void Constructor_Always_IsSelectedEqualsFalse()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.IsSelected.ShouldBeFalse();
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region Value Tests

        [Test]
        public void Value_InitHasNotBeenInvoked_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                initialize = false
            };
            var uut = context.ConstructUUT();

            var result = Should.Throw<InvalidOperationException>(() =>
            {
                var value = uut.Value;
            });

            result.Message.ShouldContain(nameof(uut.Init));
        }

        #endregion Value Tests

        /**********************************************************************/
        #region IsSelected Tests

        [Test]
        public void IsSelected_Get_InitHasNotBeenInvoked_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                initialize = false
            };
            var uut = context.ConstructUUT();

            var result = Should.Throw<InvalidOperationException>(() =>
            {
                var isSelected = uut.IsSelected;
            });

            result.Message.ShouldContain(nameof(uut.Init));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsSelected_Set_InitHasNotBeenInvoked_ThrowsException(bool isSelected)
        {
            var context = new TestContext<int>()
            {
                initialize = false
            };
            var uut = context.ConstructUUT();

            var result = Should.Throw<InvalidOperationException>(() =>
            {
                uut.IsSelected = isSelected;
            });

            result.Message.ShouldContain(nameof(uut.Init));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsSelected_Otherwise_IsSelectedEqualsValue(bool isSelected)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.IsSelected = isSelected;

            uut.IsSelected.ShouldBe(isSelected);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsSelected_IsSelectedEqualsValue_DoesNotInvokeRaisePropertyChanged(bool isSelected)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.IsSelected = isSelected;

            context.ClearReceivedCalls();

            uut.IsSelected = isSelected;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs>());
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IsSelected_IsSelectedDoesNotEqualValue_InvokesRaisePropertyChangedWithIsSelected(bool previousIsSelected, bool isSelected)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.IsSelected = previousIsSelected;

            context.ClearReceivedCalls();

            uut.IsSelected = isSelected;

            context.propertyChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.IsSelected)));
        }

        #endregion IsSelected Tests

        /**********************************************************************/
        #region Init Tests

        [TestCase(1)]
        public void Init_InitHasNotBeenInvoked_ValueEqualsGiven(int value)
        {
            var context = new TestContext<int>()
            {
                value = value
            };
            var uut = context.ConstructUUT();

            uut.Value.ShouldBe(value);
        }

        [TestCase(1)]
        public void Init_InitHasBeenInvoked_ThrowsException(int value)
        {
            var context = new TestContext<int>()
            {
                value = value
            };
            var uut = context.ConstructUUT();

            var result = Should.Throw<InvalidOperationException>(() =>
            {
                uut.Init(value);
            });

            result.Message.ShouldContain(nameof(uut.Init));
        }

        [TestCase(1, 2)]
        public void Init_InitHasBeenInvoked_ValueEqualsPrevious(int previousValue, int value)
        {
            var context = new TestContext<int>()
            {
                value = previousValue
            };
            var uut = context.ConstructUUT();

            Should.Throw<InvalidOperationException>(() =>
            {
                uut.Init(value);
            });

            uut.Value.ShouldBe(previousValue);
        }

        #endregion Init Tests

        /**********************************************************************/
        #region AssertHasInitialized Tests

        [Test]
        public void AssertHasInitialized_InitHasNotBeenInvoked_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                initialize = false
            };
            var uut = context.ConstructUUT();

            var result = Should.Throw<InvalidOperationException>(() =>
            {
                uut.AssertHasInitialized();
            });

            result.Message.ShouldContain(nameof(uut.Init));
        }

        [Test]
        public void AssertHasInitialized_InitHasBeenInvoked_DoesNotThrowException()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            Should.NotThrow(() =>
            {
                uut.AssertHasInitialized();
            });
        }

        #endregion AssertHasInitialized Tests
    }
}

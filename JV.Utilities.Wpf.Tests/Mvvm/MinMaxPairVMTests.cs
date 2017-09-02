using System;
using System.ComponentModel;
using System.Reflection;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Observation;
using JV.Utilities.Math;
using JV.Utilities.Wpf.Mvvm;

namespace JV.Utilities.Wpf.Tests.Mvvm
{
    [TestFixture]
    public class MinMaxPairVMTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T> where T : IComparable<T>
        {
            public PropertyChangedEventHandler propertyChangedHandler = Substitute.For<PropertyChangedEventHandler>();
            public EventHandler<PropertyChangedEventArgs<MinMaxPair<T>>> modelChangedHandler = Substitute.For<EventHandler<PropertyChangedEventArgs<MinMaxPair<T>>>>();

            public MinMaxPairVM<T> ConstructUUT()
            {
                try
                {
                    var uut = Substitute.ForPartsOf<MinMaxPairVM<T>>();

                    uut.PropertyChanged += propertyChangedHandler;
                    uut.ModelChanged += modelChangedHandler;

                    return uut;
                }
                catch(TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }

            public void ClearReceivedCalls()
            {
                propertyChangedHandler.ClearReceivedCalls();
                modelChangedHandler.ClearReceivedCalls();
            }
        }

        #endregion Test Context

        /**********************************************************************/
        #region Minimum Tests

        [TestCase(0)]
        [TestCase(1)]
        public void Minimum_Always_SetsToGiven(int minimum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.Minimum = minimum;

            uut.Minimum.ShouldBe(minimum);
        }

        [TestCase(0, 1, 0)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 1, 2)]
        public void Minimum_DoesNotEqualValue_InvokesRaisePropertyChangedWithMinimum(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.propertyChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(0, 1, 0)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 1, 2)]
        public void Minimum_DoesNotEqualValue_ThisMaximumIsEmpty(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            uut.Minimum = minimum;

            uut[nameof(uut.Maximum)].ShouldBeEmpty();
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 1, 2)]
        public void Minimum_DoesNotEqualValueAndValueIsLessThanOrEqualToMaximum_InvokesModelSet(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            var previousModel = uut.Model;
            var model = new MinMaxPair<int>(minimum, maximum);

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.modelChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs<MinMaxPair<int>>>(x => (x.OldValue == previousModel) && (x.NewValue == model)));
        }

        [TestCase(0, 1, 1)]
        [TestCase(0, 1, 2)]
        public void Minimum_DoesNotEqualValueAndValueIsLessThanOrEqualToMaximum_ThisMinimumIsEmpty(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            uut.Minimum = minimum;

            uut[nameof(uut.Minimum)].ShouldBeEmpty();
        }

        [TestCase(0, 1, 0)]
        public void Minimum_DoesNotEqualValueAndValueIsGreaterThanMaximum_DoesNotInvokeModelSet(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.modelChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs<MinMaxPair<int>>>());
        }

        [TestCase(0, 1, 0)]
        public void Minimum_DoesNotEqualValueAndValueIsGreaterThanMaximum_ThisMinimumIsNotNullOrEmpty(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            uut.Minimum = minimum;

            uut[nameof(uut.Minimum)].ShouldNotBeNullOrEmpty();
        }

        [TestCase(2, 1, 1)]
        public void Minimum_DoesNotEqualValueAndThisMaximumIsNotEmpty_InvokesRaisePropertyChangedWithMaximum(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.propertyChangedHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        [TestCase(0, 1, 1)]
        public void Minimum_DoesNotEqualValueAndThisMaximumIsEmpty_DoesNotInvokeRaisePropertyChangedWithMaximum(int previousMinimum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = maximum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void Minimum_EqualsValue_DoesNotInvokeRaisePropertyChanged(int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = minimum;
            uut.Maximum = maximum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs>());
        }

        #endregion Minimum Tests

        /**********************************************************************/
        #region Maximum Tests

        [TestCase(0)]
        [TestCase(1)]
        public void Maximum_Always_SetsToGiven(int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            uut.Maximum = maximum;

            uut.Maximum.ShouldBe(maximum);
        }

        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        public void Maximum_DoesNotEqualValue_InvokesRaisePropertyChangedWithMaximum(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            context.ClearReceivedCalls();

            uut.Maximum = maximum;

            context.propertyChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 2, 1)]
        public void Maximum_DoesNotEqualValue_ThisMinimumIsEmpty(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            uut.Maximum = maximum;

            uut[nameof(uut.Minimum)].ShouldBeEmpty();
        }

        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        public void Maximum_DoesNotEqualValueAndValueIsGreaterThanOrEqualToMinimum_InvokesModelSet(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            var previousModel = uut.Model;
            var model = new MinMaxPair<int>(minimum, maximum);

            context.ClearReceivedCalls();

            uut.Maximum = maximum;

            context.modelChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs<MinMaxPair<int>>>(x => (x.OldValue == previousModel) && (x.NewValue == model)));
        }

        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 1)]
        public void Maximum_DoesNotEqualValueAndValueIsGreaterThanOrEqualToMinimum_ThisMaximumIsEmpty(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            uut.Maximum = maximum;

            uut[nameof(uut.Maximum)].ShouldBeEmpty();
        }

        [TestCase(0, 2, 1)]
        public void Maximum_DoesNotEqualValueAndValueIsLessThanMinimum_DoesNotInvokeModelSet(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            context.ClearReceivedCalls();

            uut.Minimum = minimum;

            context.modelChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs<MinMaxPair<int>>>());
        }

        [TestCase(0, 2, 1)]
        public void Maximum_DoesNotEqualValueAndValueIsLessThanMinimum_ThisMaximumIsNotNullOrEmpty(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            uut.Maximum = maximum;

            uut[nameof(uut.Maximum)].ShouldNotBeNullOrEmpty();
        }

        [TestCase(1, 2, 2)]
        public void Maximum_DoesNotEqualValueAndThisMinimumIsNotEmpty_InvokesRaisePropertyChangedWithMinimum(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            context.ClearReceivedCalls();

            uut.Maximum = maximum;

            context.propertyChangedHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(1, 1, 2)]
        public void Maximum_DoesNotEqualValueAndThisMinimumIsEmpty_DoesNotInvokeRaisePropertyChangedWithMinimum(int previousMaximum, int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = minimum;

            context.ClearReceivedCalls();

            uut.Maximum = maximum;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public void Maximum_EqualsValue_DoesNotInvokeRaisePropertyChanged(int minimum, int maximum)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = maximum;
            uut.Minimum = minimum;

            context.ClearReceivedCalls();

            uut.Maximum = maximum;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs>());
        }

        #endregion Maximum Tests

        /**********************************************************************/
        #region base.ModelChanged Tests

        [TestCase(1, -1, 1, 1)]
        public void BaseModelChanged_ModelDoesNotEqualNewValueAndThisMinimumIsNotNullOrEmpty_ThisMinimumIsEmpty(int previousMinimum, int previousMaximum, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = previousMaximum;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            uut.Model = model;

            uut[nameof(uut.Minimum)].ShouldBeEmpty();
        }

        [TestCase(1, -1, 1, 1)]
        public void BaseModelChanged_ModelDoesNotEqualNewValueAndThisMinimumIsNotNullOrEmptyAndNewValueMinEqualsMinimum_InvokesRaisePropertyChangedWithMinimum(int previousMinimum, int previousMaximum, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Maximum = previousMaximum;
            uut.Minimum = previousMinimum;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            context.propertyChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(1, -1, -1, -1)]
        public void BaseModelChanged_ModelDoesNotEqualNewValueAndThisMaximumIsNotNullOrEmpty_SetsThisMaximumToEmpty(int previousMinimum, int previousMaximum, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = previousMaximum;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            uut.Model = model;

            uut[nameof(uut.Maximum)].ShouldBeEmpty();
        }

        [TestCase(1, -1, -1, -1)]
        public void BaseModelChanged_ModelDoesNotEqualNewValueAndThisMaximumIsNotNullOrEmptyAndNewValueMaxEqualsMaximum_InvokesRaisePropertyChangedWithMaximum(int previousMinimum, int previousMaximum, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();
            uut.Minimum = previousMinimum;
            uut.Maximum = previousMaximum;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            context.propertyChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        [TestCase(0, 0, 0, 1)]
        public void BaseModelChanged_NewValueMinEqualsMinimum_DoesNotInvokeRaisePropertyChangedWithMinimum(int previousModelMin, int previousModelMax, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var previousModel = new MinMaxPair<int>(previousModelMin, previousModelMax);
            uut.Model = previousModel;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(0, 0,  1, 1)]
        [TestCase(0, 0, -1, 0)]
        public void BaseModelChanged_NewValueMinDoesNotEqualMinimum_InvokesRaisePropertyChangedWithMinimum(int previousModelMin, int previousModelMax, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var previousModel = new MinMaxPair<int>(previousModelMin, previousModelMax);
            uut.Model = previousModel;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            // Do not require event to raise only once. It may raise multiple times, due to changes in both the property value, and its error message.
            context.propertyChangedHandler.Received().Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Minimum)));
        }

        [TestCase(0, 0, -1, 0)]
        public void BaseModelChanged_NewValueMaxEqualsMaximum_DoesNotInvokeRaisePropertyChangedWithMaximum(int previousModelMin, int previousModelMax, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var previousModel = new MinMaxPair<int>(previousModelMin, previousModelMax);
            uut.Model = previousModel;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            context.propertyChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        [TestCase(0, 0, 1, 1)]
        [TestCase(0, 0, 0, 1)]
        public void BaseModelChanged_NewValueMaxDoesNotEqualMaximum_InvokesRaisePropertyChangedWithMaximum(int previousModelMin, int previousModelMax, int modelMin, int modelMax)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var previousModel = new MinMaxPair<int>(previousModelMin, previousModelMax);
            uut.Model = previousModel;

            var model = new MinMaxPair<int>(modelMin, modelMax);

            context.ClearReceivedCalls();

            uut.Model = model;

            // Do not require event to raise only once. It may raise multiple times, due to changes in both the property value, and its error message.
            context.propertyChangedHandler.Received().Invoke(uut, Arg.Is<PropertyChangedEventArgs>(x => x.PropertyName == nameof(uut.Maximum)));
        }

        #endregion base.ModelChanged Tests
    }
}

using System;
using System.ComponentModel;
using System.Reflection;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Observation;

using JV.Utilities.Wpf.Mvvm;

namespace JV.Utilities.Wpf.Tests.Mvvm
{
    [TestFixture]
    public class ModelViewModelBaseTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<TModel>
        {
            public TestContext()
            {
                propertyChangedHandler = Substitute.For<PropertyChangedEventHandler>();
                modelChangedHandler = Substitute.For<EventHandler<PropertyChangedEventArgs<TModel>>>();
            }

            public PropertyChangedEventHandler propertyChangedHandler;
            public EventHandler<PropertyChangedEventArgs<TModel>> modelChangedHandler;

            public ModelViewModelBase<TModel> ConstructUUT()
            {
                try
                {
                    var uut = Substitute.ForPartsOf<ModelViewModelBase<TModel>>();

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
        #region Model Tests

        [TestCase(null)]
        [TestCase("")]
        [TestCase("model")]
        public void ModelSet_Always_SetsModelToGiven(string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = model;

            uut.Model.ShouldBe(model);
        }

        [TestCase("A", "B")]
        public void ModelSet_ModelDoesNotEqualGivenAndModelChangedIsNull_DoesNotThrowException(string previousModel, string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = previousModel;

            uut.ModelChanged -= context.modelChangedHandler;

            Should.NotThrow(() =>
            {
                uut.Model = model;
            });
        }

        [TestCase("A", "B")]
        public void ModelSet_ModelDoesNotEqualGivenAndModelChangedIsNotNull_RaisesModelChanged(string previousModel, string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = previousModel;

            context.ClearReceivedCalls();

            uut.Model = model;

            context.modelChangedHandler.Received(1).Invoke(uut, Arg.Is<PropertyChangedEventArgs<string>>(x => (x.OldValue == previousModel) && (x.NewValue == model)));
        }

        [TestCase("model")]
        public void Load_ModelEqualsGiven_DoesNotRaiseModelChanged(string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = model;

            context.ClearReceivedCalls();

            uut.Model = model;

            context.modelChangedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<PropertyChangedEventArgs<string>>());
        }

        #endregion Model Tests
    }
}

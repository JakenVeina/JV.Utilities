using System;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

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
            public ModelViewModelBase<TModel> ConstructUUT()
                => Substitute.ForPartsOf<ModelViewModelBase<TModel>>();
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
        public void ModelSet_ModelDoesNotEqualGiven_InvokesOnModelChanged(string previousModel, string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = previousModel;
            uut.ClearReceivedCalls();

            uut.Model = model;

            uut.Received(1).OnModelChanged(model);
        }

        [TestCase("A", "B")]
        public void ModelSet_ModelDoesNotEqualGiven_RaisesModelChanged(string previousModel, string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = previousModel;

            var handler = Substitute.For<EventHandler>();
            uut.ModelChanged += handler;

            uut.Model = model;

            handler.Received(1).Invoke(uut, EventArgs.Empty);
        }

        [TestCase("model")]
        public void Load_ModelEqualsGiven_DoesNotInvokeOnModelChanged(string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = model;

            uut.ClearReceivedCalls();

            uut.Model = model;

            uut.DidNotReceive().OnModelChanged(Arg.Any<string>());
        }

        [TestCase("model")]
        public void Load_ModelEqualsGiven_DoesNotRaiseModelChanged(string model)
        {
            var context = new TestContext<string>();
            var uut = context.ConstructUUT();

            uut.Model = model;

            var handler = Substitute.For<EventHandler>();
            uut.ModelChanged += handler;

            uut.Model = model;

            handler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
        }

        #endregion Model Tests
    }
}

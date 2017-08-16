using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Collections;
using JV.Utilities.Wpf.Collections.Interfaces;

namespace JV.Utilities.Wpf.Tests.Collections
{
    [TestFixture]
    public class ReadOnlyObservableCollectionTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext<T>
        {
            public TestContext()
            {
                source = Substitute.For<IObservableCollection<T>>();
            }

            public IObservableCollection<T> source;

            public ReadOnlyObservableCollection<T> ConstructUUT()
                => new ReadOnlyObservableCollection<T>(source);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor Tests

        [Test]
        public void Constructor_SourceIsNull_ThrowsException()
        {
            var context = new TestContext<int>()
            {
                source = null
            };

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                var uut = context.ConstructUUT();
            });

            result.ParamName.ShouldBe(nameof(context.source));            
        }

        #endregion Constructor Tests

        /**********************************************************************/
        #region CollectionChanged Tests

        [Test]
        public void CollectionChangedAdd_Always_InvokesSourceCollectionChangedAdd()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var handler = Substitute.For<NotifyCollectionChangedEventHandler>();

            uut.CollectionChanged += handler;

            context.source.Received(1).CollectionChanged += handler;
        }

        [Test]
        public void CollectionChangedRemove_Always_InvokesSourceCollectionChangedRemove()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var handler = Substitute.For<NotifyCollectionChangedEventHandler>();

            uut.CollectionChanged -= handler;

            context.source.Received(1).CollectionChanged -= handler;
        }

        #endregion CollectionChanged Tests

        /**********************************************************************/
        #region PropertyChanged Tests

        [Test]
        public void PropertyChangedAdd_Always_InvokesSourcePropertyChangedAdd()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var handler = Substitute.For<PropertyChangedEventHandler>();

            uut.PropertyChanged += handler;

            context.source.Received(1).PropertyChanged += handler;
        }

        [Test]
        public void PropertyChangedRemove_Always_InvokesSourcePropertyChangedRemove()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var handler = Substitute.For<PropertyChangedEventHandler>();

            uut.PropertyChanged -= handler;

            context.source.Received(1).PropertyChanged -= handler;
        }

        #endregion PropertyChanged Tests

        /**********************************************************************/
        #region this[] Tests

        [Test, Combinatorial]
        public void This_Always_ReturnsSourceThis([Values(-1, 0, 1)] int index, [Values(-1, 0, 1)] int sourceThis)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            context.source[index].Returns(sourceThis);

            uut[index].ShouldBe(sourceThis);
        }

        #endregion this[] Tests

        /**********************************************************************/
        #region Count Tests

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void Count_Always_ReturnsSourceCount(int count)
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            context.source.Count.Returns(count);

            uut.Count.ShouldBe(count);
        }

        #endregion Count Tests

        /**********************************************************************/
        #region GetEnumerator Tests

        [Test]
        public void GetEnumerator_Generic_ReturnsSourceGetEnumerator()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var enumerator = Substitute.For<IEnumerator<int>>();

            context.source.GetEnumerator().Returns(enumerator);

            uut.GetEnumerator().ShouldBeSameAs(enumerator);
        }

        [Test]
        public void GetEnumerator_NonGeneric_ReturnsSourceGetEnumerator()
        {
            var context = new TestContext<int>();
            var uut = context.ConstructUUT();

            var enumerator = Substitute.For<IEnumerator>();

            (context.source as IEnumerable).GetEnumerator().Returns(enumerator);

            (uut as IEnumerable).GetEnumerator().ShouldBeSameAs(enumerator);
        }

        #endregion GetEnumerator Tests
    }
}

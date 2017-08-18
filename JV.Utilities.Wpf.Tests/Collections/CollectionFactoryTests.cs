using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Wpf.Collections;

namespace JV.Utilities.Wpf.Tests.Collections
{
    [TestFixture]
    public class CollectionFactoryTests
    {
        /**********************************************************************/
        #region Default Tests

        [Test]
        public void Default_ByDefault_IsNotNull()
        {
            CollectionFactory.Default.ShouldNotBeNull();
        }

        #endregion Default Tests
        
        /**********************************************************************/
        #region CreateObservableCollection() Tests

        [Test]
        public void CreateObservableCollection_Default_Always_ReturnsObservableCollectionConstructorDefault()
        {
            var uut = new CollectionFactory();

            var result = uut.CreateObservableCollection<int>();

            result.ShouldBeEmpty();
        }

        #endregion CreateObservableCollection() Tests

        /**********************************************************************/
        #region CreateObservableCollection(list) Tests

        [TestCase()]
        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void CreateObservableCollection_List_Always_ReturnsObservableCollectionConstructorList(params string[] items)
        {
            var uut = new CollectionFactory();

            var list = new List<string>(items);

            var result = uut.CreateObservableCollection(list);

            result.ShouldBeOrderedEquivalentTo(list);
        }

        #endregion CreateObservableCollection(list) Tests

        /**********************************************************************/
        #region CreateObservableCollection(collection) Tests

        [TestCase()]
        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void CreateObservableCollection_Collection_Always_ReturnsObservableCollectionConstructorCollection(params string[] items)
        {
            var uut = new CollectionFactory();

            var collection = items.AsEnumerable();

            var result = uut.CreateObservableCollection(collection);

            result.ShouldBeOrderedEquivalentTo(collection);
        }

        #endregion CreateObservableCollection(collection) Tests

        /**********************************************************************/
        #region CreateReadOnlyObservableCollection(source) Tests

        [TestCase()]
        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void CreateReadOnlyObservableCollection_Collection_Always_ReturnsObservableCollectionConstructorCollection(params string[] items)
        {
            var uut = new CollectionFactory();

            var source = new ObservableCollection<string>();

            var result = uut.CreateReadOnlyObservableCollection(source);

            foreach (var item in items)
                source.Add(item);

            result.ShouldBeOrderedEquivalentTo(source);
        }

        #endregion CreateReadOnlyObservableCollection(source) Tests

        /**********************************************************************/
        #region CreateCollectionView Tests

        [Test]
        public void CreateCollectionView_Always_ReturnsNewCollectionViewSourceView()
        {
            var uut = new CollectionFactory();

            var source = new ObservableCollection<string>();

            var result = uut.CreateCollectionView(source);

            result.ShouldSatisfyAllConditions(
                () => result.SourceCollection.ShouldBeSameAs(source),
                () => result.ShouldNotBeSameAs(CollectionViewSource.GetDefaultView(source)));
        }

        #endregion CreateCollectionView Tests
    }
}

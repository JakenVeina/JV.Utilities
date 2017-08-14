using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Wpf.Utilities.Collections;

namespace JV.Wpf.Utilities.Tests.CollectionsTests
{
    [TestFixture]
    public class ObservableCollectionTests
    {
        /**********************************************************************/
        #region Constructor() Tests

        [Test]
        public void Constructor_Default_Always_InvokesBaseConstructor()
        {
            var uut = new ObservableCollection<string>();

            uut.ShouldBeEmpty();
        }

        #endregion Constructor() Tests

        /**********************************************************************/
        #region Constructor(list) Tests

        [TestCase()]
        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void Constructor_List_Always_InvokesBaseConstructor(params string[] items)
        {
            var list = new List<string>(items);

            var uut = new ObservableCollection<string>(list);

            uut.ShouldBeOrderedEquivalentTo(list);
        }

        #endregion Constructor(list) Tests

        /**********************************************************************/
        #region Constructor(collection) Tests

        [TestCase()]
        [TestCase("A")]
        [TestCase("A", "B", "C")]
        public void Constructor_Collection_Tests(params string[] items)
        {
            var collection = items.AsEnumerable();

            var uut = new ObservableCollection<string>(collection);

            uut.ShouldBeOrderedEquivalentTo(collection);
        }

        #endregion Constructor(collection) Tests
    }
}

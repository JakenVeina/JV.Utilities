using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Observation;

namespace JV.Utilities.Tests.Observation
{
    [TestFixture]
    public partial class IndexedPropertyChangedEventTests
    {
        /**********************************************************************/
        #region Constructor Tests

        [TestCase(-1, 3, 4)]
        [TestCase(0, 3, 4)]
        [TestCase(1, 3, 4)]
        public void Constructor_Always_SetsIndex(int index, int oldValue, int newValue)
        {
            var uut = new IndexedPropertyChangedEventArgs<int, int>(index, oldValue, newValue);

            uut.Index.ShouldBe(index);
        }

        [TestCase(2, -1, 4)]
        [TestCase(2, 0, 4)]
        [TestCase(2, 1, 4)]
        public void Constructor_Always_SetsOldValue(int index, int oldValue, int newValue)
        {
            var uut = new IndexedPropertyChangedEventArgs<int, int>(index, oldValue, newValue);

            uut.OldValue.ShouldBe(oldValue);
        }

        [TestCase(2, 3, -1)]
        [TestCase(2, 3, 0)]
        [TestCase(2, 3, 1)]
        public void Constructor_Always_SetNewValue(int index, int oldValue, int newValue)
        {
            var uut = new IndexedPropertyChangedEventArgs<int, int>(index, oldValue, newValue);

            uut.NewValue.ShouldBe(newValue);
        }

        #endregion Constructor Tests
    }
}

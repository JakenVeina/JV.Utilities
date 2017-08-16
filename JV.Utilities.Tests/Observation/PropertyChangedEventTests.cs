using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Observation;

namespace JV.Utilities.Tests.Observation
{
    [TestFixture]
    public class PropertyChangedEventTests
    {
        /**********************************************************************/
        #region Constructor Tests

        [TestCase(-1, 3)]
        [TestCase(0, 3)]
        [TestCase(1, 3)]
        public void Constructor_Always_SetsOldValue(int oldValue, int newValue)
        {
            var uut = new PropertyChangedEventArgs<int>(oldValue, newValue);

            uut.OldValue.ShouldBe(oldValue);
        }

        [TestCase(2, -1)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        public void Constructor_Always_SetNewValue(int oldValue, int newValue)
        {
            var uut = new PropertyChangedEventArgs<int>(oldValue, newValue);

            uut.NewValue.ShouldBe(newValue);
        }

        #endregion Constructor Tests
    }
}

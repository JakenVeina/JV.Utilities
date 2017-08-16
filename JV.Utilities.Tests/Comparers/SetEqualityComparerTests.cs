using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;
using JV.Utilities.Comparers;

namespace JV.Utilities.Tests.Comparers
{
    [TestFixture]
    public class SetEqualityComparerTests
    {
        /**********************************************************************/
        #region Test Data

        private static HashSet<string> MakeTestSet(string testSetString)
        {
            if (testSetString == null)
                return null;

            if (testSetString == string.Empty)
                return new HashSet<string>();

            return new HashSet<string>(testSetString.Split(',').Select(x => x.Trim()));
        }

        #endregion Test Data

        /**********************************************************************/
        #region Default Tests

        [Test]
        public void Default_Always_IsNotNull()
        {
            SetEqualityComparer<string>.Default.ShouldNotBeNull();
        }

        #endregion Default Tests

        /**********************************************************************/
        #region Equals Tests

        [TestCase(null)]
        [TestCase("")]
        [TestCase("A")]
        [TestCase("A, B, C")]
        public void Equals_XIsSameAsY_ReturnsTrue(string xString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);
            var y = x;

            uut.Equals(x, y).ShouldBeTrue();
        }

        [TestCase("", "")]
        [TestCase("A", "A")]
        [TestCase("A, B, C", "A, B, C")]
        [TestCase("A, B, C", "C, B, A")]
        [TestCase("C, B, A", "A, B, C")]
        public void Equals_XIsEquivalentToY_ReturnsTrue(string xString, string yString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);
            var y = MakeTestSet(yString);

            uut.Equals(x, y).ShouldBeTrue();
        }

        [TestCase(null, "")]
        [TestCase("", null)]
        [TestCase("A", "")]
        [TestCase("", "A")]
        [TestCase("A", "A, B, C")]
        [TestCase("A, B, C", "A")]
        [TestCase("A, B, C", "A, B, D")]
        [TestCase("A, B, D", "A, B, C")]
        public void Equals_XIsNotEquivalentToY_ReturnsFalse(string xString, string yString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);
            var y = MakeTestSet(yString);

            uut.Equals(x, y).ShouldBeFalse();
        }

        #endregion Equals Tests

        /**********************************************************************/
        #region GetHashCode Tests

        [Test]
        public void GetHashCode_SetIsNull_ThrowsException()
        {
            var uut = new SetEqualityComparer<string>();;

            var set = (ISet<string>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                uut.GetHashCode(set);
            });

            result.ParamName.ShouldBe(nameof(set));
        }

        [TestCase("")]
        [TestCase("A")]
        [TestCase("A, B, C")]
        public void GetHashCode_ForSameObjects_ResultsAreEqual(string xString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);

            uut.GetHashCode(x).ShouldBe(uut.GetHashCode(x));
        }


        [TestCase("", "")]
        [TestCase("A", "A")]
        [TestCase("A, B, C", "A, B, C")]
        [TestCase("A, B, C", "C, B, A")]
        [TestCase("C, B, A", "A, B, C")]
        public void GetHashCode_ForEquivalentSets_ResultsAreEqual(string xString, string yString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);
            var y = MakeTestSet(yString);

            uut.GetHashCode(x).ShouldBe(uut.GetHashCode(y));
        }

        [TestCase("A", "")]
        [TestCase("", "A")]
        [TestCase("A", "A, B, C")]
        [TestCase("A, B, C", "A")]
        [TestCase("A, B, C", "A, B, D")]
        [TestCase("A, B, D", "A, B, C")]
        public void GetHashCode_ForInequivalentSets_ResultsAreNotEqual(string xString, string yString)
        {
            var uut = new SetEqualityComparer<string>();;

            var x = MakeTestSet(xString);
            var y = MakeTestSet(yString);

            uut.GetHashCode(x).ShouldNotBe(uut.GetHashCode(y));
        }

        [Test]
        public void GetHashCode_ForRandomData_NoCollisions()
        {
            var uut = SetEqualityComparer<int>.Default;

            var rng = new Random(0);
            var setsAndHashCodes = Enumerable.Range(0, 1000).Select(i => new HashSet<int>(Enumerable.Range(0, 5).Select(j => rng.Next(50))))
                                                            .Select(set => new object[] { set, uut.GetHashCode(set) });

            var collisionCount = 1000 - setsAndHashCodes.Distinct(new DelegateEqualityComparer<object[]>(
                    (x, y) => (((HashSet<int>)x[0] != (HashSet<int>)y[0]) && ((int)x[1] == (int)y[1])),
                    (x) => x.GetHashCode()
                )).Count();

            collisionCount.ShouldBe(0);
        }

        #endregion GetHashCode Tests
    }
}

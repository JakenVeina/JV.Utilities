using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class NullableExtensionsTests
    {
        /**********************************************************************/
        #region AsNullable Tests

        [TestCase(1)]
        public void AsNullable_Always_ResultHasValueIsTrue(int @this)
        {
            @this.AsNullable().HasValue.ShouldBeTrue();
        }

        [TestCase(1)]
        public void AsNullable_Always_ResultValueEqualsThis(int @this)
        {
            @this.AsNullable().Value.ShouldBe(@this);
        }

        #endregion AsNullable Tests

        /**********************************************************************/
        #region CastAsNullable Tests

        [Test]
        public void CastAsNullable_ThisIsNull_ThrowsException()
        {
            var @this = (IEnumerable<int>)null;

            var result = Should.Throw<ArgumentNullException>(() =>
            {
                @this.CastAsNullable();
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(5)]
        public void CastAsNullable_Always_EachResultHasValueIsTrue(int count)
        {
            var @this = Enumerable.Range(1, count);

            @this.CastAsNullable().Select(x => x.HasValue).ShouldAllBe(x => (x == true));
        }

        [TestCase(5)]
        public void AsNullable_Always_EachResultValueEqualsEachThis(int count)
        {
            var @this = Enumerable.Range(1, count);

            @this.CastAsNullable().Select(x => x.Value).ShouldBeOrderedEquivalentTo(@this);
        }

        #endregion AsNullable Tests
    }
}

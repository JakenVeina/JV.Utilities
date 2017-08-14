using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shouldly;

namespace JV.Wpf.Utilities.Tests
{
    public static class ShouldlyExtensions
    {
        public static void ShouldBeSetEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
            => actual.ShouldBe(expected, true);

        public static void ShouldBeSetEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string customMessage)
            => actual.ShouldBe(expected, true, customMessage);

        public static void ShouldBeSetEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<string> customMessage)
            => actual.ShouldBe(expected, true, customMessage);

        public static void ShouldBeOrderedEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
            => actual.ShouldBe(expected, false);

        public static void ShouldBeOrderedEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string customMessage)
            => actual.ShouldBe(expected, false, customMessage);

        public static void ShouldBeOrderedEquivalentTo<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<string> customMessage)
            => actual.ShouldBe(expected, false, customMessage);

    }
}

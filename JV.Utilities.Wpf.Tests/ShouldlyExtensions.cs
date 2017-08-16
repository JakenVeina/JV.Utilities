using System;
using System.Collections.Generic;
using System.Text;

using Shouldly;

namespace JV.Utilities.Wpf.Tests
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

        public static void ShouldSatisfyAnyCondition(this object actual, params Action[] conditions)
        {
            var messageBuilder = new StringBuilder("Object should have satisfied one of the following criteria:");
            foreach (var condition in conditions)
            {
                try
                {
                    condition.Invoke();
                    return;
                }
                catch (ShouldAssertException ex)
                {
                    messageBuilder.Append("\n\n");
                    messageBuilder.Append(ex.Message);
                }
            }

            throw new ShouldAssertException(messageBuilder.ToString());
        }

        public static void ShouldSatisfyAnyCondition(this object actual, string customMessage, params Action[] conditions)
        {
            foreach (var condition in conditions)
            {
                try
                {
                    condition.Invoke();
                    return;
                }
                catch (ShouldAssertException) { }
            }

            throw new ShouldAssertException(customMessage);
        }

        public static void ShouldSatisfyAnyCondition(this object actual, Func<string> customMessage, params Action[] conditions)
        {
            foreach (var condition in conditions)
            {
                try
                {
                    condition.Invoke();
                    return;
                }
                catch (ShouldAssertException) { }
            }

            throw new ShouldAssertException(customMessage.Invoke());
        }
    }
}

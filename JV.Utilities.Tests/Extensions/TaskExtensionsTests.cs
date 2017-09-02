using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Extensions;

namespace JV.Utilities.Tests.Extensions
{
    [TestFixture]
    public class TaskExtensionsTests
    {
        /**********************************************************************/
        #region WhenAny Tests

        [Test]
        public void WhenAny_ThisIsNull_ThrowsException()
        {
            var @this = null as IEnumerable<Task<int>>;
            var predicate = new Predicate<int>(x => true);

            var result = Should.Throw<ArgumentNullException>(async () =>
            {
                await @this.WhenAny(predicate);
            });

            result.ParamName.ShouldBe(nameof(@this));
        }

        [TestCase(1)]
        [TestCase(5)]
        public void WhenAny_PredicateIsNull_ThrowsException(int taskCount)
        {
            var @this = Enumerable.Range(1, taskCount)
#if NET40
                                  .Select(x => TaskEx.FromResult(x))
#else
                                  .Select(x => Task.FromResult(x))
#endif
                                  .ToArray();
            var predicate = null as Predicate<int>;

            var result = Should.Throw<ArgumentNullException>(async () =>
            {
                await @this.WhenAny(predicate);
            });

            result.ParamName.ShouldBe(nameof(predicate));
        }

        [Test]
        public void WhenAny_ThisIsEmpty_ThrowsException()
        {
            var @this = Enumerable.Empty<Task<int>>();
            var predicate = new Predicate<int>(x => true);

            var result = Should.Throw<ArgumentException>(async () =>
            {
                await @this.WhenAny(predicate);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(@this)),
                () => result.Message.ShouldContain(nameof(predicate)));
        }

        [TestCase(1)]
        [TestCase(5)]
        public void WhenAny_PredicateReturnsFalseAndThisEachIsCompletedIsTrue_ThrowsException(int taskCount)
        {
            var @this = Enumerable.Range(0, taskCount)
#if NET40
                                  .Select(x => TaskEx.FromResult(x))
#else
                                  .Select(x => Task.FromResult(x))
#endif
                                  .ToArray();
            var predicate = new Predicate<int>(x => false);

            var result = Should.Throw<ArgumentException>(async () =>
            {
                await @this.WhenAny(predicate);
            });

            result.ShouldSatisfyAllConditions(
                () => result.ParamName.ShouldBe(nameof(@this)),
                () => result.Message.ShouldContain(nameof(predicate)));
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public void WhenAny_PredicateReturnsFalseAndThisAnyIsCompletedIsFalse_AwaitsThisEach(int taskCount, int isCompletedFalseIndex)
        {
            var taskSources = Enumerable.Repeat(0, taskCount)
                                        .Select(x => new TaskCompletionSource<int>())
                                        .ToArray();

            for (var i = 0; i < taskSources.Length; ++i)
                if (i != isCompletedFalseIndex)
                    taskSources[i].SetResult(i);

            var @this = taskSources.Select(x => x.Task)
                                   .ToArray();
            var predicate = new Predicate<int>(x => false);

            var result = @this.WhenAny(predicate);

            result.IsCompleted.ShouldBeFalse();

            taskSources[isCompletedFalseIndex].SetResult(isCompletedFalseIndex);

            result.IsCompleted.ShouldBeTrue();
        }

        [TestCase(1, 0)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        public async Task WhenAny_ThisEachIsCompletedIsTrue_ReturnsMatchingResult(int taskCount, int desiredResult)
        {
            var @this = Enumerable.Range(0, taskCount)
#if NET40
                                  .Select(x => TaskEx.FromResult(x))
#else
                                  .Select(x => Task.FromResult(x))
#endif
                                  .ToArray();
            var predicate = new Predicate<int>(x => x == desiredResult);

            (await @this.WhenAny(predicate)).ShouldBe(desiredResult);
        }

        [TestCase(0)]
        [TestCase(0, 1, 2, 3, 4)]
        [TestCase(4, 3, 2, 1, 0)]
        [TestCase(1, 3, 0, 2, 4)]
        [TestCase(0, 2, 4, 1, 3)]
        public async Task WhenAny_ThisLastToCompleteMatchesPredicate_ReturnsMatchingResult(params int[] taskCompletionOrderIndices)
        {
            var taskSources = Enumerable.Repeat(0, taskCompletionOrderIndices.Length)
                                        .Select(x => new TaskCompletionSource<int>())
                                        .ToArray();

            var desiredResult = taskCompletionOrderIndices.Last();

            var @this = taskSources.Select(x => x.Task)
                                   .ToArray();
            var predicate = new Predicate<int>(x => x == desiredResult);

            var result = @this.WhenAny(predicate);

            foreach (var index in taskCompletionOrderIndices)
                taskSources[index].SetResult(index);

            (await result).ShouldBe(desiredResult);
        }

#endregion WhenAny Tests
    }
}

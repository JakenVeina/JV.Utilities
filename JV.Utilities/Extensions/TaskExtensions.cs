using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Contains extension methods for operating on <see cref="Task"/> objects.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// For a given set of <see cref="Task{T}"/> objects, returns the result of the first task to complete,
        /// whose result matches the given <see cref="Predicate{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of result of each given <see cref="Task{T}"/>.</typeparam>
        /// <param name="this">The set of <see cref="Task{T}"/> objects to be awaited.</param>
        /// <param name="predicate">A <see cref="Predicate{T}"/> delegate to select the <see cref="Task{T}.Result"/> value to be returned.</param>
        /// <exception cref="ArgumentNullException">Throws for <paramref name="this"/> and <paramref name="predicate"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Throws if <paramref name="this"/> is empty,
        /// or if none of its tasks return a <see cref="Task{T}.Result"/> value that matches <paramref name="predicate"/>.
        /// </exception>
        /// <returns>The first received <see cref="Task{T}.Result"/> value matched by <paramref name="predicate"/>.</returns>
        public static async Task<T> WhenAny<T>(this IEnumerable<Task<T>> @this, Predicate<T> predicate)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var taskList = new LinkedList<Task<T>>(@this);

            while(taskList.Any())
            {
#if NET40
                await TaskEx.WhenAny(taskList);
#else
                await Task.WhenAny(taskList);
#endif

                var taskNode = taskList.First;
                while(taskNode != null)
                {
                    var next = taskNode.Next;

                    if (taskNode.Value.IsCompleted)
                    {
                        var result = taskNode.Value.Result;

                        if (predicate.Invoke(result))
                            return result;

                        taskList.Remove(taskNode);
                    }

                    taskNode = next;
                }
            }

            throw new ArgumentException($"None of the given tasks returned a result matching {predicate}", nameof(@this));
        }
    }
}

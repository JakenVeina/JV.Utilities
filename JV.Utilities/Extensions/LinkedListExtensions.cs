using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="LinkedList{T}"/>.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Iterates a given <see cref="LinkedList{T}"/>, removing items that meet the given criteria, during the iteration.
        /// </summary>
        /// <typeparam name="T">The type of the enumerated items.</typeparam>
        /// <param name="this">The list to be iterated.</param>
        /// <param name="criteria">A <see cref="Predicate{T}"/> to identify items in the list to be removed.</param>
        /// <returns>The number of items that were removed.</returns>
        public static int RemoveWhere<T>(this LinkedList<T> @this, Predicate<T> criteria)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            int removedNodesCount = 0;
            var currentNode = @this.First;
            while(currentNode != null)
            {
                if(criteria.Invoke(currentNode.Value))
                {
                    var nextNode = currentNode.Next;
                    @this.Remove(currentNode);
                    ++removedNodesCount;
                    currentNode = nextNode;
                }
                else
                    currentNode = currentNode.Next;
            }

            return removedNodesCount;
        }

        /// <summary>
        /// Iterates a given <see cref="LinkedList{T}"/>, in reverse, removing items that meet the given criteria, during the iteration.
        /// </summary>
        /// <typeparam name="T">The type of the enumerated items.</typeparam>
        /// <param name="this">The list to be iterated.</param>
        /// <param name="criteria">A <see cref="Predicate{T}"/> to identify items in the list to be removed.</param>
        /// <returns>The number of items that were removed.</returns>
        public static int ReverseRemoveWhere<T>(this LinkedList<T> @this, Predicate<T> criteria)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            int removedNodesCount = 0;
            var currentNode = @this.Last;
            while (currentNode != null)
            {
                if (criteria.Invoke(currentNode.Value))
                {
                    var previousNode = currentNode.Previous;
                    @this.Remove(currentNode);
                    ++removedNodesCount;
                    currentNode = previousNode;
                }
                else
                    currentNode = currentNode.Previous;
            }

            return removedNodesCount;
        }
    }
}

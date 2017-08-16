using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Observation
{
    /// <summary>
    /// Contains information about a change that occurred to an indexed object property.
    /// </summary>
    /// <typeparam name="TIndex">The index type of the property that changed.</typeparam>
    /// <typeparam name="TValue">The value type of the property that changed.</typeparam>
    public class IndexedPropertyChangedEventArgs<TIndex, TValue> : EventArgs
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Constructs a new set of args with the given property values.
        /// </summary>
        /// <param name="index">The value to use for <see cref="Index"/>.</param>
        /// <param name="oldValue">The value to use for <see cref="OldValue"/>.</param>
        /// <param name="newValue">The value to use for <see cref="NewValue"/>.</param>
        public IndexedPropertyChangedEventArgs(TIndex index, TValue oldValue, TValue newValue)
        {
            Index = index;
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The index for which the change occurred.
        /// </summary>
        public TIndex Index { get; private set; }

        /// <summary>
        /// The value of the property before the change occurred.
        /// </summary>
        public TValue OldValue { get; private set; }

        /// <summary>
        /// The value of the property after the change occurred.
        /// </summary>
        public TValue NewValue { get; private set; }

        #endregion Properties
    }
}

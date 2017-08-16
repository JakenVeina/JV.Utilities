using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JV.Utilities.Observation
{
    /// <summary>
    /// Contains information about a change that occurred to an object property.
    /// </summary>
    /// <typeparam name="T">The type of the property that changed.</typeparam>
    public class PropertyChangedEventArgs<T> : EventArgs
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// Constructs a new set of args with the given property values.
        /// </summary>
        /// <param name="oldValue">The value to use for <see cref="OldValue"/>.</param>
        /// <param name="newValue">The value to use for <see cref="NewValue"/>.</param>
        public PropertyChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The value of the property before the change occurred.
        /// </summary>
        public T OldValue { get; private set; }

        /// <summary>
        /// The value of the property after the change occurred.
        /// </summary>
        public T NewValue { get; private set; }

        #endregion Properties
    }
}

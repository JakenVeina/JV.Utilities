using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JV.Utilities.Exceptions
{
    /// <summary>
    /// Encapsulates an error that occurred during validation of an object's properties.
    /// </summary>
    [Serializable]
    public class PropertyValidationException : Exception
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// See <see cref="Exception"/>.
        /// </summary>
        public PropertyValidationException() : base() { }

        /// <summary>
        /// See <see cref="Exception"/>.
        /// </summary>
        public PropertyValidationException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        public PropertyValidationException(string propertyName) : base()
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        public PropertyValidationException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        /// <param name="innerException">See <see cref="Exception"/>.</param>
        public PropertyValidationException(string propertyName, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        /// <param name="invalidValue">The value to use for <see cref="InvalidValue"/>.</param>
        public PropertyValidationException(string propertyName, object invalidValue) : base()
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        /// <param name="invalidValue">The value to use for <see cref="InvalidValue"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        public PropertyValidationException(string propertyName, object invalidValue, string message) : base(message)
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }

        /// <summary>
        /// Creates a new exception with the given property values.
        /// </summary>
        /// <param name="propertyName">The value to use for <see cref="PropertyName"/>.</param>
        /// <param name="invalidValue">The value to use for <see cref="InvalidValue"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        /// <param name="innerException">See <see cref="Exception"/>.</param>
        public PropertyValidationException(string propertyName, object invalidValue, string message, Exception innerException) : base(message, innerException)
        {
            PropertyName = propertyName;
            InvalidValue = invalidValue;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The name of the property for which validation failed.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The value of the property that failed validation.
        /// </summary>
        public virtual object InvalidValue { get; private set; }

        #endregion Properties

        /**********************************************************************/
        #region ISerializable

        /// <summary>
        /// See <see cref="ISerializable"/>.
        /// </summary>
        protected internal PropertyValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PropertyName = info.GetString(nameof(PropertyName));
            InvalidValue = info.GetValue(nameof(InvalidValue), typeof(object));
        }

        /// <summary>
        /// See <see cref="ISerializable.GetObjectData(SerializationInfo, StreamingContext)"/>.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(PropertyName), PropertyName, typeof(string));
            info.AddValue(nameof(InvalidValue), InvalidValue, typeof(object));
        }

        #endregion ISerializable
    }
}

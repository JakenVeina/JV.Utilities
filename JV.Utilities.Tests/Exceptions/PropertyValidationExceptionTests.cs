using System;
using System.Runtime.Serialization;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Exceptions;

namespace JV.Utilities.Tests.Exceptions
{
    [TestFixture]
    public class PropertyValidationExceptionTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public TestContext()
            {
                propertyName = nameof(propertyName);
                invalidValue = nameof(invalidValue);
                message = nameof(message);
                innerException = new Exception();

                info = new SerializationInfo(typeof(PropertyValidationException), new FormatterConverter());
                context = new StreamingContext();
                (new Exception(message)).GetObjectData(info, context);
                info.AddValue(nameof(PropertyValidationException.PropertyName), propertyName);
                info.AddValue(nameof(PropertyValidationException.InvalidValue), invalidValue);
            }

            public string propertyName;
            public object invalidValue;
            public string message;
            public Exception innerException;

            public PropertyValidationException ConstructUUT_Default()
                => new PropertyValidationException();

            public PropertyValidationException ConstructUUT_MessageInnerException()
                => new PropertyValidationException(message, innerException);

            public PropertyValidationException ConstructUUT_PropertyName()
                => new PropertyValidationException(propertyName);

            public PropertyValidationException ConstructUUT_PropertyNameMessage()
                => new PropertyValidationException(propertyName, message);

            public PropertyValidationException ConstructUUT_PropertyNameMessageInnerException()
                => new PropertyValidationException(propertyName, message, innerException);

            public PropertyValidationException ConstructUUT_PropertyNameInvalidValue()
                => new PropertyValidationException(propertyName, invalidValue);

            public PropertyValidationException ConstructUUT_PropertyNameInvalidValueMessage()
                => new PropertyValidationException(propertyName, invalidValue, message);

            public PropertyValidationException ConstructUUT_PropertyNameInvalidValueMessageInnerException()
                => new PropertyValidationException(propertyName, invalidValue, message, innerException);

            public SerializationInfo info;
            public StreamingContext context;

            public PropertyValidationException ConstructUUT_ISerializable()
                => new PropertyValidationException(info, context);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor() Tests

        [Test]
        public void Constructor_Default_Always_PropertNameIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.PropertyName.ShouldBeNull();
        }

        [Test]
        public void Constructor_Default_Always_InvalidValueIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.InvalidValue.ShouldBeNull();
        }

        [Test]
        public void Constructor_Default_Always_MessageContainsExceptionType()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_Default_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor() Tests

        /**********************************************************************/
        #region Constructor(message, innerException) Tests

        [Test]
        public void Constructor_MessageInnerException_Always_PropertyNameIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.PropertyName.ShouldBeNull();
        }

        [Test]
        public void Constructor_MessageInnerException_Always_InvalidValueIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.InvalidValue.ShouldBeNull();
        }

        [Test]
        public void Constructor_MessageInnerException_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_MessageInnerException();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_MessageInnerException_MessageIsNotNull_SetsMessage()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_MessageInnerException_Always_SetsInnerException()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.InnerException.ShouldBeSameAs(context.innerException);
        }

        #endregion Constructor(message, innerException) Tests

        /**********************************************************************/
        #region Constructor(propertyName) Tests

        [Test]
        public void Constructor_PropertyName_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyName();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyName_Always_InvalidValueIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyName();

            uut.InvalidValue.ShouldBeNull();
        }

        [Test]
        public void Constructor_PropertyName_Always_MessageContainsExceptionType()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyName();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyName_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyName();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(propertyName) Tests

        /**********************************************************************/
        #region Constructor(propertyName, message) Tests

        [Test]
        public void Constructor_PropertyNameMessage_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessage();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyNameMessage_Always_InvalidValueIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessage();

            uut.InvalidValue.ShouldBeNull();
        }

        [Test]
        public void Constructor_PropertyNameMessage_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_PropertyNameMessage();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyNameMessage_MessageIsNotNull_SetsMessage()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessage();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_PropertyNameMessage_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessage();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(propertyName, message) Tests

        /**********************************************************************/
        #region Constructor(propertyName, message, innerException) Tests

        [Test]
        public void Constructor_PropertyNameMessageInnerException_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessageInnerException();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyNameMessageInnerException_Always_InvalidValueIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessageInnerException();

            uut.InvalidValue.ShouldBeNull();
        }

        [Test]
        public void Constructor_PropertyNameMessageInnerException_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_PropertyNameMessageInnerException();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyNameMessageInnerException_MessageIsNotNull_SetsMessage()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessageInnerException();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_PropertyNameMessageInnerException_Always_SetsInnerException()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameMessageInnerException();

            uut.InnerException.ShouldBeSameAs(context.innerException);
        }

        #endregion Constructor(propertyName, message, innerException) Tests

        /**********************************************************************/
        #region Constructor(propertyName, invalidValue) Tests

        [Test]
        public void Constructor_PropertyNameInvalidValue_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValue();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValue_Always_SetsInvalidValue()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValue();

            uut.InvalidValue.ShouldBe(context.invalidValue);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValue_Always_MessageContainsExceptionType()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValue();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValue_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValue();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(propertyName, invalidValue) Tests

        /**********************************************************************/
        #region Constructor(propertyName, invalidValue, message) Tests

        [Test]
        public void Constructor_PropertyNameInvalidValueMessage_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessage_Always_SetsInvalidValue()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            uut.InvalidValue.ShouldBe(context.invalidValue);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessage_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessage_MessageIsNotNull_SetsMessage()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessage_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(propertyName, invalidValue, message) Tests

        /**********************************************************************/
        #region Constructor(propertyName, invalidValue, message, innerException) Tests

        [Test]
        public void Constructor_PropertyNameInvalidValueMessageInnerException_Always_SetsPropertyName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessageInnerException();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessageInnerException_Always_SetsInvalidValue()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessageInnerException();

            uut.InvalidValue.ShouldBe(context.invalidValue);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessageInnerException_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessageInnerException();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessageInnerException_MessageIsNotNull_SetsMessage()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessageInnerException();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_PropertyNameInvalidValueMessageInnerException_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessageInnerException();

            uut.InnerException.ShouldBeSameAs(context.innerException);
        }

        #endregion Constructor(propertyName, invalidValue, message, innerException) Tests

        /**********************************************************************/
        #region ISerializable Tests

        [Test]
        public void ISerializable_Constructor_Always_SetsPropertyNameFromInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.PropertyName.ShouldBe(context.propertyName);
        }

        [Test]
        public void ISerializable_Constructor_Always_SetsInvalidValueFromInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.InvalidValue.ShouldBe(context.invalidValue);
        }

        [Test]
        public void ISerializable_Constructor_Always_CallsBaseConstructor()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_AddsPropertyNameToInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            var info = new SerializationInfo(typeof(PropertyValidationException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetString(nameof(PropertyValidationException.PropertyName)).ShouldBe(context.propertyName);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_AddsInvalidValueToInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            var info = new SerializationInfo(typeof(PropertyValidationException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetString(nameof(PropertyValidationException.InvalidValue)).ShouldBe(context.invalidValue);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_CallsBaseMethod()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_PropertyNameInvalidValueMessage();

            var info = new SerializationInfo(typeof(PropertyValidationException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetString(nameof(PropertyValidationException.Message)).ShouldBe(context.message);
        }

        #endregion ISerializable Tests
    }
}

using System;
using System.Runtime.Serialization;

using NUnit.Framework;
using NSubstitute;
using Shouldly;

using JV.Utilities.Exceptions;

namespace JV.Utilities.Tests.Exceptions
{
    [TestFixture]
    public class DocumentParseExceptionTests
    {
        /**********************************************************************/
        #region Test Context

        private class TestContext
        {
            public TestContext()
            {
                documentName = nameof(documentName);
                lineNumber = 1;
                linePosition = 2;
                message = nameof(message);
                innerException = new Exception();

                info = new SerializationInfo(typeof(DocumentParseException), new FormatterConverter());
                context = new StreamingContext();
                (new Exception(message)).GetObjectData(info, context);
                info.AddValue(nameof(DocumentParseException.DocumentName), documentName);
                info.AddValue(nameof(DocumentParseException.LineNumber), lineNumber);
                info.AddValue(nameof(DocumentParseException.LinePosition), linePosition);
            }

            public string documentName;
            public int lineNumber;
            public int linePosition;
            public string message;
            public Exception innerException;

            public DocumentParseException ConstructUUT_Default()
                => new DocumentParseException();

            public DocumentParseException ConstructUUT_Message()
                => new DocumentParseException(message);

            public DocumentParseException ConstructUUT_MessageInnerException()
                => new DocumentParseException(message, innerException);

            public DocumentParseException ConstructUUT_DocumentNameLineNumberLinePosition()
                => new DocumentParseException(documentName, lineNumber, linePosition);

            public DocumentParseException ConstructUUT_DocumentNameLineNumberLinePositionMessage()
                => new DocumentParseException(documentName, lineNumber, linePosition, message);

            public DocumentParseException ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException()
                => new DocumentParseException(documentName, lineNumber, linePosition, message, innerException);

            public SerializationInfo info;
            public StreamingContext context;

            public DocumentParseException ConstructUUT_ISerializable()
                => new DocumentParseException(info, context);
        }

        #endregion Test Context

        /**********************************************************************/
        #region Constructor() Tests

        [Test]
        public void Constructor_Default_Always_DocumentNameIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.DocumentName.ShouldBeNull();
        }

        [Test]
        public void Constructor_Default_Always_LineNumberIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.LineNumber.ShouldBe(default(int));
        }

        [Test]
        public void Constructor_Default_Always_LinePositionIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Default();

            uut.LinePosition.ShouldBe(default(int));
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
        #region Constructor(message) Tests

        [Test]
        public void Constructor_Message_Always_DocumentNameIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Message();

            uut.DocumentName.ShouldBeNull();
        }

        [Test]
        public void Constructor_Message_Always_LineNumberIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Message();

            uut.LineNumber.ShouldBe(default(int));
        }

        [Test]
        public void Constructor_Message_Always_LinePositionIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Message();

            uut.LinePosition.ShouldBe(default(int));
        }

        [Test]
        public void Constructor_Message_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_Message();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_Message_MessageIsNotNull_MessageEqualsGiven()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Message();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_Message_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_Message();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(message) Tests

        /**********************************************************************/
        #region Constructor(message, innerException) Tests

        [Test]
        public void Constructor_MessageInnerException_Always_DocumentNameIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.DocumentName.ShouldBeNull();
        }

        [Test]
        public void Constructor_MessageInnerException_Always_LineNumberIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.LineNumber.ShouldBe(default(int));
        }

        [Test]
        public void Constructor_MessageInnerException_Always_LinePositionIsDefault()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_MessageInnerException();

            uut.LinePosition.ShouldBe(default(int));
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
        public void Constructor_MessageInnerException_MessageIsNotNull_MessageEqualsGiven()
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
        #region Constructor(documentName, lineNumber, linePosition) Tests

        [Test]
        public void Constructor_DocumentNameLineNumberLinePosition_Always_SetsDocumentName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePosition();

            uut.DocumentName.ShouldBe(context.documentName);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePosition_Always_SetsLineNumber()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePosition();

            uut.LineNumber.ShouldBe(context.lineNumber);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePosition_Always_SetsLinePosition()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePosition();

            uut.LinePosition.ShouldBe(context.linePosition);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePosition_Always_MessageContainsExceptionType()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePosition();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePosition_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePosition();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(documentName, lineNumber, linePosition) Tests

        /**********************************************************************/
        #region Constructor(documentName, lineNumber, linePosition, message) Tests

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_Always_SetsDocumentName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.DocumentName.ShouldBe(context.documentName);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_Always_SetsLineNumber()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.LineNumber.ShouldBe(context.lineNumber);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_Always_SetsLinePosition()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.LinePosition.ShouldBe(context.linePosition);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_MessageIsNotNull_MessageEqualsGiven()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessage_Always_InnerExceptionIsNull()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            uut.InnerException.ShouldBeNull();
        }

        #endregion Constructor(documentName, lineNumber, linePosition, message) Tests

        /**********************************************************************/
        #region Constructor(documentName, lineNumber, linePosition, message, innerException) Tests

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_Always_SetsDocumentName()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.DocumentName.ShouldBe(context.documentName);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_Always_SetsLineNumber()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.LineNumber.ShouldBe(context.lineNumber);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_Always_SetsLinePosition()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.LinePosition.ShouldBe(context.linePosition);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_MessageIsNull_MessageContainsExceptionType()
        {
            var context = new TestContext()
            {
                message = null
            };
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.Message.ShouldContain(uut.GetType().Name);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_MessageIsNotNull_MessageEqualsGiven()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void Constructor_DocumentNameLineNumberLinePositionMessageInnerException_Always_SetsInnerException()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessageInnerException();

            uut.InnerException.ShouldBeSameAs(context.innerException);
        }

        #endregion Constructor(documentName, lineNumber, linePosition, message, innerException) Tests

        /**********************************************************************/
        #region ISerializable Tests

        [Test]
        public void ISerializable_Constructor_Always_SetsDocumentNameFromInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.DocumentName.ShouldBe(context.documentName);
        }

        [Test]
        public void ISerializable_Constructor_Always_SetsLineNumberFromInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.LineNumber.ShouldBe(context.lineNumber);
        }

        [Test]
        public void ISerializable_Constructor_Always_SetsLinePositionFromInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.LinePosition.ShouldBe(context.linePosition);
        }

        [Test]
        public void ISerializable_Constructor_Always_CallsBaseConstructor()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_ISerializable();

            uut.Message.ShouldBe(context.message);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_AddsDocumentNameToInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            var info = new SerializationInfo(typeof(DocumentParseException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetString(nameof(DocumentParseException.DocumentName)).ShouldBe(context.documentName);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_AddsLineNumberToInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            var info = new SerializationInfo(typeof(DocumentParseException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetInt32(nameof(DocumentParseException.LineNumber)).ShouldBe(context.lineNumber);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_AddsLinePositionToInfo()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            var info = new SerializationInfo(typeof(DocumentParseException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetInt32(nameof(DocumentParseException.LinePosition)).ShouldBe(context.linePosition);
        }

        [Test]
        public void ISerializable_GetObjectData_Always_CallsBaseMethod()
        {
            var context = new TestContext();
            var uut = context.ConstructUUT_DocumentNameLineNumberLinePositionMessage();

            var info = new SerializationInfo(typeof(DocumentParseException), new FormatterConverter());

            uut.GetObjectData(info, new StreamingContext());

            info.GetString(nameof(DocumentParseException.Message)).ShouldBe(context.message);
        }

        #endregion ISerializable Tests
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace JV.Utilities.Exceptions
{
    /// <summary>
    /// Encapsulates an error that occurred during parsing of a text document.
    /// </summary>
    [Serializable]
    public class DocumentParseException : Exception
    {
        /**********************************************************************/
        #region Constructors

        /// <summary>
        /// See <see cref="Exception"/>.
        /// </summary>
        public DocumentParseException() : base() { }

        /// <summary>
        /// See <see cref="Exception"/>.
        /// </summary>
        public DocumentParseException(string message) : base(message) { }

        /// <summary>
        /// See <see cref="Exception"/>.
        /// </summary>
        public DocumentParseException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Creates a new excpetion with the given property values.
        /// </summary>
        /// <param name="documentName">The value to use for <see cref="DocumentName"/>.</param>
        /// <param name="lineNumber">The value to use for <see cref="LineNumber"/>.</param>
        /// <param name="linePosition">The value to use for <see cref="LinePosition"/>.</param>
        public DocumentParseException(string documentName, int lineNumber, int linePosition) : base()
        {
            DocumentName = documentName;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        /// <summary>
        /// Creates a new excpetion with the given property values.
        /// </summary>
        /// <param name="documentName">The value to use for <see cref="DocumentName"/>.</param>
        /// <param name="lineNumber">The value to use for <see cref="LineNumber"/>.</param>
        /// <param name="linePosition">The value to use for <see cref="LinePosition"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        public DocumentParseException(string documentName, int lineNumber, int linePosition, string message) : base(message)
        {
            DocumentName = documentName;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        /// <summary>
        /// Creates a new excpetion with the given property values.
        /// </summary>
        /// <param name="documentName">The value to use for <see cref="DocumentName"/>.</param>
        /// <param name="lineNumber">The value to use for <see cref="LineNumber"/>.</param>
        /// <param name="linePosition">The value to use for <see cref="LinePosition"/>.</param>
        /// <param name="message">See <see cref="Exception"/>.</param>
        /// <param name="inner">See <see cref="Exception"/>.</param>
        public DocumentParseException(string documentName, int lineNumber, int linePosition, string message, Exception inner) : base(message, inner)
        {
            DocumentName = documentName;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        /// <summary>
        /// The name of the document that parsed incorrectly. Defaults to null.
        /// </summary>
        public string DocumentName { get; private set; }

        /// <summary>
        /// The 1-based index of the line within the document where the parse failed. Defaults to 0.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// The 1-based index of the position within the current line where the parse failed. Defaults to 0.
        /// </summary>
        public int LinePosition { get; private set; }

        #endregion Properties

        /**********************************************************************/
        #region ISerializable

        /// <summary>
        /// See <see cref="ISerializable"/>.
        /// </summary>
        protected internal DocumentParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            DocumentName = info.GetString(nameof(DocumentName));
            LineNumber = info.GetInt32(nameof(LineNumber));
            LinePosition = info.GetInt32(nameof(LinePosition));
        }

        /// <summary>
        /// See <see cref="ISerializable.GetObjectData(SerializationInfo, StreamingContext)"/>.
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(DocumentName), DocumentName, typeof(string));
            info.AddValue(nameof(LineNumber), LineNumber, typeof(int));
            info.AddValue(nameof(LinePosition), LinePosition, typeof(int));
        }

        #endregion ISerializable
    }
}

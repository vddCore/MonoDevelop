using System;
using System.Runtime.Serialization;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    /// <summary>
    /// The exception that is thrown attempting to reference a resource that does not exist.
    /// </summary>
    [Serializable]
    public class NotFoundException : LibGit2SharpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibGit2Sharp.NotFoundException"/> class.
        /// </summary>
        public NotFoundException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibGit2Sharp.NotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public NotFoundException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibGit2Sharp.NotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LibGit2Sharp.NotFoundException"/> class with a serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        internal NotFoundException(string message, GitErrorCode code, GitErrorCategory category)
            : base(message, code, category)
        { }
    }
}

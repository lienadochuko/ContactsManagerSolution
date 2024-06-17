namespace ContactsManager.Core.Exceptions
{
    public class InvalidPersonIDException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the CustomException class.
        /// </summary>
        public InvalidPersonIDException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the CustomException class with a specified message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public InvalidPersonIDException(string message) : base(message)
        {
        }

        /// Initializes a new instance of the CustomException class with a specified message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public InvalidPersonIDException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

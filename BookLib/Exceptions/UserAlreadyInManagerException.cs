using System;

namespace BookLib.Exceptions
{
    /// <summary>
    /// The UserAlreadyInManagerException class represents an exception that is thrown when an item is not found in a collection or when an operation fails due to the absence of an item.
    /// </summary>
    public class UserAlreadyInManagerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the UserAlreadyInManagerException class with no message.
        /// </summary>
        public UserAlreadyInManagerException() { }

        /// <summary>
        /// Initializes a new instance of the UserAlreadyInManagerException class with the specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public UserAlreadyInManagerException(string message) : base(message) { }
    }
}

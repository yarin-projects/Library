using System;

namespace BookLib.Exceptions
{
    /// <summary>
    /// The ItemNotFoundException class represents an exception that is thrown when an item is not found in a collection or when an operation fails due to the absence of an item.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ItemNotFoundException class with no message.
        /// </summary>
        public ItemNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the ItemNotFoundException class with the specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ItemNotFoundException(string message) : base(message) { }
    }
}

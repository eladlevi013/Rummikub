using System;

namespace rummikubGame.Exceptions
{
    public class EmptyPoolException : Exception
    {
        /*
            Exception class, thrown when trying to draw a tile from an empty pool.
            was created in order to avoid using the general Exception class,
            and for the easy use of the exception handeling in c#.
        */
        public EmptyPoolException() { }
        public EmptyPoolException(string message) : base(message) { }
        public EmptyPoolException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

using System;

namespace GraphEditor.Exceptions
{
    public class InvalidInputException : ApplicationException
    {
        public InvalidInputException() { }
        public InvalidInputException(string? message) : base(message) { }
    }
}

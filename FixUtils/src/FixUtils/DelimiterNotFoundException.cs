namespace FixUtils
{
    using System;

    public class DelimiterNotFoundException : Exception
    {
        public DelimiterNotFoundException() : base("Could not find fix delimeter in message")
        {
        }

        public DelimiterNotFoundException(string message) : base(message)
        {
        }

        public DelimiterNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}


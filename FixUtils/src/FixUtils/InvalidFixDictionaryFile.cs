namespace FixUtils
{
    using System;

    public class InvalidFixDictionaryFile : Exception
    {
        public InvalidFixDictionaryFile() : base("Invalid Fix Dictionary file")
        {
        }

        public InvalidFixDictionaryFile(string message) : base(message)
        {
        }

        public InvalidFixDictionaryFile(string message, Exception inner) : base(message, inner)
        {
        }
    }
}


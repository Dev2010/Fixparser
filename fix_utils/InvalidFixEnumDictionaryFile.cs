namespace fix_utils
{
    using System;

    public class InvalidFixEnumDictionaryFile : Exception
    {
        public InvalidFixEnumDictionaryFile() : base("Invalid Fix Enum Dictionary file")
        {
        }

        public InvalidFixEnumDictionaryFile(string message) : base(message)
        {
        }

        public InvalidFixEnumDictionaryFile(string message, Exception inner) : base(message, inner)
        {
        }
    }
}


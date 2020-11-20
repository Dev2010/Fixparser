namespace fix_data_dictionary
{
    using System;
    using System.Collections.Generic;

    public class FIXDataDictionaryMessage
    {
        private readonly string _name;
        private readonly string _messageCategory;
        private readonly string _messageType;
        private readonly Dictionary<string, bool> _messageEnums = new Dictionary<string, bool>();

        public FIXDataDictionaryMessage(string name, string messageCategory, string messageType)
        {
            _name = name;
            _messageCategory = messageCategory;
            _messageType = messageType;
        }

        public void AddMessageEnum(string enumStr, bool isRequired)
        {
            _messageEnums[enumStr] = isRequired;
        }

        public bool IsMessageEnumRequired(string enumStr) =>
            !_messageEnums.ContainsKey(enumStr) ? false : _messageEnums[enumStr];

        public string MessageType =>
            _messageType;

        public string MessageCategory =>
            _messageCategory;

        public string Name =>
            _name;
    }
}


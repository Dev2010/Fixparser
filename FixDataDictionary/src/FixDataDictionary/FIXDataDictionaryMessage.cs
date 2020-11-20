namespace FixDataDictionary
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
            this._name = name;
            this._messageCategory = messageCategory;
            this._messageType = messageType;
        }

        public void AddMessageEnum(string enumStr, bool isRequired)
        {
            this._messageEnums[enumStr] = isRequired;
        }

        public bool IsMessageEnumRequired(string enumStr) => 
            !this._messageEnums.ContainsKey(enumStr) ? false : this._messageEnums[enumStr];

        public string MessageType =>
            this._messageType;

        public string MessageCategory =>
            this._messageCategory;

        public string Name =>
            this._name;
    }
}


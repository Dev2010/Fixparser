namespace fix_data_dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FixDictionaryField
    {
        private readonly string _type;
        private readonly string _name;
        private readonly int _tag;
        private readonly Dictionary<string, string> _dtEnumDescription = new Dictionary<string, string>();

        public FixDictionaryField(string type, string name, int tag)
        {
            _type = type;
            _name = name;
            _tag = tag;
        }

        public void AddEnumDescription(string enumStr, string description)
        {
            if (!_dtEnumDescription.ContainsKey(enumStr))
            {
                _dtEnumDescription[enumStr] = description;
            }
        }

        public string GetDescription(string enumStr) =>
            !_dtEnumDescription.ContainsKey(enumStr) ? string.Empty : _dtEnumDescription[enumStr];

        public void UpdateEnumDescription(string enumStr, string description)
        {
            _dtEnumDescription[enumStr] = description;
        }

        public int Tag =>
            _tag;

        public string Name =>
            _name;

        public string FieldType =>
            _type;

        public int EnumDescriptionCount =>
            _dtEnumDescription.Count();
    }
}


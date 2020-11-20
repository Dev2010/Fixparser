namespace FixDataDictionary
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
            this._type = type;
            this._name = name;
            this._tag = tag;
        }

        public void AddEnumDescription(string enumStr, string description)
        {
            if (!this._dtEnumDescription.ContainsKey(enumStr))
            {
                this._dtEnumDescription[enumStr] = description;
            }
        }

        public string GetDescription(string enumStr) => 
            !this._dtEnumDescription.ContainsKey(enumStr) ? string.Empty : this._dtEnumDescription[enumStr];

        public void UpdateEnumDescription(string enumStr, string description)
        {
            this._dtEnumDescription[enumStr] = description;
        }

        public int Tag =>
            this._tag;

        public string Name =>
            this._name;

        public string FieldType =>
            this._type;

        public int EnumDescriptionCount =>
            this._dtEnumDescription.Count<KeyValuePair<string, string>>();
    }
}


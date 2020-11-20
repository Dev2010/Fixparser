namespace fix_data_dictionary
{
    using fix_utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public class FixDictionary
    {
        private readonly string _fileName;
        private readonly string _enumFileName;
        private readonly string _enumFileNameFix50Sp2;
        private readonly Dictionary<int, FixDictionaryField> _fixFieldDictionary = new Dictionary<int, FixDictionaryField>();
        private readonly Dictionary<string, FIXDataDictionaryMessage> _fixMessageNameDictionary = new Dictionary<string, FIXDataDictionaryMessage>();
        private readonly Dictionary<string, FIXDataDictionaryMessage> _fixMessageTypeDictionary = new Dictionary<string, FIXDataDictionaryMessage>();
        private string _fixType;
        private int _major;
        private int _minor;
        private int _servicePack;

        public FixDictionary(string fileName, string enumFileName, string enumFileNameFix50SP2)
        {
            _fileName = fileName;
            _enumFileName = enumFileName;
            _enumFileNameFix50Sp2 = enumFileNameFix50SP2;
        }

        public FixDictionaryField GetFixDictionaryField(int tag) =>
            _fixFieldDictionary.ContainsKey(tag) ? _fixFieldDictionary[tag] : null;

        public FIXDataDictionaryMessage GetFixMessagebyMsgType(string msgType) =>
            _fixMessageTypeDictionary.ContainsKey(msgType) ? _fixMessageTypeDictionary[msgType] : null;

        public FIXDataDictionaryMessage GetFixMessagebyName(string name) =>
            _fixMessageNameDictionary.ContainsKey(name) ? _fixMessageNameDictionary[name] : null;

        public void Load()
        {
            if (!File.Exists(_fileName))
            {
                string fileName = _fileName;
                throw new FileNotFoundException("File not found", fileName);
            }
            XElement fix = XDocument.Load(_fileName).Element("fix");
            if (fix == null)
            {
                throw new InvalidFixDictionaryFile();
            }
            ParseFixFields(fix);
            ParseFixMessageTypes(fix);
            ParseEnums(_enumFileNameFix50Sp2);
            ParseEnums(_enumFileName);
            SetMessageTypeForTag35();
        }

        private void ParseEnums(string enumFileName)
        {
            if (!File.Exists(enumFileName))
            {
                string fileName = enumFileName;
                throw new FileNotFoundException("File not found", fileName);
            }
            XElement element = XDocument.Load(enumFileName).Element("dataroot");
            if (element == null)
            {
                throw new InvalidFixEnumDictionaryFile();
            }
            foreach (var type in from fixEnum in element.Descendants("Enums")
                                 select new
                                 {
                                     Tag = fixEnum.Element("Tag").Value,
                                     EnumValue = fixEnum.Element("Enum").Value,
                                     Description = fixEnum.Element("Description").Value
                                 })
            {
                int num;
                if (int.TryParse(type.Tag, out num) && _fixFieldDictionary.ContainsKey(num))
                {
                    _fixFieldDictionary[num].UpdateEnumDescription(type.EnumValue, type.Description);
                }
            }
        }

        private void ParseFixFields(XElement fix)
        {
            XElement element = fix.Element("fields");
            if (element == null)
            {
                throw new InvalidFixDictionaryFile();
            }
            _minor = int.Parse(fix.Attribute("minor").Value);
            _major = int.Parse(fix.Attribute("major").Value);
            XAttribute attribute = fix.Attribute("servicepack");
            if (attribute != null)
            {
                _servicePack = int.Parse(attribute.Value);
            }
            XAttribute attribute2 = fix.Attribute("type");
            _fixType = attribute2 != null ? attribute2.Value : "FIX";
            foreach (var type in from field in element.Descendants("field")
                                 select new
                                 {
                                     FieldName = field.Attribute("name").Value,
                                     FieldType = field.Attribute("type").Value,
                                     FieldNumber = field.Attribute("number").Value,
                                     Children = field.Descendants("value")
                                 })
            {
                int num = int.Parse(type.FieldNumber);
                string fieldType = type.FieldType;
                string fieldName = type.FieldName;
                int tag = num;
                FixDictionaryField field = new FixDictionaryField(fieldType, fieldName, tag);
                if (type.Children != null)
                {
                    foreach (XElement element2 in type.Children)
                    {
                        string description = element2.Attribute("description") == null ? string.Empty : element2.Attribute("description").Value;
                        field.AddEnumDescription(element2.Attribute("enum") == null ? string.Empty : element2.Attribute("enum").Value, description);
                    }
                }
                _fixFieldDictionary[num] = field;
            }
        }

        private void ParseFixMessageTypes(XElement fix)
        {
            XElement element = fix.Element("messages");
            if (element == null)
            {
                throw new InvalidFixDictionaryFile();
            }
            foreach (var type in from fixMessage in element.Descendants("message")
                                 select new
                                 {
                                     MessageName = fixMessage.Attribute("name").Value,
                                     MessageCategory = fixMessage.Attribute("msgcat").Value,
                                     MessageType = fixMessage.Attribute("msgtype").Value
                                 })
            {
                string messageName = type.MessageName;
                string messageCategory = type.MessageCategory;
                string messageType = type.MessageType;
                FIXDataDictionaryMessage message = new FIXDataDictionaryMessage(messageName, messageCategory, messageType);
                _fixMessageNameDictionary[type.MessageName] = message;
                _fixMessageTypeDictionary[type.MessageType] = message;
            }
        }

        private void SetMessageTypeForTag35()
        {
            foreach (KeyValuePair<string, FIXDataDictionaryMessage> pair in _fixMessageTypeDictionary)
            {
                FIXDataDictionaryMessage message = pair.Value;
                if (_fixFieldDictionary.ContainsKey(0x23))
                {
                    _fixFieldDictionary[0x23].AddEnumDescription(message.MessageType, message.Name);
                }
            }
        }

        public string FixVersion
        {
            get
            {
                if (ServicePack <= 0)
                {
                    return $"{FixType}.{Major}.{Minor}";
                }
                return $"{FixType}.{Major}.{Minor}SP{ServicePack}";
            }
        }

        public int Major =>
            _major;

        public string FixType =>
            _fixType;

        public int Minor =>
            _minor;

        public int ServicePack =>
            _servicePack;
    }
}


namespace FixDataDictionary
{
    using FixUtils;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            this._fileName = fileName;
            this._enumFileName = enumFileName;
            this._enumFileNameFix50Sp2 = enumFileNameFix50SP2;
        }

        public FixDictionaryField GetFixDictionaryField(int tag) => 
            this._fixFieldDictionary.ContainsKey(tag) ? this._fixFieldDictionary[tag] : null;

        public FIXDataDictionaryMessage GetFixMessagebyMsgType(string msgType) => 
            this._fixMessageTypeDictionary.ContainsKey(msgType) ? this._fixMessageTypeDictionary[msgType] : null;

        public FIXDataDictionaryMessage GetFixMessagebyName(string name) => 
            this._fixMessageNameDictionary.ContainsKey(name) ? this._fixMessageNameDictionary[name] : null;

        public void Load()
        {
            if (!File.Exists(this._fileName))
            {
                string fileName = this._fileName;
                throw new FileNotFoundException("File not found", fileName);
            }
            XElement fix = XDocument.Load(this._fileName).Element("fix");
            if (fix == null)
            {
                throw new InvalidFixDictionaryFile();
            }
            this.ParseFixFields(fix);
            this.ParseFixMessageTypes(fix);
            this.ParseEnums(this._enumFileNameFix50Sp2);
            this.ParseEnums(this._enumFileName);
            this.SetMessageTypeForTag35();
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
            foreach (var type in from fixEnum in element.Descendants("Enums") select new { 
                Tag = fixEnum.Element("Tag").Value,
                EnumValue = fixEnum.Element("Enum").Value,
                Description = fixEnum.Element("Description").Value
            })
            {
                int num;
                if (int.TryParse(type.Tag, out num) && this._fixFieldDictionary.ContainsKey(num))
                {
                    this._fixFieldDictionary[num].UpdateEnumDescription(type.EnumValue, type.Description);
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
            this._minor = int.Parse(fix.Attribute("minor").Value);
            this._major = int.Parse(fix.Attribute("major").Value);
            XAttribute attribute = fix.Attribute("servicepack");
            if (attribute != null)
            {
                this._servicePack = int.Parse(attribute.Value);
            }
            XAttribute attribute2 = fix.Attribute("type");
            this._fixType = (attribute2 != null) ? attribute2.Value : "FIX";
            foreach (var type in from field in element.Descendants("field") select new { 
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
                        string description = (element2.Attribute("description") == null) ? string.Empty : element2.Attribute("description").Value;
                        field.AddEnumDescription((element2.Attribute("enum") == null) ? string.Empty : element2.Attribute("enum").Value, description);
                    }
                }
                this._fixFieldDictionary[num] = field;
            }
        }

        private void ParseFixMessageTypes(XElement fix)
        {
            XElement element = fix.Element("messages");
            if (element == null)
            {
                throw new InvalidFixDictionaryFile();
            }
            foreach (var type in from fixMessage in element.Descendants("message") select new { 
                MessageName = fixMessage.Attribute("name").Value,
                MessageCategory = fixMessage.Attribute("msgcat").Value,
                MessageType = fixMessage.Attribute("msgtype").Value
            })
            {
                string messageName = type.MessageName;
                string messageCategory = type.MessageCategory;
                string messageType = type.MessageType;
                FIXDataDictionaryMessage message = new FIXDataDictionaryMessage(messageName, messageCategory, messageType);
                this._fixMessageNameDictionary[type.MessageName] = message;
                this._fixMessageTypeDictionary[type.MessageType] = message;
            }
        }

        private void SetMessageTypeForTag35()
        {
            foreach (KeyValuePair<string, FIXDataDictionaryMessage> pair in this._fixMessageTypeDictionary)
            {
                FIXDataDictionaryMessage message = pair.Value;
                if (this._fixFieldDictionary.ContainsKey(0x23))
                {
                    this._fixFieldDictionary[0x23].AddEnumDescription(message.MessageType, message.Name);
                }
            }
        }

        public string FixVersion
        {
            get
            {
                if (this.ServicePack <= 0)
                {
                    return $"{this.FixType}.{this.Major}.{this.Minor}";
                }
                return $"{this.FixType}.{this.Major}.{this.Minor}SP{this.ServicePack}";
            }
        }

        public int Major =>
            this._major;

        public string FixType =>
            this._fixType;

        public int Minor =>
            this._minor;

        public int ServicePack =>
            this._servicePack;
    }
}


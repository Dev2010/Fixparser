namespace FixDataDictionary
{
    using System;
    using System.Collections.Generic;

    public class FixDataDictionarySingleton
    {
        private readonly Dictionary<string, FixDictionary> _fixDataDictionaries = new Dictionary<string, FixDictionary>();
        private static FixDataDictionarySingleton _instance;

        private FixDataDictionarySingleton()
        {
        }

        public FIXDataDictionaryMessage GetFieldByMsgType(string fixVersion, string msgType) => 
            this.GetFIXDictionary(fixVersion).GetFixMessagebyMsgType(msgType);

        private FixDictionary GetFIXDictionary(string fixVersion)
        {
            string str = this._fixDataDictionaries.ContainsKey(fixVersion) ? fixVersion : "FIX.4.4";
            return this._fixDataDictionaries[str];
        }

        public FixDictionaryField GetFixDictionaryFieldByName(string fixVersion, int tag) => 
            this.GetFIXDictionary(fixVersion).GetFixDictionaryField(tag);

        public FIXDataDictionaryMessage GetMessageByName(string fixVersion, string name) => 
            this.GetFIXDictionary(fixVersion).GetFixMessagebyName(name);

        public void Load(string fileName, string enumFileName, string enumFileNameFix50SP2)
        {
            string str = enumFileName;
            FixDictionary dictionary = new FixDictionary(fileName, str, enumFileNameFix50SP2);
            dictionary.Load();
            this._fixDataDictionaries[dictionary.FixVersion] = dictionary;
        }

        public static FixDataDictionarySingleton Instance =>
            _instance ?? (_instance = new FixDataDictionarySingleton());
    }
}


namespace fix_data_dictionary
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
            GetFIXDictionary(fixVersion).GetFixMessagebyMsgType(msgType);

        private FixDictionary GetFIXDictionary(string fixVersion)
        {
            string str = _fixDataDictionaries.ContainsKey(fixVersion) ? fixVersion : "FIX.4.4";
            return _fixDataDictionaries[str];
        }

        public FixDictionaryField GetFixDictionaryFieldByName(string fixVersion, int tag) =>
            GetFIXDictionary(fixVersion).GetFixDictionaryField(tag);

        public FIXDataDictionaryMessage GetMessageByName(string fixVersion, string name) =>
            GetFIXDictionary(fixVersion).GetFixMessagebyName(name);

        public void Load(string fileName, string enumFileName, string enumFileNameFix50SP2)
        {
            string str = enumFileName;
            FixDictionary dictionary = new FixDictionary(fileName, str, enumFileNameFix50SP2);
            dictionary.Load();
            _fixDataDictionaries[dictionary.FixVersion] = dictionary;
        }

        public static FixDataDictionarySingleton Instance =>
            _instance ?? (_instance = new FixDataDictionarySingleton());
    }
}


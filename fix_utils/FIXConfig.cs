namespace fix_utils
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Xml.Linq;

    public sealed class FIXConfig
    {
        private static FIXConfig _instance;
        private bool _isConfigLoaded;
        private readonly Dictionary<string, string> _dtConfigurations = new Dictionary<string, string>();

        private FIXConfig()
        {
        }

        public string GetConfiguration(string name)
        {
            if (!_dtConfigurations.ContainsKey(name))
            {
                throw new ConfigurationErrorsException($"Configuration key {name} not found");
            }
            return _dtConfigurations[name];
        }

        public void Load(string fileName)
        {
            if (_isConfigLoaded)
            {
                _isConfigLoaded = true;
            }
            else
            {
                XDocument document = XDocument.Load(fileName);
                _dtConfigurations.Clear();
                _isConfigLoaded = true;
                foreach (var type in from configuration in document.Element("FIXConfig").Descendants("ConfigElement")
                                     select new
                                     {
                                         ConfigName = configuration.Attribute("name").Value,
                                         ConfigValue = configuration.Attribute("value").Value
                                     })
                {
                    _dtConfigurations[type.ConfigName] = type.ConfigValue;
                }
            }
        }

        public static FIXConfig Instance =>
            _instance ?? (_instance = new FIXConfig());
    }
}


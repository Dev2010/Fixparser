namespace FixUtils
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
            if (!this._dtConfigurations.ContainsKey(name))
            {
                throw new ConfigurationErrorsException($"Configuration key {name} not found");
            }
            return this._dtConfigurations[name];
        }

        public void Load(string fileName)
        {
            if (this._isConfigLoaded)
            {
                this._isConfigLoaded = true;
            }
            else
            {
                XDocument document = XDocument.Load(fileName);
                this._dtConfigurations.Clear();
                this._isConfigLoaded = true;
                foreach (var type in from configuration in document.Element("FIXConfig").Descendants("ConfigElement") select new { 
                    ConfigName = configuration.Attribute("name").Value,
                    ConfigValue = configuration.Attribute("value").Value
                })
                {
                    this._dtConfigurations[type.ConfigName] = type.ConfigValue;
                }
            }
        }

        public static FIXConfig Instance =>
            _instance ?? (_instance = new FIXConfig());
    }
}


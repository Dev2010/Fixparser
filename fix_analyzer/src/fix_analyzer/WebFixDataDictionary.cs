namespace fix_analyzer
{
    using FixDataDictionary;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Configuration;

    public class WebFixDataDictionary
    {
        public FixDataDictionarySingleton GetFixDataDictionary()
        {
            if (HttpContext.Current.Cache["DataDictionary"] == null)
            {
                string str = WebConfigurationManager.AppSettings["FixSpecDir"];
                string str2 = WebConfigurationManager.AppSettings["FixEnumDir"];
                string str3 = Path.Combine(str2, @"FIX.5.0SP2\Base\Enums.xml");
                string fileName = Path.Combine(str, "FIXT11.xml");
                FixDataDictionarySingleton.Instance.Load(fileName, Path.Combine(str2, @"FIXT.1.1\Base\Enums.xml"), str3);
                fileName = Path.Combine(str, "FIX40.xml");
                FixDataDictionarySingleton.Instance.Load(fileName, Path.Combine(str2, @"FIX.4.0\Base\Enums.xml"), str3);
                string enumFileName = Path.Combine(str2, @"FIX.4.1\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX41.xml"), enumFileName, str3);
                string str8 = Path.Combine(str2, @"FIX.4.2\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX42.xml"), str8, str3);
                string str9 = Path.Combine(str2, @"FIX.4.3\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX43.xml"), str9, str3);
                string str10 = Path.Combine(str2, @"FIX.4.4\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX44.xml"), str10, str3);
                string str11 = Path.Combine(str2, @"FIX.5.0\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX50.xml"), str11, str3);
                string str12 = Path.Combine(str2, @"FIX.5.0SP1\Base\Enums.xml");
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX50SP1.xml"), str12, str3);
                FixDataDictionarySingleton.Instance.Load(Path.Combine(str, "FIX50SP2.xml"), str3, str3);
                HttpContext.Current.Cache["DataDictionary"] = FixDataDictionarySingleton.Instance;
            }
            return (FixDataDictionarySingleton) HttpContext.Current.Cache["DataDictionary"];
        }
    }
}


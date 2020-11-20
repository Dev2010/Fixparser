namespace fix_analyzer
{
    using fix_analyzer.Models;
    using fix_parser;
    using log4net;
    using System;

    public class FixProcessing
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FixMessageLoad ProcessFixMessageLoad(FixMessageLoad fixMessageLoad)
        {
            if (fixMessageLoad == null)
            {
                fixMessageLoad = new FixMessageLoad();
            }
            if (string.IsNullOrWhiteSpace(fixMessageLoad.FIXMessages))
            {
                fixMessageLoad.FIXMessages = string.Empty;
            }
            if (fixMessageLoad.FIXMessages.Length > 0x5dd)
            {
                fixMessageLoad.FIXMessages = fixMessageLoad.FIXMessages.Substring(0, 0x5dc);
            }
            string[] separator = new string[] { "\r\n", "\n", "\r" };
            foreach (string str in fixMessageLoad.FIXMessages.Split(separator, StringSplitOptions.None))
            {
                if (!string.IsNullOrWhiteSpace(str))
                {
                    try
                    {
                        FixMessageParser item = new FixMessageParser(str);
                        item.Parse();
                        fixMessageLoad.FixMessageParsers.Add(item);
                    }
                    catch (Exception exception2)
                    {
                        Logger.Error("Unhandled exception", exception2);
                    }
                }
            }
            return fixMessageLoad;
        }
    }
}


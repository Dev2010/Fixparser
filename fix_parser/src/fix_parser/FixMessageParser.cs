namespace fix_parser
{
    using FixUtils;
    using log4net;
    using QuickFix;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    public class FixMessageParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _fixMessage;
        private string _cleanedUpFixMessage = string.Empty;
        private bool _error;
        private char _delimiter;
        private FixField _messageType;
        private readonly List<string> _errorList = new List<string>();
        private readonly List<FixField> _fixFields = new List<FixField>();

        public FixMessageParser(string fixMessage)
        {
            this._fixMessage = fixMessage;
        }

        public bool CleanUpFixMessage()
        {
            if (this.FIXMessage.Length > 0xbb8)
            {
                this.ErrorList.Add("Fix Message too long");
                return true;
            }
            int length = "8=FIX".Length;
            this._fixMessage = this.FIXMessage.Trim();
            if (this.FIXMessage.IndexOf("8=FIX", StringComparison.Ordinal) == 0)
            {
                this._cleanedUpFixMessage = this.FIXMessage;
            }
            else
            {
                string[] separator = new string[] { "8=FIX" };
                string[] strArray = this.FIXMessage.Split(separator, length, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length != 2)
                {
                    this.ErrorList.Add("Invalid Fix Message");
                    return true;
                }
                this._cleanedUpFixMessage = $"8=FIX{strArray[1]}";
            }
            return false;
        }

        public char GetDelimiter(string line, out string fixVersion)
        {
            fixVersion = string.Empty;
            int length = "8=FIX".Length;
            int num2 = CultureInfo.InvariantCulture.CompareInfo.IndexOf(line, "8=FIX", CompareOptions.IgnoreCase);
            if (num2 < 0)
            {
                throw new DelimiterNotFoundException();
            }
            int index = line.IndexOf('=', length);
            if (index < 0)
            {
                throw new DelimiterNotFoundException();
            }
            int num4 = index;
            while (true)
            {
                if (index > (num2 + length))
                {
                    num4--;
                    if (char.IsNumber(line[num4]))
                    {
                        continue;
                    }
                }
                if (num4 >= index)
                {
                    throw new DelimiterNotFoundException();
                }
                fixVersion = line.Substring(num2 + (length - 3), ((num4 - num2) - length) + 3);
                return line[num4];
            }
        }

        public void Parse()
        {
            try
            {
                if (this.CleanUpFixMessage())
                {
                    this._error = true;
                }
                else
                {
                    string str;
                    this._delimiter = this.GetDelimiter(this.CleanedUpFixMessage, out str);
                    char[] separator = new char[] { this._delimiter };
                    foreach (string str2 in this._cleanedUpFixMessage.Split(separator))
                    {
                        int num2;
                        char[] trimChars = new char[] { '\r', '\n', '\t' };
                        string str3 = str2.Trim(trimChars);
                        str3 = str2.Trim();
                        int index = str3.IndexOf("=", StringComparison.Ordinal);
                        if (index <= 0)
                        {
                            break;
                        }
                        string s = str3.Substring(0, index);
                        string str5 = str3.Substring(index + 1);
                        if (!int.TryParse(s, out num2))
                        {
                            this.ErrorList.Add($"Invalid tag {s} with value {str5}");
                        }
                        else
                        {
                            FixField item = new FixField(str, num2, str5);
                            item.Initialize();
                            this._fixFields.Add(item);
                            if (num2 == 0x23)
                            {
                                this._messageType = item;
                            }
                        }
                    }
                }
            }
            catch (DelimiterNotFoundException exception)
            {
                this._error = true;
                Exception exception3 = exception;
                Logger.Error("Delimiter not found", exception3);
            }
            catch (Exception exception2)
            {
                string item = $"Failed to parse fix message {exception2}";
                this.ErrorList.Add(item);
                this._error = true;
                Logger.Error(item, exception2);
            }
        }

        public Message ParseFile(string fileName)
        {
            string str = new StreamReader(fileName).ReadLine();
            if (str != null)
            {
                string str2;
                this._fixMessage = str;
                if (this.CleanUpFixMessage())
                {
                    return null;
                }
                this._delimiter = this.GetDelimiter(this.CleanedUpFixMessage, out str2);
                char[] separator = new char[] { this._delimiter };
                this._cleanedUpFixMessage.Split(separator);
            }
            return new Message(this._cleanedUpFixMessage);
        }

        public bool Error =>
            this._error;

        public string CleanedUpFixMessage =>
            this._cleanedUpFixMessage;

        public List<string> ErrorList =>
            this._errorList;

        public List<FixField> FixFields =>
            this._fixFields;

        public string FIXMessage =>
            this._fixMessage;

        public FixField MessageType =>
            this._messageType;
    }
}


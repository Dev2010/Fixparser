﻿namespace fix_analyzer.Models
{
    using fix_parser;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FixMessageLoad
    {
        public List<FixMessageParser> FixMessageParsers = new List<FixMessageParser>();

        public string FIXMessages { get; set; }
    }
}


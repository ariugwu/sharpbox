﻿using System;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames : EnumPattern
    {
        public CommandNames(string value)
            : base(value)
        {
        }

        public CommandNames() { }

        public int CommandNameId { get; set; }
        public string Value { get { return _value; } }
    }
}

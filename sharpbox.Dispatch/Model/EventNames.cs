﻿using System;
using sharpbox.Util.Enum;

namespace sharpbox.Dispatch.Model
{
    /// <summary>
    /// As a rule there are events and requests. "Events" begin with "On" and have a One-To-Many relationship. Anything else is a request and should generally have a One-to-One relationship
    /// </summary>
    [Serializable]
    public class EventNames : EnumPattern
    {



        public EventNames(string value) : base(value)
        {
        }

        public EventNames() { }

    }
}

﻿using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Dispatch.Model
{
    using sharpbox.Common.Dispatch;

    [Serializable]
    public class RoutineItem : IRoutineItem
    {
        public CommandName CommandName { get; set; }
        public EventName EventName { get; set; }
        public Delegate Action { get; set; }
        public Delegate FailOver { get; set; }
        public Delegate Rollback { get; set; }

        public string BroadCastMessage { get; set; }
    }
}

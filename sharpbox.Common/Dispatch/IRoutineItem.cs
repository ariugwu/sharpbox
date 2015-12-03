using System;

namespace sharpbox.Common.Dispatch
{
    using Model;

    public interface IRoutineItem
    {
        CommandName CommandName { get; set; }

        EventName EventName { get; set; }

        Delegate Action { get; set; }

        Delegate FailOver { get; set; }

        Delegate Rollback { get; set; }

        string BroadCastMessage { get; set; }
    }
}

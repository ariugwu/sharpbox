using System;

namespace sharpbox.Common.Dispatch
{
    using Model;

    public interface ICommandHubItem
    {
        Delegate Action { get; set; }

        EventName EventName { get; set; }
    }
}

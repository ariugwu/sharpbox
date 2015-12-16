namespace sharpbox.Dispatch.Model
{
    using System;

    public interface ICommandHubItem
    {
        Delegate Action { get; set; }

        EventName EventName { get; set; }
    }
}

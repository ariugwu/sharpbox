namespace sharpbox.Dispatch.Model
{
    using System;

    public interface IResponse
    {
        Guid ResponseId { get; set; }

        string Message { get; set; }

        int EventNameId { get; set; }

        EventName EventName { get; set; }

        Guid RequestId { get; set; }

        IRequest Request { get; set; }

        string UserId { get; set; }

        DateTime CreatedDate { get; set; }

        Guid? EnvironmentId { get; set; }

        object Entity { get; set; }

        Type Type { get; set; }

        ResponseTypes ResponseType { get; set; }
    }
}

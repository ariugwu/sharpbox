namespace sharpbox.Dispatch.Model
{
    using System;

    public interface IRequest
    {
        Guid RequestId { get; set; }

        string Message { get; set; }

        Guid CommandNameId { get; set; }

        CommandName CommandName { get; set; }

        Delegate Action { get; set; }

        DateTime CreatedDate { get; set; }

        object Entity { get; set; }

        Type Type { get; set; }
    }
}

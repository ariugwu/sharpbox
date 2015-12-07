using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Dispatch.Model
{
    using sharpbox.Common.Data;
    using sharpbox.Common.Dispatch;

    [Serializable]
    public class Request : BasePackage, IRequest
    {
        public Request()
        {
            this.CreatedDate = DateTime.Now;
            this.RequestId = Guid.NewGuid();
        }

        public Guid RequestId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega

        public string Message { get; set; }
        public Guid CommandNameId { get; set; }
        public CommandName CommandName { get; set; }
        public Delegate Action { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid? EnvironmentId { get; set; }

        public static Request Create<T>(CommandName commandName, string message, T entity)
        {
            return new Request()
            {
                RequestId = Guid.NewGuid(),
                CommandName = commandName,
                Message = message,
                Entity = entity,
                Type = entity != null ? entity.GetType() : null,
                CreatedDate = DateTime.Now
            };
        }
    }
}

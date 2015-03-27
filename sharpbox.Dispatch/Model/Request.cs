using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Request : BasePackage
    {
        public Request()
        {
            CreatedDate = DateTime.Now;
        }

        public int RequestId { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        public Guid RequestUniqueKey { get; set; }
        
        public string Message { get; set; }
        public int CommandNameId { get; set; }
        public CommandNames CommandName { get; set; }
        public Delegate Action { get; set; }
        public DateTime CreatedDate { get; set; }

        public static Request Create<T>(CommandNames commandName, string message, T entity)
        {
            return new Request()
            {  
                RequestUniqueKey = Guid.NewGuid(),
                CommandName = commandName,
                Message = String.Format("[Invoke Command: {0}] [Message: {1}]",commandName, message),
                Entity = entity,
                Type = entity != null? entity.GetType() : null,
                CreatedDate = DateTime.Now
            };
        }
    }
}

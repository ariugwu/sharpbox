using System;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class Request
    {
        public Request()
        {
            
        }
        public Guid RequestId { get; set; }
        public int RequestClusteringKey { get; set; } // @SEE http://stackoverflow.com/questions/11938044/what-are-the-best-practices-for-using-a-guid-as-a-primary-key-specifically-rega
        public string Message { get; set; }
        public CommandNames CommandName { get; set; }
        public Delegate Action { get; set; }
        public object Entity { get; set; }
        public Type Type { get; set; }

        public static Request Create<T>(CommandNames commandName, string message, T entity)
        {
            return new Request()
            {
                RequestId = Guid.NewGuid(),
                CommandName = commandName,
                Message = message,
                Entity = entity,
                Type = entity != null? entity.GetType() : null
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Common.Dispatch
{
    using sharpbox.Common.Dispatch.Model;

    using Type = System.Type;

    public interface IResponse
    {
        int ResponseId { get; set; }

        string Message { get; set; }

        int EventNameId { get; set; }

        EventName EventName { get; set; }

        int RequestId { get; set; }

        IRequest Request { get; set; }

        string UserId { get; set; }

        DateTime CreatedDate { get; set; }

        Guid? EnvironmentId { get; set; }

        object Entity { get; set; }

        Type Type { get; set; }

        ResponseTypes ResponseType { get; set; }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        string SerializedEntity { get; set; }

        /// <summary>
        /// Used only for EF. @SEE: http://stackoverflow.com/a/14785553
        /// </summary>
        string SerializeEntityType { get; set; }

        void DeserializeEntity();

        void DeserializeEntityType();
    }
}

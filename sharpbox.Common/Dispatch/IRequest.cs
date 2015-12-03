using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Common.Dispatch
{
    using sharpbox.Common.Dispatch.Model;

    public interface IRequest
    {
        int RequestId { get; set; }

        string Message { get; set; }

        int CommandNameId { get; set; }

        CommandName CommandName { get; set; }

        Delegate Action { get; set; }

        DateTime CreatedDate { get; set; }

        Guid? EnvironmentId { get; set; }

        object Entity { get; set; }

        System.Type Type { get; set; }

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

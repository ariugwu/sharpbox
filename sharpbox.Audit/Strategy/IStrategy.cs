using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public interface IStrategy
    {
        List<Response> Trail { get; set; }

        /// <summary>
        /// Used as a callback to record
        /// </summary>
        /// <param name="response"></param>
        void Record(Response response);

    }
}

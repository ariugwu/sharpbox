using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public interface IStrategy
    {
        List<Package> Trail { get; set; }

        /// <summary>
        /// Used as a callback to record
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="package"></param>
        void RecordDispatch(Dispatch.Client dispatcher,Package package);

        void RecordDispatch(Dispatch.Client dispatcher, Request request);
    }
}

using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public interface IStrategy<T>
    {
        List<T> Entries { get; set; }

        void RecordDispatch(Dispatch.Client dispatcher,Package package);

        void RecordDispatch(Dispatch.Client dispatcher, Request request);

        void Load(Dispatch.Client dispatcher);

        void Save(Dispatch.Client dispatcher);

        T Create(Dispatch.Client dispatcher, T e);

        T Get(Dispatch.Client dispatcher, int id);
    }
}

using System.Collections.Generic;
using sharpbox.Audit.Model;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public interface IStrategy
    {
        List<Entry> Entries { get; set; }

        void RecordDispatch(Package package);

        void Load();

        void Save();

        Entry Create(Entry e);

        Entry Get(int id);
    }
}

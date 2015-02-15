using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public interface IStrategy
    {
        List<Package> Entries { get; set; }

        void RecordDispatch(Package package);

        void Load();

        void Save();

        Package Create(Package e);

        Package Get(int id);
    }
}

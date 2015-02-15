using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public class BaseStrategy : IStrategy
    {
        public List<Package> Entries { get; set; }

        public void RecordDispatch(Package package)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public Package Create(Package e)
        {
            throw new NotImplementedException();
        }

        public Package Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}

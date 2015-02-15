using System;
using System.Collections.Generic;
using sharpbox.Audit.Model;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public class BaseStrategy : IStrategy
    {
        public List<Entry> Entries { get; set; }

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

        public Entry Create(Entry e)
        {
            throw new NotImplementedException();
        }

        public Entry Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}

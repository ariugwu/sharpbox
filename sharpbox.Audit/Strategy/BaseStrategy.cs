using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public class BaseStrategy<T> : IStrategy<T> where T : class
    {
        #region Constructor(s)

        public BaseStrategy(Dispatch.Client dispatcher)
        {
            var auxInfo = new Dictionary<string, object> { { "xmlPath", "AuditXmlRepository.xml" } };
            _repository = new Repository<Package>(dispatcher, auxInfo: auxInfo);

            Load(dispatcher);
        }

        #endregion

        #region Field(s)

        private string _xmlPath;
        private Repository<Package> _repository;

        #endregion

        public Dictionary<string, object> AuxInfo { get; set; }

        public Repository<Package> Repository
        {
            get { return _repository; }
            set
            {
                _repository = value;
            }
        }

        public List<T> Entries { get; set; }

        public void RecordDispatch(Dispatch.Client dispatcher, Package package)
        {
            Repository.Create(dispatcher, package);
            Load(dispatcher);
        }

        public void Load(Dispatch.Client dispatcher)
        {
            Entries = Repository.All(dispatcher).ToList() as List<T>;
        }

        public void Save(Dispatch.Client dispatcher)
        {
            Repository.UpdateAll(dispatcher,Entries as List<Package>);
            Load(dispatcher);
        }

        public T Create(Dispatch.Client dispatcher, T e)
        {
            var entity = Repository.Create(dispatcher, e as Package) as T; // throw it to the repo as Package and return to the Client as T. Since this is our strategy we know it will always be package.
            Load(dispatcher);
            return entity;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            throw new NotImplementedException();
        }
    }
}

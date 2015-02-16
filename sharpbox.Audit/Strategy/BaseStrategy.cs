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

        public BaseStrategy()
        {
            Load();
        }

        #endregion

        #region Field(s)

        private Repository<Package> _repository;

        #endregion

        public Repository<Package> Repository
        {
            get
            {
                var auxInfo = new Dictionary<string, object> {{"xmlPath", "AuditXmlRepository.xml"}};
                return _repository ?? (_repository = new Repository<Package>(auxInfo: auxInfo));
            }
            set
            {
                _repository = value;
            }
        }

        public List<T> Entries { get; set; }

        public void RecordDispatch(Dispatch.Client dispatcher, Package package)
        {
            Repository.Create(dispatcher, package);
            Load();
        }

        public void Load()
        {
            Entries = Repository.All().ToList() as List<T>;
        }

        public void Save(Dispatch.Client dispatcher)
        {
            Repository.UpdateAll(dispatcher,Entries as List<Package>);
            Load();
        }

        public T Create(Dispatch.Client dispatcher, T e)
        {
            var entity = Repository.Create(dispatcher, e as Package) as T; // throw it to the repo as Package and return to the Client as T. Since this is our strategy we know it will always be package.
            Load();
            return entity;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            throw new NotImplementedException();
        }
    }
}

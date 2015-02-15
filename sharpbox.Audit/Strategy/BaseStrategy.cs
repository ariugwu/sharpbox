using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public class BaseStrategy<T> : IStrategy<T> where T : class
    {
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
            _repository.Create(dispatcher, package);
        }

        public void Load()
        {
            Entries = _repository.All().ToList() as List<T>;
        }

        public void Save(Dispatch.Client dispatcher)
        {
            _repository.UpdateAll(dispatcher,Entries as List<Package>);
        }

        public T Create(Dispatch.Client dispatcher, T e)
        {
            return _repository.Create(dispatcher, e as Package) as T; // throw it to the repo as Package and return to the Client as T. Since this is our strategy we know it will always be package.
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            throw new NotImplementedException();
        }
    }
}

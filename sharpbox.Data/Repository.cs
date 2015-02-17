using System.Collections.Generic;
using sharpbox.Data.Strategy;
using sharpbox.Dispatch.Model;

namespace sharpbox.Data
{
    public class Repository<T>
    {
        #region Field(s)

        private IStrategy<T> _strategy;

        #endregion

        #region Properties

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auxInfo">General use dictionary for anything a strategy might need. For xml it's the key "xmlPath", and for EF it's the key "dbContext"</param>
        public Repository(Dispatch.Client dispatcher, IStrategy<T> strategy = null, Dictionary<string, object> auxInfo = null)
        {
            _strategy = strategy ?? new XmlStrategy<T>(dispatcher, auxInfo);
            _strategy.AuxInfo = auxInfo ?? new Dictionary<string, object>();
        }

        public Repository()
        {

        }

        #endregion

        #region Strategy Method(s)

        public IEnumerable<T> All(Dispatch.Client dispatcher)
        {
            return _strategy.All(dispatcher);
        }

        public T Create(Dispatch.Client dispatcher, T entity)
        {
            var e = _strategy.Create(dispatcher, entity);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnDataCreate, Message = "Entity created.", Entity = e, Type = this.GetType(), PackageId = 0, UserId = "System" });
            return e;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            var e = _strategy.Get(dispatcher, id);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnDataCreate, Message = "Entity retrieved", Entity = e, Type = this.GetType(), PackageId = 0, UserId = "System" });
            return e;
        }

        public T Update(Dispatch.Client dispatcher, T entity)
        {
            var e = _strategy.Update(dispatcher, entity);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnDataCreate, Message = "Entity updated", Entity = e, Type = this.GetType(), PackageId = 0, UserId = "System" });
            return e;
        }

        public IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> entities)
        {
            var e = _strategy.UpdateAll(dispatcher, entities);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnDataCreate, Message = "All entities updated", Entity = e, Type = this.GetType(), PackageId = 0, UserId = "System" });
            return e;
        }

        public void Delete(Dispatch.Client dispatcher, T entity)
        {
            _strategy.Delete(dispatcher, entity);
            dispatcher.Publish(new Package() { PublisherName = PublisherNames.OnDataCreate, Message = "Entity deleted", Entity = entity, Type = this.GetType(), PackageId = 0, UserId = "System" });
        }

        #endregion
    }
}

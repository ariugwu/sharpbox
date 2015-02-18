using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using sharpbox.Dispatch;

namespace sharpbox.Data.Strategy
{
    public class EntityFrameworkStrategy<T> : IStrategy<T> where T : class
    {
        #region Constructor(s)

        public EntityFrameworkStrategy(Dispatch.Client dispatcher, Dictionary<string, object> props)
        {
            Db = (DbContext) props["dbContext"];
            Props = props;
        }

        #endregion

        #region Properties

        public Dictionary<string, object> Props { get; set; }
        public List<T> Entities { get; set; }
        public DbContext Db { get; set; }
        #endregion

        #region Interface Methods / Members
        public void Init(Client dispatcher)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> All(Client dispatcher)
        {
            return Db.Set<T>();
        }

        public T Create(Client dispatcher, T entity)
        {
            Db.Set<T>().AddOrUpdate(entity);

            return entity;
        }

        public T Get(Client dispatcher, int id)
        {
            return Db.Set<T>().Find(id);
        }

        public T Update(Client dispatcher, T entity)
        {
            return Create(dispatcher, entity);
        }

        public IEnumerable<T> UpdateAll(Client dispatcher, IEnumerable<T> list)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Client dispatcher, T entity)
        {
            Db.Set<T>().Remove(entity);
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace sharpbox.Data.Strategy.EntityFramework
{
    public class EfStrategy<T> : IStrategy<T> where T : class
    {
        #region Constructor(s)

        public EfStrategy(Dictionary<string, object> props)
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

        public IEnumerable<T> All()
        {
            return Db.Set<T>();
        }

        public T Create(T entity)
        {
            Db.Set<T>().AddOrUpdate(entity);

            return entity;
        }

        public T Get(int id)
        {
            return Db.Set<T>().Find(id);
        }

        public T Update(T entity)
        {
            return Create(entity);
        }

        public IEnumerable<T> UpdateAll(IEnumerable<T> list)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T entity)
        {
            Db.Set<T>().Remove(entity);
        }

        #endregion
    }
}

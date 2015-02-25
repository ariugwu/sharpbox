using System.Collections.Generic;
using sharpbox.Data.Strategy;

namespace sharpbox.Data
{
    public class Repository<T>
    {

        private IStrategy<T> _strategy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategy">A persistence strategy is required.</param>
        public Repository(IStrategy<T> strategy)
        {
            _strategy = strategy;
        }

        public Repository()
        {

        }

        public IEnumerable<T> All()
        {
            return _strategy.All();
        }

        public T Create(T entity)
        {
            var e = _strategy.Create(entity);
            return e;
        }

        public T Get(int id)
        {
            var e = _strategy.Get(id);
            return e;
        }

        public T Update(T entity)
        {
            var e = _strategy.Update(entity);
            return e;
        }

        public IEnumerable<T> UpdateAll(IEnumerable<T> entities)
        {
            var e = _strategy.UpdateAll(entities);
            return e;
        }

        public void Delete(T entity)
        {
            _strategy.Delete(entity);

        }

    }
}
